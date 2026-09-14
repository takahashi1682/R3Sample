# 4-4. TakeWhileによる継続条件

## この回で学ぶこと
- `TakeWhile` オペレーターで、「回数」ではなく「条件」でストリームの継続・終了を制御する方法
- `Take` と `TakeWhile` の使い分け
- 条件式の中で参照する変数のタイミングに注意する必要があること

## 前提知識
- 4-3 で学んだ `Take` と `OnCompleted` の関係
- `BindText` のキャッシュパターン（3-2 / 4-3 と同じ形）

## 概念解説
前回学んだ `Take` は「回数」で止めるオペレーターでしたが、`TakeWhile` は「条件」で止めるオペレーターです。ベルトコンベアで言えば、`Take` が「3個通したらシャッターを下ろす」のに対して、`TakeWhile` は「検品して基準を満たしている間だけ通し続け、基準を外れた瞬間にシャッターを下ろす」ゲートです。

```csharp
Observable.Interval(TimeSpan.FromSeconds(1))
    .TakeWhile(_ => _frameCount < 3) // 3回だけ発行する
    .Subscribe(
        _ => UpdateSubject.OnNext(++_frameCount),
        _ => Debug.Log("完了"))
    .AddTo(this);
```

このサンプルは `_frameCount < 3` という条件を使うことで、結果的に `Take(3)` と似た「3回で止まる」動作になっています。しかし仕組みはまったく違います。`TakeWhile` は Interval が値を流してくるたびに **その時点の条件を評価** し、条件が `False` になった時点でストリームを完了（`OnCompleted`）させます。

## シーンの操作方法
1. Play してシーンを実行します。
2. 画面のテキストが1秒ごとに `1` → `2` → `3` と更新されるのを確認します。
3. 3回表示された後、それ以上テキストが更新されないことを確認します。
4. Console を開き、4回目の Interval のタイミングで `完了` のログが1回出力されることを確認します（見た目の結果は 4-3 の `Take(3)` と同じになります）。

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
            .TakeWhile(_ => _frameCount < 3)
            .Subscribe(
                _ => UpdateSubject.OnNext(++_frameCount),
                _ => Debug.Log("完了"))
            .AddTo(this);
    }
}
```
- `TakeWhile(_ => _frameCount < 3)`：ここで参照している `_frameCount` は、`Subscribe` の中で `++_frameCount` として更新されている**同じフィールド**です。つまり `TakeWhile` の条件式は、直前の `Subscribe` によって書き換えられた値を見ながら、次にデータが流れてきたタイミングで再評価されています。この「条件式が参照している変数がどこで更新されているか」を追いかけることが、このサンプルを正しく理解する鍵になります。
- 具体的な流れ：
  1. 1回目の Interval → `TakeWhile` 評価時点で `_frameCount == 0` なので `True` → 通過 → `Subscribe` 内で `_frameCount` が `1` になる
  2. 2回目の Interval → 評価時点で `_frameCount == 1` → `True` → 通過 → `_frameCount` が `2` になる
  3. 3回目の Interval → 評価時点で `_frameCount == 2` → `True` → 通過 → `_frameCount` が `3` になる
  4. 4回目の Interval → 評価時点で `_frameCount == 3` → `3 < 3` は `False` → ここで `OnCompleted` が呼ばれてストリーム終了
- `BindText` のキャッシュパターンは 4-3 と全く同じ形です。何度出てきても仕組みは共通なので、迷ったら 3-2 の解説に立ち返りましょう。

## 実行結果の例
```
1秒後: テキストが "1" になる
2秒後: テキストが "2" になる
3秒後: テキストが "3" になる
4秒後: Console に "完了" が出力される（テキストはそれ以上変化しない）
```

## よくあるつまずきポイント
- シーンの解説文にある `OnCompleted` の綴りに注意してください（`OnComplete` ではありません）。
- `TakeWhile` の条件式がいつ・何回評価されるのかを誤解しやすいです。「値が流れてくるたびに、その都度、現在の条件を再評価している」だけであり、あらかじめ何回で止まるかを予約しているわけではありません。
- `Take(3)` と `TakeWhile(_ => _frameCount < 3)` は今回たまたま同じ結果になりますが、`TakeWhile` は回数ではなく任意の条件（例えば「HPが0以下になるまで」など）を扱える点で、`Take` より柔軟なオペレーターです。両者を「常に同じ」と覚えないようにしましょう。

## 理解度チェック
1. `TakeWhile` は「回数」と「条件」のどちらでストリームの終了を判断しますか。
2. このサンプルで `TakeWhile` の条件式に使われている `_frameCount` は、どこで・いつ更新されていますか。
3. `Take(3)` と `TakeWhile(_ => _frameCount < 3)` が今回同じ結果になる理由と、両者の本質的な違いを説明してください。
