# 4-8. Switchによる購読切り替え

## この回で学ぶこと
- `Select` で「Observable を流す Observable」を作り、`Switch` で**最新の Observable だけ**を購読する方法
- 新しい Observable が来たとき、それまで購読していた Observable が**自動的に破棄される**という動き
- 「ボタンを押すたびに処理をやり直す」タイプの仕組み（リスタート）を Observable で書く考え方

## 前提知識
- 4-2 で学んだ `Select`（値を別の値に変換する）
- 4-5 で学んだ `OnClickAsObservable()`（ボタンのクリックを Observable として扱う）
- 4-3 で使った `Observable.Interval`（一定間隔で通知を流す）と、`BindText` のキャッシュパターン（`??=` と `.AddTo(this)`）
- 4-7 で登場した `AwaitOperation.Switch`（今回の `Switch` と考え方が同じ。あとで比較します）

## 概念解説
これまでの `Select` は「値 → 別の値」に変換していました。では、**「値 → Observable」に変換したらどうなるでしょうか。** たとえば「クリックされるたびに、1秒ごとに通知を流す Observable を作る」場合、結果は「Observable が流れてくる Observable」という入れ子の形になります。

```
クリック ──●─────────────●────────▶   （Observable<Unit>）
            │             │
            ▼             ▼
        タイマーA      タイマーB        （クリックごとに作られる Observable）
```

この入れ子のままでは、そのまま `Subscribe` できません。そこで使うのが **`Switch`** です。`Switch` は、入れ子の Observable を1本の流れに平らにしつつ、次のルールで動きます。

> **新しい Observable が流れてきたら、それまで購読していた Observable の購読をやめ、新しい方だけを購読する**

ラジオのチャンネルに例えると分かりやすいです。チューナーを別の局に合わせた瞬間、前の局の放送は聞こえなくなり、今合わせた局だけが聞こえます。`Switch` は「常に最後に選んだ局だけを聞く」オペレーターです。

もし `Switch` ではなく、すべてを同時に購読する仕組み（10-1 の `Merge` の考え方）を使うと、ボタンを押すたびにタイマーが**増えていき**、前のタイマーも動き続けます。「最新のものだけ有効にして、古いものは止めたい」ときに `Switch` を選びます。

タイムラインで比べると次のようになります（`●` はカウントされるタイミング、`▲` はボタンクリック）。

```
クリック    ▲        ▲
タイマーA   ─●──●──●─×          ← 2回目のクリックで破棄（×）
タイマーB            ─●──●──●──▶
Switch後    ─●──●──●──●──●──●──▶   （AとBのうち、最新の方だけが流れる）
```

## シーンの操作方法
1. Play してシーンを実行します。カウントは `0` です。
2. ボタンを1回クリックします。カウントが `0` にリセットされ、その後**1秒ごとに** `1, 2, 3, …` と増えていきます。
3. カウントが増えている途中で、もう一度ボタンをクリックしてみましょう。カウントが `0` に戻り、また 1 秒ごとに数え直しになります。
4. ボタンを連打してみましょう。**増えるペースは1秒に1ずつのまま**で、速くなったりしません（前のタイマーが破棄されているためです）。
5. Hierarchy の `Scripts` オブジェクトの `SampleOperator` コンポーネントにある `Interval` を変更すると、カウントの間隔を調整できます（デフォルトは1秒）。

## コード解説
```csharp
public class SampleOperator : MonoBehaviour, ITextBinder
{
    [SerializeField] private float _interval = 1;
    [SerializeField] private Button _button;

    private readonly ReactiveProperty<int> _count = new(0);
    private ReadOnlyReactiveProperty<string> _bindText;
    public ReadOnlyReactiveProperty<string> BindText => _bindText ??=
        _count.Select(x => x.ToString()).ToReadOnlyReactiveProperty().AddTo(this);

    public void Start()
    {
        _count.AddTo(this);

        _button.OnClickAsObservable()
            .Select(_ =>
            {
                _count.Value = 0; // カウントをリセット
                return Observable.Interval(TimeSpan.FromSeconds(_interval));
            })
            .Switch()
            .Subscribe(_ => _count.Value++)
            .AddTo(this);
    }
}
```
- `_button.OnClickAsObservable()`：ボタンのクリックを Observable として扱います（4-5 と同じ）。
- `.Select(_ => { ... return Observable.Interval(...); })`：クリックのたびに、`_interval` 秒ごとに通知を流す**新しい Observable** を作って返します。ここで流れる型は `Observable<Observable<Unit>>`（Observable を流す Observable）になります。`_count.Value = 0;` は、クリックの瞬間に表示を 0 に戻すための処理です。
- `.Switch()`：このレッスンの主役です。入れ子の Observable から、**最後に作られた Observable だけ**を購読し、以前の購読は自動で解除（Dispose）します。これにより、クリックのたびにタイマーが「作り直し」になります。
- `.Subscribe(_ => _count.Value++)`：`Switch` を通った後の通知が来るたびにカウントを増やします。`Switch` を通った後は普通の `Observable<Unit>` に戻っているので、これまでと同じ `Subscribe` が使えます。
- `BindText` は 3-2・4-3〜4-5 と同じキャッシュパターンです。`_count` の変化が `TextBinder` を通じて画面に表示されます。

## 実行結果の例
- Play 直後：表示は `0`
- クリック後 3 秒間放置：`0 → 1 → 2 → 3`（1 秒ごとに増える）
- 3 まで数えた時点でもう一度クリック：`0` に戻り、そこから `1 → 2 → …` と数え直す
- 連打した場合：最後のクリックから 1 秒ごとに `1, 2, 3, …`（複数のタイマーが同時に動くことはない）

## よくあるつまずきポイント
- **`Switch` は「入れ子の Observable」にしか使えません。** 普通の `Observable<Unit>` に対して `.Switch()` と書いてもコンパイルエラーになります。「`Select` で Observable を返す → `Switch`」という2つをセットで覚えましょう。
- 「`Switch` が古い購読を止めてくれている」ことを実感するには、`Switch` の代わりに「作ったものをすべて同時に購読する」仕組み（`SelectMany` など）に変えて試してみましょう。クリックするたびにタイマーが積み重なり、増えるペースがどんどん速くなります。
- `Select` の中でカウントのリセット（`_count.Value = 0`）をしているのは、クリックした瞬間に表示を戻したいためです。もしこれがないと、新しいタイマーの最初の通知（1 秒後）までは古い数字が表示されたままになります。
- `Switch` が止めるのは「購読」です。`Observable.Interval` のような**時間で動く Observable** の購読が解除されるので、タイマーが止まります。しかし、すでに実行が始まった `async` 処理そのものを止めてくれるわけではありません（それを扱うのが 4-7 の `SubscribeAwait` と `AwaitOperation`、および UniTask 単元の `CancellationToken` です）。
- 4-7 の `AwaitOperation.Switch` と名前が同じなのは偶然ではありません。どちらも「新しい処理が来たら古い処理をやめて、新しい方に切り替える」という考え方です。4-7 は「非同期処理」の切り替え、今回は「Observable の購読」の切り替えという違いがあります。

## 理解度チェック
1. `Switch` を使うと、新しい Observable が流れてきたとき、それまで購読していた Observable はどうなりますか。
2. もし `Switch` の代わりに「作ったすべてのタイマーを同時に購読する」仕組みにすると、ボタンを 3 回連打したときのカウントの増え方はどう変わるでしょうか。
3. 「検索ボックスに文字を入力するたびにサーバーへ検索リクエストを出し、最新の入力に対する結果だけを表示したい」という場合、`Switch` はなぜ向いているでしょうか。
