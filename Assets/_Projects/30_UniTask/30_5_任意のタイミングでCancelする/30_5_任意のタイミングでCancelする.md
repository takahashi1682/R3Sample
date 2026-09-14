# 30-5. 任意のタイミングでCancelする

## この回で学ぶこと
- `CancellationTokenSource`を使って、自分の好きなタイミングで非同期処理をキャンセルする方法
- `Cancel()`と`Dispose()`の役割の違い
- 使い終わった`CancellationTokenSource`をきちんと破棄しておく理由

## 前提知識
- 30-4で学んだ`destroyCancellationToken`（オブジェクトが破棄された時に自動でキャンセルされる仕組み）
- `cancellationToken:`引数に紙切れ（トークン）を渡すことで非同期処理と連動させる、という考え方

## 概念解説
`destroyCancellationToken`は「オブジェクトが破棄された時」にしか反応しません。しかし実際のゲーム制作では、「ボタンを押した時」「特定の条件を満たした時」など、破棄以外の任意のタイミングでキャンセルしたい場面がよくあります。そこで使うのが`CancellationTokenSource`です。

`CancellationTokenSource`は「キャンセルの発信機」だとイメージしてください。発信機自体（`CancellationTokenSource`）と、それが発行する紙切れ（`.Token`）は別物です。非同期処理には`.Token`（紙切れ）の方を渡しておき、発信機側で`.Cancel()`を呼ぶと、渡しておいた紙切れが一斉に「キャンセルされた」状態に変わります。

そしてもう1つ大事なのが`.Dispose()`です。`CancellationTokenSource`は内部でOS側のリソース（待機用のハンドルなど）を保持しており、使い終わったら明示的に解放してあげるのがお作法です。今回のサンプルでは、このオブジェクトが破棄される`OnDestroy`のタイミングで`_token.Dispose()`を呼び出しています。これは「発信機を最後まで使い終わったら、ちゃんと片付ける」という後始末の処理です。片付けを忘れても致命的な問題にすぐつながるわけではありませんが、長時間動くアプリや`CancellationTokenSource`を何度も生成するようなコードでは、リソースの解放漏れ（リーク）の原因になり得るため、破棄されるタイミングで`Dispose()`するのは良い習慣です。

## シーンの操作方法
1. Playボタンを押します。
2. 何も操作しなければ、3秒後にConsoleへ「3秒経過しました」というログが表示されます。
3. もう一度Playし直し、今度は3秒が経過する前に画面上の「停止」ボタンを押します。
4. ボタンを押すと、非同期処理がキャンセルされ、Consoleに`OperationCanceledException`が表示され、「3秒経過しました」は表示されないことを確認してください。

## コード解説
```csharp
// キャンセルトークン
private readonly CancellationTokenSource _token = new();

private async void Start()
{
    await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: _token.Token); // 3秒待機する

    Debug.Log("3秒経過しました");
}

public void OnCancel()
{
    // トークンをキャンセルする
    _token.Cancel();
}

private void OnDestroy()
{
    // オブジェクト破棄時にトークンを破棄する
    _token.Dispose();
}
```
- `private readonly CancellationTokenSource _token = new();`: 「発信機」本体をフィールドとして持っています。
- `cancellationToken: _token.Token`: 発信機から発行される紙切れ（`.Token`）を、`UniTask.Delay`に渡しています。
- `public void OnCancel()`: 画面上の「停止」ボタンに紐付けられているメソッドです。`_token.Cancel()`を呼ぶことで、渡しておいた紙切れがキャンセル状態になり、待機中の`UniTask.Delay`が中断されます。
- `private void OnDestroy()`: このオブジェクトが破棄されるタイミングで、`_token.Dispose()`を呼んで発信機自体の後始末をしています。`destroyCancellationToken`のような「破棄と連動してキャンセルする」仕組みとは別に、自分で作った`CancellationTokenSource`は自分でDisposeまで面倒を見る必要がある点に注意してください。

## 実行結果の例
- 何もしない場合: 3秒後にConsoleに「3秒経過しました」と表示されます。
- 「停止」ボタンを3秒以内に押した場合: ボタンを押した瞬間にConsoleへ`OperationCanceledException`が表示され、「3秒経過しました」は表示されません。

## よくあるつまずきポイント
- `_token.Cancel()`を呼べば自動的に全部うまくいくと思いがちですが、そもそも`cancellationToken:`として`_token.Token`を渡していなければ、`Cancel()`を呼んでも待機中の処理には何も伝わりません。「発信機を作ること」「紙切れを渡すこと」「発信すること」の3つがそろって初めて機能します。
- `Dispose()`と`Cancel()`の役割を混同してしまう。`Cancel()`は「今すぐキャンセルの合図を出す」、`Dispose()`は「発信機自体の後片付けをする」という別の役割です。
- 一度`Dispose()`した`CancellationTokenSource`を再利用しようとしてエラーになるケースがあります。基本的に使い捨てのつもりで扱いましょう。

## 理解度チェック
1. `CancellationTokenSource`と`CancellationToken`（`.Token`）の関係を説明してください。
2. `OnDestroy`で`_token.Dispose()`を呼んでいるのはなぜですか。
3. 「停止」ボタンを押すタイミングを3秒後より後にした場合、Consoleの表示はどうなると思いますか。理由も含めて答えてください。
