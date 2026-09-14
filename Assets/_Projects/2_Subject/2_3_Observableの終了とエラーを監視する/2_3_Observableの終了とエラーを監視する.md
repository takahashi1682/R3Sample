# 2-3. Observableの終了とエラーを監視する

## この回で学ぶこと
- `OnNext`以外にObservableが送信できる2種類の通知、`OnErrorResume`（エラー）と`OnCompleted`（完了）を知る
- `Subscribe`に複数のコールバック（`onNext`・`onErrorResume`・`onCompleted`）をまとめて渡す書き方を身につける
- Observableには「データを送り続ける」だけでなく「終わる」という状態があることを理解する

## 前提知識
- 「2-1. データを発行する」で学んだ、`OnNext`によるデータ発行
- 「2-2. データを監視する」で学んだ、`Subscribe`でObserverを登録する書き方
- 例外（`Exception`）の基本的な扱い（`throw`ではなく、ここでは既存の例外オブジェクトを渡す使い方をします）

## 概念解説
これまでのレッスンでは、Subjectが`OnNext`で通知するデータをひたすら受け取り続けてきました。しかし実際のストリームには「エラーが起きた」「もう二度とデータは流れてこない」といった、**通常のデータとは違う特別な出来事**を伝えたい場面があります。

Observableには、データを送信する`OnNext`以外にも、次の2種類の通知が用意されています。

- **`OnErrorResume`**：エラーの発生を送信します。「問題は起きたけれど、このストリーム自体は継続する」というニュアンスの通知です。
- **`OnCompleted`**：データの送信がすべて完了したことを伝えます。「もうこのストリームからは何も流れてきません」という、ストリームの終わりを示す通知です。

この2つは、`OnNext`と同じように**Observer側で監視する**ことができます。つまり、Observer（購読側）は「データが来たとき」「エラーが起きたとき」「ストリームが完了したとき」の3種類の出来事それぞれに対して、別々の処理を書き分けられるということです。

たとえるなら、`OnNext`は「通常の配達」、`OnErrorResume`は「配達中に一部の荷物が壊れていたという連絡」、`OnCompleted`は「もう配達する荷物はありません、という最終連絡」のようなものです。壊れた荷物の連絡（`OnErrorResume`）が来ても、配達自体（ストリーム）は続けられる、という点がポイントです。

## シーンの操作方法
1. Playボタンを押してシーンを実行します。
2. Consoleに、`0`→`1`（通常のデータ）→エラーログ（赤字の`エラー発生`）→`OnCompleted`という順番でログが出力されるのを確認しましょう。
3. Consoleのログの色（通常の黒字ログと、`Debug.LogError`による赤字のエラーログ）の違いにも注目してください。

## コード解説
発行する側（`SampleSubject`）：
```csharp
public Subject<int> TestSubject { get; } = new();

private void Start()
{
    TestSubject.AddTo(this);

    TestSubject.OnNext(0);
    TestSubject.OnNext(1);

    // エラーを発行
    TestSubject.OnErrorResume(new Exception("エラー発生"));

    // イベントの発行を終了(破棄)
    TestSubject.OnCompleted();
}
```
- `TestSubject.OnNext(0);` `TestSubject.OnNext(1);`：これまで通り、通常のデータを2件発行しています。
- `TestSubject.OnErrorResume(new Exception("エラー発生"));`：`new Exception("エラー発生")`で例外オブジェクトを作り、それを`OnErrorResume`で通知しています。ここで注意したいのは、`throw`していない（＝C#レベルの例外としては投げていない）ことです。あくまでRxの仕組みの中で「エラー情報」としてストリームに乗せて流しているだけなので、この行が実行されてもプログラムが停止したりはしません。
- `TestSubject.OnCompleted();`：これ以上データを発行しない、という完了通知です。呼んだ後にもし`OnNext`を呼んでも、購読しているObserverには届かなくなります。

監視する側（`SampleObserver`）：
```csharp
_target.TestSubject
    .Subscribe(x =>
    {
        // OnNext
        Debug.Log(x);
    }, onErrorResume: error =>
    {
        // OnErrorResume
        Debug.LogError(error);
    }, onCompleted: _ =>
    {
        // OnCompleted
        Debug.Log("OnCompleted");
    })
    .AddTo(this);
```
- `Subscribe`には、これまで使ってきた「通常データを受け取る処理（第1引数）」に加えて、名前付き引数`onErrorResume:`と`onCompleted:`を渡すことができます。これにより、1つの`Subscribe`だけで3種類すべての出来事に反応できるようになります。
- `onErrorResume: error => { Debug.LogError(error); }`：発行された`Exception`が、そのまま`error`として渡ってきます。ここで`Debug.LogError`を使うことで、Console上で赤いエラー表示として確認できます。
- `onCompleted: _ => { Debug.Log("OnCompleted"); }`：`OnCompleted`が呼ばれたタイミングで実行される処理です。引数を`_`で受けて捨てているのは、完了通知には基本的に使う値がないためです（内部的には完了理由を表す情報が渡されますが、今回は使いません）。

## 実行結果の例
```
0
1
エラー発生    ← 赤字（Debug.LogErrorによるエラーログ）
OnCompleted
```

## よくあるつまずきポイント
- `OnErrorResume`という名前から「エラーが起きたらストリームが終了する」と誤解しがちですが、このサンプルのように`OnErrorResume`の後も`OnCompleted`まで処理が続いています。「エラーが起きても、ストリームそのものは（明示的に完了させない限り）続く」という点を、実際のログの順番で確認しておきましょう。
- `Subscribe`の第2・第3引数は、書かなくてもコンパイルは通ります（省略した場合、対応する通知が来ても単に無視されます）。今回のように「エラーや完了を検知したい」場合は、`onErrorResume:`・`onCompleted:`を明示的に指定する必要があることを覚えておきましょう。
- コード中の`onErrorResume`（引数名、小文字始まり）とメソッド名`OnErrorResume`（大文字始まり）を混同しないようにしましょう。呼び出す側（発行）は`OnErrorResume`というメソッド、受け取る側（`Subscribe`の引数）は`onErrorResume`という名前付き引数、という違いです。
- `OnCompleted`のスペルミス（`OnComplete`と書いてしまう等）に注意してください。R3のAPIでは必ず過去形の`OnCompleted`です。

## 理解度チェック
1. Observableが送信できる、`OnNext`以外の2種類の通知は何でしたか。
2. `OnErrorResume`が呼ばれた後も、ストリームからのデータ発行は続きますか、それとも終わりますか。
3. `Subscribe`で`OnCompleted`が呼ばれたときの処理を指定するには、どの名前付き引数を使いますか。
