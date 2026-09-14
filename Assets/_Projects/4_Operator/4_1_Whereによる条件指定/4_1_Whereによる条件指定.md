# 4-1. Whereによる条件指定

## この回で学ぶこと
- `Where` オペレーターを使って、条件を満たしたデータだけを後続に流す方法
- `enum`（列挙型）で状態を管理し、ReactiveProperty と組み合わせて使う方法
- 同じストリーム（`FixedUpdateAsObservable`）から複数の `Where` で条件分岐する書き方

## 前提知識
- 4-0 で学んだ「オペレーター＝ベルトコンベアの工程」というイメージ
- 3-1 で学んだ ReactiveProperty / ReadOnlyReactiveProperty の基本
- `R3.Triggers` 名前空間の `FixedUpdateAsObservable()`（`FixedUpdate` を Observable 化するもの）

## 概念解説
`Where` は、LINQ の配列操作でおなじみの「条件に合うものだけを残す」処理を、Observable の流れに対して行うオペレーターです。前回のベルトコンベアの例で言えば、「検品ゲート」に相当します。ゲートを通る材料（データ）のうち、条件（`bool` を返す式）が `True` のものだけが次の工程に進み、`False` のものはその場で弾かれて捨てられます。

このサンプルでは、`EMoveMode` という enum（`None` / `Pingpong` / `Rotate`）を ReactiveProperty で管理し、「今の状態が特定のモードのときだけ、対応する処理を実行する」という使い方をしています。

```csharp
this.FixedUpdateAsObservable()
    .Where(_ => _target.MoveMode.CurrentValue == EMoveMode.Pingpong)
    .Subscribe(_ => Move())
    .AddTo(this);
```

`FixedUpdateAsObservable()` は「毎 `FixedUpdate` ごとに値を流す Observable」です。これだけだと単なる `FixedUpdate` の呼び出しと変わりませんが、`Where` を挟むことで「`MoveMode` が `Pingpong` のときだけ `Move()` を呼ぶ」という条件付きの処理に変わります。同じ `FixedUpdateAsObservable()` に対して、別の条件の `Where` をもう一つ用意すれば、`if / else if` を書かなくても、それぞれ独立したストリームとして条件分岐を表現できます。

## シーンの操作方法
1. Play してシーンを実行します。
2. Hierarchy から `Scripts` オブジェクトを選択し、Inspector にある **`SampleOperator` コンポーネント**の `Move Mode` の値を、Play 中に `None` → `Pingpong` → `Rotate` と切り替えてみましょう。
3. `Pingpong` にすると、シーン内のオブジェクトが左右に往復移動します。
4. `Rotate` にすると、円を描くように移動します。
5. `None` に戻すと、どちらの `Where` の条件も満たさなくなるため、移動処理が止まります。

## コード解説
`SampleOperator.cs`
```csharp
public enum EMoveMode
{
    None = 0,
    Pingpong = 1,
    Rotate = 2
}

[SerializeField] private SerializableReactiveProperty<EMoveMode> _moveMode = new(EMoveMode.None);
public ReadOnlyReactiveProperty<EMoveMode> MoveMode => _moveMode;
```
- `EMoveMode` は状態を表す enum です。Inspector からドロップダウンで選べるように `SerializableReactiveProperty<EMoveMode>` として宣言されています。
- 外部に公開する `MoveMode` は 3-1 で学んだ通り `ReadOnlyReactiveProperty` にして、他クラスから値を書き換えられないようにしています。

`SampleObserver.cs`
```csharp
this.FixedUpdateAsObservable()
    .Where(_ => _target.MoveMode.CurrentValue == EMoveMode.Pingpong)
    .Subscribe(_ => Move()).AddTo(this);

this.FixedUpdateAsObservable()
    .Where(_ => _target.MoveMode.CurrentValue == EMoveMode.Rotate)
    .Subscribe(_ => Rotate()).AddTo(this);
```
- `_target.MoveMode.CurrentValue` で「今この瞬間の値」を取得しています。`Where` の条件式はストリームが流れてくるたびに毎回評価されるため、Inspector で値を変えれば次の `FixedUpdate` からすぐに条件の結果が変わります。
- 見落としがちなポイントとして、この `Where` は `MoveMode` の**変化そのもの**を監視しているのではなく、`FixedUpdateAsObservable()` が流れてくるたびに `MoveMode.CurrentValue` を**都度チェック**している、という点があります。「値が変わった瞬間に発火する」のではなく「毎フレーム条件を再評価している」という違いを意識してください。

## 実行結果の例
- `Move Mode = Pingpong` のとき：オブジェクトが左右に往復移動する
- `Move Mode = Rotate` のとき：オブジェクトが円運動する
- `Move Mode = None` のとき：オブジェクトは静止したままになる

## よくあるつまずきポイント
- 存在しない「Operator1」のような名前を探してしまうミス（実際のコンポーネント名は **`SampleOperator`** です）。Hierarchy の `Scripts` オブジェクトについているコンポーネント名を必ず確認しましょう。
- `Where` を「値が変わった瞬間だけ発火するもの」と誤解してしまう。実際には毎フレーム（この場合は毎 `FixedUpdate`）条件を再チェックしているだけです。
- 2つの `Where` を「if / else if」のように排他的だと思い込むが、実際には両方とも独立したストリームなので、条件次第では両方 True になり得る書き方も可能です（このサンプルでは enum が同時に2つの値を取れないため排他的に見えています）。

## 理解度チェック
1. `Where` オペレーターの役割を、検品ゲートの例を使わずに説明してください。
2. このサンプルで `Move Mode` を `Rotate` に変えると、なぜ `Move()` ではなく `Rotate()` が呼ばれるようになるのか、`Where` の条件式に触れながら説明してください。
3. `Where` は「値が変化した瞬間」に発火するものですか、それとも「ストリームが流れてくるたびに毎回条件を評価する」ものですか。
