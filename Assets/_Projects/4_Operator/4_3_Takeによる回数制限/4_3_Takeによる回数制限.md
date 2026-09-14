# 4-3. Takeによる回数制限

## この回で学ぶこと
- `Take` オペレーターで、次に流すデータの「回数」を制限する方法
- 回数制限に達すると `OnCompleted` が呼ばれてストリームが終了する、という仕組み
- 3-2 で学んだ「`??=` でキャッシュしてから `.AddTo(this)`」パターンの再確認

## 前提知識
- 4-2 で学んだ `Subject<T>` と `Select` の組み合わせ
- 3-2 で学んだ `ToReadOnlyReactiveProperty()` とキャッシュパターン
- `Subscribe` の第2引数（エラー時のコールバック）や `OnCompleted` の概念（Observable が持つ3種類の通知：`OnNext` / `OnErrorResume` / `OnCompleted`）

## 概念解説
`Take` は、ベルトコンベアで言えば「決まった個数を通したら自動でシャッターを下ろすゲート」です。指定した回数だけデータを次の工程に流したら、それ以降は一切データを流さず、ストリームそのものを完了させます。

```csharp
Observable.Interval(TimeSpan.FromSeconds(1))
    .Take(3) // 3回だけ発行する
    .Subscribe(_ => UpdateSubject.OnNext(++_frameCount),
        _ => Debug.Log("完了"))
    .AddTo(this);
```

`Observable.Interval` は本来無限に値を流し続けますが、`Take(3)` を挟むことで「最初の3回だけ」通過を許可します。3回目のデータが流れた後、`Take` は自動的にストリームを終了（`OnCompleted`）させます。`Subscribe` の第2引数はこの完了通知を受け取るコールバックで、ここでは `Debug.Log("完了")` が実行されます。

## シーンの操作方法
1. Play してシーンを実行します。
2. 画面のテキストが `1秒ごと` に `1` → `2` → `3` と3回だけ更新されるのを確認します。
3. 3回表示された後は、それ以上テキストが更新されないことを確認します。
4. Console を開き、4回目の Interval が発火するタイミング（3秒後）で `完了` というログが1回だけ出力されることを確認します。

## コード解説
```csharp
public class SampleOperator : MonoBehaviour, ITextBinder
{
    public readonly Subject<int> UpdateSubject = new();
    private int _frameCount;

    private ReadOnlyReactiveProperty<string> _bindText;
    public ReadOnlyReactiveProperty<string> BindText => _bindText ??=
        UpdateSubject.Select(x => x.ToString()).ToReadOnlyReactiveProperty().AddTo(this);

    private void Start()
    {
        UpdateSubject.AddTo(this);

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Take(3)
            .Subscribe(_ => UpdateSubject.OnNext(++_frameCount),
                _ => Debug.Log("完了"))
            .AddTo(this);
    }
}
```
- `BindText` は 3-2 で学んだキャッシュパターンそのものです。`UpdateSubject.Select(...).ToReadOnlyReactiveProperty()` は呼び出すたびに新しい購読を作ってしまうため、`_bindText ??= ...` で最初の1回だけ生成し、以後はキャッシュを再利用しています。ここでも `.AddTo(this)` はキャッシュを作る式の中に書かれていて、生成された購読を確実に破棄対象へ登録しています。
- `Take(3)` の後ろの `Subscribe` は、第1引数に「値が流れてきたときの処理」、第2引数に「完了（`OnCompleted`）したときの処理」を渡しています。この第2引数は「エラー時」ではなく「正常に完了したとき」に呼ばれることに注意してください（もし本当にエラー処理をしたい場合は3引数版の `Subscribe` を使います）。
- `UpdateSubject.AddTo(this)` について、コード中のコメントにもある通り「readonly な `Subject` は自動で `Dispose` されるものではないため、念のため登録している」という位置づけです。`Subject` はストリームの終了を制御する仕組みを持たないため、GameObject 破棄時に確実に閉じるための保険です。

## 実行結果の例
```
1秒後: テキストが "1" になる
2秒後: テキストが "2" になる
3秒後: テキストが "3" になる
4秒後: Console に "完了" が出力される（テキストはそれ以上変化しない）
```

## よくあるつまずきポイント
- シーンの解説文に登場する `OnCompleted` を、誤って `OnComplete`（末尾に `d` がない綴り）と覚えてしまう学生が多いので注意してください。R3 の正しい API 名は **`OnCompleted`** です。
- `Take(3)` が「3回目のデータも流したうえで、4回目からストップする」動作であることを、「3回目は流れない」と勘違いしないようにしましょう（1, 2, 3 の3個は流れます）。
- `Subscribe` の第2引数を「エラーハンドラ」だと思い込んでしまうケースがありますが、この2引数バージョンでは「完了時の処理」です。

## 理解度チェック
1. `Take(3)` を付けた `Observable.Interval` から、実際に流れてくる値は何個で、それぞれいくつですか。
2. `Take` によって回数制限に達したとき、どの通知（`OnNext` / `OnCompleted` のどちらか）が呼ばれますか。
3. `BindText` プロパティで `??=` を使っている理由を、3-2 の内容を踏まえて説明してください。
