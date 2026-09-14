# 1-1. UnityのイベントをObservableに変換する

## この回で学ぶこと
- R3が用意している「〇〇AsObservable」というメソッドを使うと、UnityのイベントをObservableに変換できることを知る
- `Subscribe`を使って、変換したObservableからイベントを受け取る書き方を身につける
- `AddTo`が「このオブジェクトが破棄されたら購読を自動的に解除する」という後片付けのための仕組みであることを理解する

## 前提知識
- 「1-0. ObservableとObserver」で学んだ、ObservableとObserverの役割
- Unityのイベント関数（`Update`、`OnEnable`、`OnTriggerEnter2D`など）がどんなタイミングで呼ばれるか
- `[SerializeField]`でInspectorから参照を渡す書き方

## 概念解説
前回学んだ「Observable」は抽象的な概念でしたが、R3ではUnityの`Update`や`OnTriggerEnter2D`といった**既存のイベント関数を、そのままObservableに変換するメソッド**が数多く用意されています。それが`UpdateAsObservable()`や`OnTriggerEnter2DAsObservable()`のような、末尾に`AsObservable`が付くメソッド群です。

これまでUnityでは、`Update`や`OnTriggerEnter2D`のようなイベント関数はMonoBehaviourに1つずつしか書けず、処理があちこちに散らばりがちでした。`AsObservable`を使うと、これらのイベントを「データの流れ（ストリーム）」として扱えるようになり、`Subscribe`で受け取った上で、後から学ぶ`Where`や`Select`といったオペレータを使って柔軟に加工できるようになります。

もう1つ重要なのが`AddTo`です。Observableを`Subscribe`すると、その購読（Observerの登録）はコードで明示的に解除しない限りずっと生き続けます。`AddTo(対象)`と書いておくと、「対象のGameObjectが破棄されたタイミングで、自動的に購読を解除する」という後片付けをR3が代わりにやってくれます。これを書き忘れると、シーンを移動してもオブジェクトが破棄されても購読が残り続け、メモリリークや意図しない処理の原因になります。

## シーンの操作方法
1. Playボタンを押してシーンを実行します。
2. Play開始直後、`Scripts`オブジェクトの`SampleObservable`が動き出し、`Square`（赤い四角）が`Triangle`（黒い三角）と最初から重なる位置にあるため、Consoleに`OnTriggerEnter2DAsObservable`のログが1回出力されます。
3. Game画面上で`Square`（赤い四角）にマウスカーソルを乗せてみましょう。`OnMouseEnterAsObservable`のログがConsoleに出力されます。
4. `OnMouseOverAsObservable`や`OnMouseDownAsObservable`など、コード上に`Subscribe(_ => { })`と書かれている（中身が空の）ものは、実際にはイベントが発生していてもConsoleには何も表示されません。Inspectorで`SampleObservable`スクリプトにブレークポイントを貼るか、コードを書き換えてログを追加すると、確かに呼ばれていることを確認できます。

## コード解説
```csharp
[SerializeField] private GameObject _target;

private void Awake()
{
    // 毎フレーム呼ばれる処理
    _target.UpdateAsObservable().Subscribe(_ => { }).AddTo(_target);
```
- `_target`はInspectorで`Square`オブジェクトが割り当てられています。このスクリプト自身がアタッチされている`Scripts`オブジェクトではなく、`_target`（＝`Square`）のイベントを観測している点に注意してください。
- `UpdateAsObservable()`は、`_target`の`Update`が呼ばれるたびに通知を流すObservableです。

```csharp
    _target.OnEnableAsObservable().Subscribe(_ => Debug.Log("OnEnableAsObservable")).AddTo(_target);
```
- `Subscribe`の引数には、通知が来るたびに実行したい処理をラムダ式で渡します。ここでは`Debug.Log`でConsoleに出力しているだけですが、実際のゲームではここに「体力バーを更新する」「爆発エフェクトを出す」といった処理を書きます。

```csharp
    _target.OnTriggerEnter2DAsObservable().Subscribe(_ => Debug.Log("OnTriggerEnter2DAsObservable")).AddTo(_target);
```
- 通常の`private void OnTriggerEnter2D(Collider2D other)`と同じタイミングで通知が飛んできます。第一引数（ここでは`_`で受け取って捨てています）から衝突相手の情報を取得することも可能です。

**`AddTo(_target)`について**：このスクリプトの他の多くのサンプルでは`AddTo(this)`（自分自身が破棄されたら解除）と書きますが、このスクリプトでは`AddTo(_target)`となっています。これは、「`_target`（Square）のイベントを観測しているのだから、`_target`が破棄されたタイミングで購読を解除するのが自然」という考え方によるものです。もし`_target`だけが破棄されて`this`（Scriptsオブジェクト）が生き残った場合、`AddTo(this)`のままだと、存在しない`_target`のイベントを購読し続けようとしてエラーの原因になり得ます。**「観測対象のライフサイクルに合わせて`AddTo`の引数を選ぶ」**という考え方を覚えておきましょう。

## 実行結果の例
```
OnTriggerEnter2DAsObservable
OnMouseEnterAsObservable   （Squareにマウスカーソルを乗せたとき）
```
`Update`や`LateUpdate`、`OnMouseOver`など中身が空のSubscribeについてはConsoleに何も表示されません。

## よくあるつまずきポイント
- `AddTo(this)`と書かずに`AddTo(_target)`と書いてある理由が分からず、「間違いでは？」と感じる学生が多いです。上記の「概念解説」「コード解説」で説明した通り、観測対象と購読の寿命を一致させるための意図的な書き方です。
- コード上の`Subscribe(_ => { })`のように中身が空のものは、動いていても目に見える変化がありません。「ログが出ない＝動いていない」と誤解しないよう注意しましょう。
- `AddTo`を書き忘れると購読が解除されないままになります。今回のサンプルではすべての`Subscribe`に`AddTo`が付いていることを確認し、なぜ必要なのかを説明できるようにしておきましょう。

## 理解度チェック
1. `UpdateAsObservable()`のように、末尾に`AsObservable`が付くメソッドは何をするためのものでしたか。
2. `AddTo`を書かないと、どのような問題が起こる可能性がありますか。
3. このレッスンのコードで、`AddTo(this)`ではなく`AddTo(_target)`と書かれているのはなぜでしたか。
