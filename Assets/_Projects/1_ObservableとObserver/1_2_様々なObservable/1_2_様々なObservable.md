# 1-2. 様々なObservable

## この回で学ぶこと
- UnityのイベントをAsObservable化したもの以外にも、R3自身が用意しているObservableの生成メソッドがあることを知る
- `Observable.Range`、`Observable.Repeat`、`Observable.Timer`、`Observable.Interval`の違いを理解する
- `Observable.Create`を使うと、非同期処理を含む独自のObservableを作れることを知る

## 前提知識
- 「1-1. UnityのイベントをObservableに変換する」で学んだ、`Subscribe`と`AddTo`の基本的な使い方
- `TimeSpan`構造体（`TimeSpan.FromSeconds(3)`のように、時間の長さを表す型）
- （余裕があれば）`async`/`await`の基本的な意味

## 概念解説
前回はUnityの既存イベント（`Update`など）をObservableに変換する方法を学びましたが、Observableには**Unityのイベントに由来しない、R3自身が用意している「独自のストリーム」**も数多く用意されています。今回はその代表的なものを扱います。

- `Observable.Range(開始値, 個数)`：指定した範囲の連続した数値を、順番に通知します。`for`ループで数値を1つずつ流していくイメージです。
- `Observable.Repeat(値, 回数)`：指定した値を、指定した回数だけ繰り返し通知します。
- `Observable.Timer(時間)`：指定した時間が経過した後に、1回だけ通知します。
- `Observable.Interval(時間)`：指定した時間の間隔で、繰り返し通知し続けます。目覚まし時計が一定間隔で鳴り続けるイメージです。
- `Observable.Create`：自分で好きなタイミングで通知を発行する、オリジナルのストリームを作るためのメソッドです。

これらはいずれも「時間の経過に応じてデータやタイミングを流す」という点で共通しており、`Update`の中で`Time.time`や`if`文を使って自前でタイマーを実装するよりも、はるかに簡潔に書くことができます。

## シーンの操作方法
1. Playボタンを押してシーンを実行します。
2. Playした瞬間から、Consoleに`Range`と`Repeat`のログが一気に出力されます（この2つは時間待ちがないため、ほぼ即座に完了します）。
3. 3秒待つと`"3秒経過しました"`のログが1回だけ出ます（`Observable.Timer(TimeSpan.FromSeconds(3))`）。
4. Play開始と同時に`Timer: (時間)`のログが1秒おきに出続けます（`Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))`）。
5. Play開始から1秒後を起点に、`Interval: (時間)`のログが1秒おきに出続けます。TimerとIntervalのログを見比べて、開始タイミングの違い（0秒後に始まるか、1秒後に始まるか）を確認してください。
6. `Observable.Create`によるログ（`Create: ゲーム開始`→`Create: 敵出現`→`Create: タイムオーバー`）が1秒間隔で順番に出力されるのを確認しましょう。

## コード解説
```csharp
Observable.Range(5, 3)
    .Subscribe(x => Debug.Log($"Range: {x}"))
    .AddTo(this);
```
- `5`から始まる`3`個の連続した整数（5, 6, 7）を、間を空けずに一気に通知します。

```csharp
Observable
    .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
    .Subscribe(_ => Debug.Log("Timer: " + Time.time))
    .AddTo(this);

Observable
    .Interval(TimeSpan.FromSeconds(1))
    .Subscribe(_ => Debug.Log("Interval: " + Time.time))
    .AddTo(this);
```
- `Timer`に2つの引数を渡すと、「最初の待ち時間」と「以降の繰り返し間隔」を別々に指定できます。ここでは`TimeSpan.Zero`（待ち時間なし）としているので、Play直後から1秒おきに通知されます。
- 一方`Interval`は「一定間隔で繰り返す」専用のメソッドで、最初の通知も指定した間隔（1秒）待ってから来ます。この違いに注目しましょう。

```csharp
Observable.Create<string>(async (observer, ct) =>
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct); // 1秒待機
        observer.OnNext("Create: ゲーム開始"); // 通知

        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct); // 1秒待機
        observer.OnNext("Create: 敵出現"); // 通知

        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct); // 1秒待機
        observer.OnNext("Create: タイムオーバー"); // 通知

        observer.OnCompleted(); // 完了
    }).Subscribe(Debug.Log)
    .AddTo(this);
```
- `Observable.Create`には、`observer`（通知を送る相手）と`ct`（キャンセルを伝えるための`CancellationToken`）を受け取る`async`な処理を渡します。
- `observer.OnNext(値)`を呼ぶたびに、`Subscribe`側にその値が届きます。ここでは1秒待つごとに`OnNext`を1回呼んでいるので、1秒間隔でメッセージが流れてきます。
- 最後に`observer.OnCompleted()`を呼ぶことで、「このストリームはもう終わりです」という完了通知を送っています（`OnCompleted`については次回のレッスンで詳しく扱います）。
- UniTaskの`await`を使った非同期処理を、そのままObservableとして扱えるのが`Observable.Create`の強力な点です。

## 実行結果の例
```
Range: 5
Range: 6
Range: 7
Repeat: Hello   （10回繰り返し）
Timer: 0.0123...
（1秒後）Timer: 1.0256...
（1秒後）Interval: 1.0301...
（1秒後）Timer: 2.0311... / Create: ゲーム開始
（3秒後）3秒経過しました
（1秒後）Create: 敵出現
（1秒後）Create: タイムオーバー
```
※実際のタイミングはフレームレートにより多少前後します。

## よくあるつまずきポイント
- `Timer`と`Interval`を混同しがちです。`Timer(開始待ち, 間隔)`は「開始待ち時間」を自由に指定できるのに対し、`Interval(間隔)`は最初から「間隔」だけしか指定できず、最初の通知も間隔分待ってから来ます。
- `Observable.Create`の中でネストされた`async`ラムダに慣れておらず、「`await`の後に`OnNext`を書く」という流れを難しく感じることがあります。まずは「1秒待つ→通知する」を1セットとして、それが3回繰り返されているだけ、と捉えると理解しやすいです。
- `Observable.Range`や`Observable.Repeat`は待ち時間がないため、実行結果が一瞬で流れてしまい見落としがちです。Console内の表示回数（Repeatなら10回）を実際に数えて確認してみましょう。

## 理解度チェック
1. `Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))`と`Observable.Interval(TimeSpan.FromSeconds(1))`の、最初の通知タイミングの違いを説明してください。
2. `Observable.Range(5, 3)`を`Subscribe`すると、どのような値がどんな順番で通知されますか。
3. `Observable.Create`の中で、ストリームの終わりを伝えるために呼んでいるメソッドは何でしたか。
