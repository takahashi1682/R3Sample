# 3-2. ToReadOnlyReactiveProperty

## この回で学ぶこと
- `ToReadOnlyReactiveProperty()` を使うと、任意の Observable の流れを「常に最新値を保持したプロパティ」に変換できることを理解する
- 複数の ReactiveProperty を `CombineLatest` で組み合わせて新しい値を作る方法
- 「一度だけ生成してキャッシュする」という `??=` を使ったパターンと、その必要性
- `AddTo(this)` をメソッドチェーンの末尾に付ける理由

## 前提知識
- 3-1 で学んだ `ReactiveProperty<T>` / `ReadOnlyReactiveProperty<T>` の基本
- Unit4 で学ぶ `Select` などのオペレーターの考え方（先取りで軽く触れます）
- C# の `??=`（null 合体代入演算子）の意味

## 概念解説
これまでの ReactiveProperty は「元から用意した値」を監視するものでした。しかし実際のゲームでは、「現在HP ÷ 最大HP ＝ HP比率」のように、**複数の値を組み合わせて作る新しい値**をリアルタイムに扱いたい場面がよく出てきます。

そこで使うのが `ToReadOnlyReactiveProperty()` です。これは「Observable の流れ」を「常に最新の値を持っているプロパティ」に変換してくれる機能です。例えるなら、株価や為替の**リアルタイムな値動きのグラフ**を、いつ見ても「今の値」がひと目でわかる**電光掲示板**に変換するようなイメージです。グラフ（Observable）は流れていくだけですが、電光掲示板（ReadOnlyReactiveProperty）は最後に流れてきた値を保持し続け、いつでも `.CurrentValue` で取り出せます。

このシーンでは、HP比率（`HealthRate`）や生死フラグ（`IsDead`）がこのパターンで作られています。

```csharp
private ReadOnlyReactiveProperty<float> _rate;
public ReadOnlyReactiveProperty<float> HealthRate => _rate ??=
    Observable.CombineLatest(_currentHeath, _maxHeath)
        .Select(x => x[0] / x[1]).ToReadOnlyReactiveProperty().AddTo(this);
```

`Observable.CombineLatest` は複数の Observable（ここでは現在HPと最大HP）の最新値をまとめて流す仕組みで、どちらか一方が変化するたびに `[現在HP, 最大HP]` という配列を流します。それを `Select` で「割り算した比率」に変換し、最後に `ToReadOnlyReactiveProperty()` で「常に最新の比率を保持するプロパティ」に変換しています。

## シーンの操作方法
1. Play してシーンを実行します。
2. 画面上部のスライダーをドラッグして、現在HPの値を変化させます。
3. `50/100` のように表示されている「現在HP／最大HP」のテキストが、スライダーに連動して変わることを確認します。
4. その右側にある「HP比率→」の値（0.00〜1.00）が、スライダーの動きに合わせて自動的に再計算されることを確認します。
5. スライダーを 0 まで下げると、「IsDead→」の表示が `False` から `True` に切り替わることを確認します。

## コード解説
### なぜ `??=` でキャッシュするのか
```csharp
private ReadOnlyReactiveProperty<float> _rate;
public ReadOnlyReactiveProperty<float> HealthRate => _rate ??=
    Observable.CombineLatest(_currentHeath, _maxHeath)
        .Select(x => x[0] / x[1]).ToReadOnlyReactiveProperty().AddTo(this);
```
もし `??=` を使わずに、プロパティにアクセスするたびに `ToReadOnlyReactiveProperty()` を呼び出すコードを書いてしまうと、**アクセスするたびに新しい購読（Subscribe）が作られてしまいます**。例えば `HealthRate` を3か所から参照すると、知らないうちに3つの CombineLatest 購読が生まれてしまい、無駄な計算とメモリリークの温床になります。

`_rate ??= ...` と書くことで、「`_rate` がまだ null のときだけ右側の式を実行して `_rate` に代入し、2回目以降のアクセスではキャッシュ済みの `_rate` をそのまま返す」という動作になります。こうすることで、`Observable.CombineLatest` によるストリームの生成は**最初の1回だけ**で済みます。

そして、キャッシュを作るその場で `.AddTo(this)` をチェーンの末尾に付けているのもポイントです。`ToReadOnlyReactiveProperty()` 自体が内部で購読を1つ持っているため、この購読をこの `Program` コンポーネントの寿命に紐付けて自動で片付けてもらう必要があります。`??=` の右辺は最初の1回しか実行されないので、`AddTo` の登録も1回だけで正しく機能します。

このパターン（`private フィールド` ＋ `??= ～.ToReadOnlyReactiveProperty().AddTo(this)`）は、この後の 4-3・4-4・4-5 の `BindText` プロパティでも同じ形で登場します。「プロパティを呼び出すたびに新しいストリームが生成されるのを防ぐための定番の書き方」として覚えておいてください。

### その他のポイント
```csharp
public ReadOnlyReactiveProperty<float> CurrentHealth => _currentHeath;
public ReadOnlyReactiveProperty<float> MaxHealth => _maxHeath;
```
こちらはキャッシュ不要の単純な公開で、`_currentHeath` 自体が既に1つの ReactiveProperty（＝インスタンスは常に同じ）だからです。キャッシュが必要になるのは「都度新しいストリームを作る処理（CombineLatest や Select など）を挟むとき」だけだと覚えておきましょう。

`SampleObserver.cs` では、これらを UI にバインドしています。
```csharp
_program1.HealthRate
    .Subscribe(x => _heathRateText.text = x.ToString("N2")).AddTo(this);
```
`HealthRate` を購読するだけで、HP比率が変わるたびにテキストが自動更新されます。UI 側は「今何が起きているか」を意識せず、届いた値をそのまま表示するだけで済むのが ReactiveProperty を使う大きなメリットです。

## 実行結果の例
- スライダーが 100/100 のとき：「HP比率→ 1.00」「IsDead→ False」
- スライダーを 25/100 まで下げたとき：「HP比率→ 0.25」「IsDead→ False」
- スライダーを 0/100 まで下げたとき：「HP比率→ 0.00」「IsDead→ True」

## よくあるつまずきポイント
- `??=` の意味がわからず「なぜこんな書き方をするのか」で立ち止まってしまう。まずは「`_rate` が空っぽのときだけ中身を作る」という意味だと理解しましょう。
- `.AddTo(this)` を `??=` の右辺の中（式の途中）に書く理由がわからず、Subscribe の後ろに付けるものと勘違いする。ここでは `ToReadOnlyReactiveProperty()` 自体が持つ購読を破棄対象にしています。
- `CombineLatest` は「両方のReactivePropertyの初期値が確定してから」動き出す点を忘れ、値が来ないと勘違いしてしまう（実際には ReactiveProperty は生成時に初期値を持っているため、初回から正しく動作します）。

## 理解度チェック
1. `ToReadOnlyReactiveProperty()` はどのような変換を行うものか、電光掲示板の例えを使わずに自分の言葉で説明してください。
2. `HealthRate` プロパティで `??=` を使わずに、毎回 `Observable.CombineLatest(...).ToReadOnlyReactiveProperty()` を呼び出してしまうと、どのような問題が起きますか。
3. `CurrentHealth` プロパティにはキャッシュ（`??=`）が使われていないのはなぜですか。
