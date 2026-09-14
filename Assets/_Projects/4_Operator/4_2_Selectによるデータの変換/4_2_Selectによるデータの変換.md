# 4-2. Selectによるデータの変換

## この回で学ぶこと
- `Select` オペレーターを使って、受け取ったデータを別の値・別の型に変換する方法
- `Subject<T>` から流れてきた値を加工して `ReactiveProperty` に詰め直す流れ
- `ITextBinder` インターフェースを使った「値を UI テキストにバインドする」共通の仕組み

## 前提知識
- 4-0・4-1 で学んだオペレーター（ベルトコンベアの工程）の考え方
- `Subject<T>` の基本的な使い方（`OnNext` で値を流す）
- 3-2 で学んだ「`??=` でキャッシュしてから `.AddTo(this)` する」パターン

## 概念解説
`Select` は、ベルトコンベアで言えば「材料の形を加工する工程」です。`Where` が「通す／通さない」を判断するだけなのに対して、`Select` は流れてきたデータを**別の値に変換して**次の工程に渡します。変換前と変換後で型が変わってもかまいません（今回は `int` → `string` に変換しています）。

このサンプルでは、`SampleOperator` がフレーム数（`int`）を毎フレーム流し続け、`SampleObserver` がその値を `Select` でカンマ区切りの文字列（`string`）に変換して画面に表示しています。

```csharp
_target.UpdateSubject
    .Select(x => x.ToString("N0")) // 受け取った値をカンマ区切りの文字列に変換
    .Subscribe(x => _value.Value = x)
    .AddTo(this);
```

`ToString("N0")` は数値を桁区切り（例：`1,234`）の文字列に変換する C# 標準の書式指定です。`Select` の中でこの変換を行い、変換した結果を `Subscribe` で受け取って `_value.Value` に代入しています。

また、今回から `ITextBinder` というインターフェースが登場します。
```csharp
public interface ITextBinder
{
    ReadOnlyReactiveProperty<string> BindText { get; }
}
```
これは「文字列を公開できるクラス」の共通の窓口です。`SampleObserver` がこのインターフェースを実装しておくことで、シーン上の `TextBinder` コンポーネント（`Text (TMP)` オブジェクトに付いている）が `BindText` を自動的に購読し、テキストに反映してくれます。つまり、**「値をどう作るか」（SampleObserver の役割）と「値をどう表示するか」（TextBinder の役割）を分離**できる設計になっています。

## シーンの操作方法
1. Play してシーンを実行します。
2. 画面のテキストが `1`, `2`, `3`... と毎フレーム増えていき、桁が大きくなると `1,234` のようにカンマ区切りで表示されることを確認します（数千フレーム待つか、フレームレートが高い環境ではすぐに確認できます）。
3. Hierarchy の `Scripts` オブジェクトの `SampleOperator` コンポーネントを見て、`UpdateSubject` が毎フレーム値を流していることをイメージしながら、`Text (TMP)` オブジェクトの `TextBinder` コンポーネントの `Target` に `SampleObserver` が設定されていることを確認しましょう。

## コード解説
`SampleOperator.cs`
```csharp
public readonly Subject<int> UpdateSubject = new();
private int _frameCount;

private void Update()
{
    UpdateSubject.OnNext(++_frameCount);
}
```
- `Subject<int>` は ReactiveProperty と違い、値を保持しません。`Update()` のたびに `OnNext` で「フレーム数が増えました」というイベントを流すだけの、いわば放送専用のマイクです。

`SampleObserver.cs`
```csharp
public class SampleObserver : MonoBehaviour, ITextBinder
{
    private readonly ReactiveProperty<string> _value = new();
    public ReadOnlyReactiveProperty<string> BindText => _value;

    private void Start()
    {
        _value.AddTo(this); // このコンポーネントが破棄されたら_valueも破棄する（メモリーリーク防止）

        _target.UpdateSubject
            .Select(x => x.ToString("N0"))
            .Subscribe(x => _value.Value = x)
            .AddTo(this);
    }
}
```
- `_value.AddTo(this)`：`_value` は `Subject` ではなく `ReactiveProperty` なので内部に購読を持ちません。それでも `AddTo` を呼んでいるのは、`ReactiveProperty` 自身も `IDisposable` であり、GameObject の破棄と同時に確実に片付けるためです。コメントにもある通り「メモリーリーク防止」が目的です。
- `Select(x => x.ToString("N0"))`：ここが今回の主役です。`int` の値を受け取り、書式付きの `string` に変換しています。
- `Subscribe(x => _value.Value = x)`：変換後の文字列を `_value`（ReactiveProperty）に代入しています。この代入によって `BindText` を購読している `TextBinder` に通知が飛び、画面のテキストが更新されます。

## 実行結果の例
画面のテキストが以下のように毎フレーム更新されていきます。
```
1 → 2 → 3 → ... → 999 → 1,000 → 1,001 → ...
```

## よくあるつまずきポイント
- `Select` を `Where` と混同し、「条件に合わないものを弾く」機能だと勘違いしてしまう。`Select` は**必ず1つの値を1つの値に変換して流す**オペレーターであり、値を弾くことはありません。
- `Subject<int>` と `ReactiveProperty<string>` の役割の違いが曖昧になりがちです。`UpdateSubject` は「フレームごとのイベント」を流すだけで現在値を保持しませんが、`_value` は「今表示すべき文字列」を保持し続けるという違いがあります。
- `ITextBinder` を実装しているのに `BindText` を正しく `AddTo` していないと、UI に反映されない、あるいは意図しないタイミングで購読が切れるといった不具合につながります。

## 理解度チェック
1. `Select` と `Where` の役割の違いを説明してください。
2. `_target.UpdateSubject.Select(x => x.ToString("N0"))` で、`Select` に渡されている `x` はどんな型で、変換後は何の型になっていますか。
3. `ITextBinder` インターフェースがあることで、`SampleObserver` と `TextBinder` の役割はそれぞれどう分担されていますか。
