# 30-4. cancellationToken

## この回で学ぶこと
- `CancellationToken`とは何かをイメージできるようになる
- `destroyCancellationToken`を使って、オブジェクトの破棄と非同期処理を連動させる方法
- `OperationCanceledException`が発生する仕組みと、それが「エラーではない」ということ

## 前提知識
- 30-3で体験した「オブジェクトを破棄しても非同期処理は止まらない」という問題
- `async`/`await`の基本文法

## 概念解説
前回、非同期処理はオブジェクトの破棄と自動的には連動しないことを確認しました。今回はこの問題を解決する`CancellationToken`という仕組みを学びます。

`CancellationToken`は、「そろそろやめてください」という合図を伝えるための紙切れのようなものだとイメージしてください。非同期処理を実行している側（`UniTask.Delay`など）は、この紙切れを定期的にチェックしていて、「キャンセルする」という合図が来たら、そこで処理を打ち切ります。

MonoBehaviourには`destroyCancellationToken`という便利なプロパティが標準で用意されています。これは「そのGameObject専用の紙切れ」で、GameObjectが破棄されるタイミングで自動的に「キャンセルしてください」という合図が発行される仕組みになっています。つまり、非同期処理の`cancellationToken`引数にこの`destroyCancellationToken`を渡しておくだけで、「このオブジェクトが壊れたら、この処理もそこで止める」という連動を実現できます。

オブジェクトが破棄されると、渡したトークンでキャンセルが発行され、`OperationCanceledException: The operation was canceled.`というエラーがConsoleに表示されます。これはあくまで「予定通りキャンセルされました」という報告であり、ゲームが強制終了するようなものではありません。安心してください。

## シーンの操作方法
1. Playボタンを押します。
2. 何も操作せずに3秒待ち、Consoleに「3秒経過しました」というログが出ることを確認します。
3. もう一度Playし直し、3秒が経過する前にHierarchy上のオブジェクトを削除します。
4. 前回（30-3）とは違い、今回はオブジェクトを削除した直後にConsoleへ赤字で`OperationCanceledException`が表示されることを確認してください。これが「非同期処理が正しくキャンセルされた」証拠です。

## コード解説
```csharp
private async void Start()
{
    // このオブジェクトが破棄されたらキャンセルを発行するトークン
    var token = destroyCancellationToken;

    // cancellationToken: token とすることで、token がキャンセルされたら処理を中断する
    await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: token); // 3秒待機する

    Debug.Log("3秒経過しました");
}
```
- `var token = destroyCancellationToken;`: MonoBehaviourが自動で持っている「このオブジェクト専用のキャンセル用トークン」を取得しています。
- `cancellationToken: token`: `UniTask.Delay`に対して「このトークンがキャンセルされたら、この待機処理も中断してください」と伝えています。この引数を渡し忘れると、30-3のように何も連動しないままになってしまうので注意してください。
- オブジェクトが破棄されると`token`が自動的にキャンセル状態になり、`await`していた行で`OperationCanceledException`が投げられ、その先の`Debug.Log`は実行されません。

## 実行結果の例
- 通常時: 3秒後にConsoleに「3秒経過しました」と表示されます。
- オブジェクトを3秒以内に削除した場合: 削除した瞬間、Consoleに赤字で`OperationCanceledException: The operation was canceled.`と表示されます。「3秒経過しました」は表示されません。ゲーム自体は問題なく動き続けます。

## よくあるつまずきポイント
- Consoleに赤いエラーが出ると「バグを埋め込んでしまった」と焦ってしまう学生が多いですが、`OperationCanceledException`は意図した通りの正常な挙動です。慌てず、なぜこの例外が発生したのかを説明できるようにしましょう。
- `cancellationToken:`引数を渡し忘れると、`destroyCancellationToken`を取得しただけでは何の効果もありません。「トークンを作る（取得する）こと」と「そのトークンを非同期処理に渡すこと」は別の作業だと意識してください。
- `destroyCancellationToken`はあくまで「そのGameObjectが破棄されたとき」にしか反応しません。任意のタイミングでキャンセルしたい場合は次回の30-5で学ぶ`CancellationTokenSource`が必要になります。

## 理解度チェック
1. `destroyCancellationToken`はどのようなタイミングでキャンセル状態になりますか。
2. `cancellationToken:`引数を渡さなかった場合、30-3と30-4のコードはどちらも同じ挙動になりますか。理由も含めて答えてください。
3. `OperationCanceledException`が表示されたとき、なぜ慌てる必要がないのか説明してください。
