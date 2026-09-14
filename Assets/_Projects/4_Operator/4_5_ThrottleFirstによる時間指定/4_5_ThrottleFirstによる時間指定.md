# 4-5. ThrottleFirstによる時間指定

## この回で学ぶこと
- `ThrottleFirst` オペレーターで、一定時間内の連打・連続入力を制限する方法
- ボタンの `OnClickAsObservable()` を Observable として扱う方法
- 「最初の入力を通し、その後の一定時間は無視する」という時間制御の考え方

## 前提知識
- 4-1〜4-4 で学んだ `Where`・`Select`・`Take`・`TakeWhile` の基本的なオペレーターの使い方
- 3-2 / 4-3 / 4-4 で繰り返し登場した `BindText` のキャッシュパターン
- UI の `Button` コンポーネントの基本操作

## 概念解説
ゲームでは「連打防止」がよく必要になります。例えば決定ボタンを連打されて、同じ処理が何度も走ってしまうと不具合の元になります。`ThrottleFirst` は、まさにこの連打防止のためのオペレーターです。

ベルトコンベアで例えると、`ThrottleFirst` は「一度材料を通したら、タイマーが切れるまで次の材料を止めておく検問所」です。最初の1個はすぐに通しますが、指定した時間が経過するまでの間に来た材料はすべて無視され、時間が経過した後に初めて次の1個を通します。

```csharp
_button.OnClickAsObservable()
    .ThrottleFirst(TimeSpan.FromSeconds(_waitSeconds)) // クリックされてから1秒間は何もしない
    .Subscribe(_ => _count.Value++)
    .AddTo(this);
```

`_button.OnClickAsObservable()` はボタンのクリックイベントを Observable に変換したものです。ここに `ThrottleFirst(TimeSpan.FromSeconds(1))` を挟むことで、「クリックされたら即座に1回だけ通し、その後1秒間のクリックはすべて無視する」という挙動になります。

## シーンの操作方法
1. Play してシーンを実行します。
2. 画面のボタンを何度も素早く連打してみましょう。
3. カウントを表示しているテキストが、連打してもすぐには増えず、**約1秒に1回**のペースでしか増えないことを確認します。
4. Hierarchy の `Scripts` オブジェクトの `SampleOperator` コンポーネントにある `Wait Seconds` の値を変更すると、この間隔を調整できることも確認しておきましょう（デフォルトは1秒）。

## コード解説
```csharp
public class SampleOperator : MonoBehaviour, ITextBinder
{
    [SerializeField] private float _waitSeconds = 1;
    [SerializeField] private Button _button;

    private readonly ReactiveProperty<int> _count = new(0);
    private ReadOnlyReactiveProperty<string> _bindText;
    public ReadOnlyReactiveProperty<string> BindText => _bindText ??=
        _count.Select(x => x.ToString()).ToReadOnlyReactiveProperty().AddTo(this);

    public void Start()
    {
        _count.AddTo(this);

        _button.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(_waitSeconds))
            .Subscribe(_ => _count.Value++)
            .AddTo(this);
    }
}
```
- `_waitSeconds` は Inspector から調整できる待機時間です。実運用でも「連打防止の間隔をデザイナーが調整できるようにする」場合によく使われる形です。
- `ThrottleFirst` という名前の `First` の意味に注目してください。「一定時間内で**最初**に来たイベントを採用し、それ以降はタイマーが切れるまで捨てる」という動作です。似た名前のオペレーターに `Debounce`（次回学習）がありますが、動作が逆に近いので混同しないようにしましょう。
- `_count` はここでも `BindText` を通じて画面に表示されており、キャッシュパターン（`??=` と `.AddTo(this)`）は 3-2・4-3・4-4 と全く同じ形です。この教材で何度も同じ書き方が出てくるのは、それだけ実務でも頻出するパターンだからです。

## 実行結果の例
- ボタンを1回クリック：カウントが即座に `1` 増える
- クリック直後（1秒未満）にさらに連打：カウントは変化しない
- 最初のクリックから1秒以上経過してから再度クリック：カウントがまた `1` 増える

## よくあるつまずきポイント
- `ThrottleFirst` を「クリックしてから1秒待ってからカウントが増える」と誤解しがちですが、実際には**最初のクリックは即座に反映**され、その後の1秒間だけクリックが無視されます。
- 連打防止の効果を確認するには、実際に指を使って素早く連打する必要があります。ゆっくりクリックすると毎回反映されてしまい、効果が確認できないので注意してください。
- `_waitSeconds` を極端に短くする（例：0.01秒）と、通常のクリック間隔でもほぼ毎回反映されてしまうため、効果が分かりにくくなります。まずはデフォルトの1秒で挙動を確認しましょう。

## 理解度チェック
1. `ThrottleFirst` は「一定時間内で最初のイベント」と「一定時間内で最後のイベント」のどちらを採用するオペレーターですか。
2. ボタンを1秒間に5回連打した場合、`ThrottleFirst(TimeSpan.FromSeconds(1))` を通すと、カウントは何回増えますか（クリックのタイミングによって変わりますが、最大何回増え得るか考えてください）。
3. `_waitSeconds` を `SerializeField` にしていることのメリットを説明してください。
