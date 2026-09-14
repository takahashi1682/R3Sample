# 2-4. ReplaySubjectとは

## この回で学ぶこと
- `ReplaySubject<T>`が、通常の`Subject<T>`とは違い「過去に発行したデータを保持しておく」Subjectであることを理解する
- 遅れて`Subscribe`したObserverでも、`ReplaySubject`なら過去のデータをすべて受け取れることを実際に確認する
- 自作クラスにR3の型と同じ名前を付けると紛らわしくなる、という命名上の注意点を知る

## 前提知識
- 「2-0〜2-3」で学んだ、`Subject<T>`の基本（`OnNext`で発行、`Subscribe`で監視、`AddTo`で後片付け）
- 通常の`Subject`は「Subscribeした時点より後に発行されたデータしか受け取れない」という性質（2-1、2-2で体験済み）
- `async`/`await`とUniTaskの`UniTask.Delay`（1-2で軽く登場）

## 概念解説
これまで使ってきた`Subject<T>`には、実は1つの弱点があります。それは、**Subscribeするタイミングより前に発行されたデータは、後から購読しても受け取れない**という点です。例えば「ゲーム開始」というイベントをSubjectで発行した後にUIが初期化された場合、そのUIは「ゲーム開始」の通知を一生受け取れません。

この問題を解決するのが**`ReplaySubject<T>`**です。`ReplaySubject`はSubjectの一種で、**過去に発行したすべてのデータを保持**します。そのため、新しいObserverがSubscribeした際には、これまで通知したすべてのデータを一度に、まとめて受け取ることができます。

たとえるなら、通常の`Subject`が「生放送のラジオ」だとすれば、`ReplaySubject`は「録画済みの動画」です。途中から見始めても（途中からSubscribeしても）、最初から今までの内容をまとめて再生（Replay）してくれる、とイメージすると分かりやすいでしょう。これにより、**Observerがいつ登録されても、過去のデータを見逃すことなく取得できる**ようになります。

## シーンの操作方法
1. Playボタンを押してシーンを実行します。
2. Play開始直後、`SampleReplaySubject`側の`Start`で`0`, `1`, `2`が即座に発行され、続けて`"通知完了"`というログが表示されます。この時点では、まだ`SampleObserver`はSubscribeしていません（後述のコードで3秒待っているため）。
3. Play開始から3秒後、`SampleObserver`が`Subscribe`を実行します。すると、既に発行済みだったはずの`0`, `1`, `2`が、Subscribeした瞬間にまとめてConsoleへ出力されます。
4. 手順2と3のログの間に3秒の間隔が空くこと、そして3秒後に出るログが「新しく発行された値」ではなく「過去に発行済みだった値の再生」であることを、タイムスタンプ（Consoleの左側に表示される時間）で確認してみましょう。

## コード解説
データを発行する側：
```csharp
public class SampleReplaySubject : MonoBehaviour
{
    public ReplaySubject<int> TestSubject { get; } = new();

    private void Start()
    {
        TestSubject.AddTo(this);

        TestSubject.OnNext(0);
        TestSubject.OnNext(1);
        TestSubject.OnNext(2);

        Debug.Log("通知完了");
    }
}
```
- このクラスの名前は**`SampleReplaySubject`**であり、`ReplaySubject`ではありません。R3が提供する型の名前は`ReplaySubject<int>`（`public ReplaySubject<int> TestSubject { get; } = new();`の部分）であり、もしこのMonoBehaviourクラス自体を`ReplaySubject`という名前にしてしまうと、同じ名前のクラスと型がぶつかってしまい、コード上で非常に紛らわしくなります（実際、このプロジェクトでも過去にこの理由でクラス名が`SampleReplaySubject`にリネームされています）。**自分のクラスに、使っているライブラリの型と同じ名前を付けない**というのは、実務でも重要な命名のルールです。
- `TestSubject`自体は`Start`のタイミングで`0`, `1`, `2`という3つの値を発行してしまいます。この時点でSubscribeしているObserverはまだいませんが、`ReplaySubject`はこれらの値を内部に保持し続けます。

データを監視する側：
```csharp
public class SampleObserver : MonoBehaviour
{
    [SerializeField] private SampleReplaySubject _target;

    private async void Awake()
    {
        // 3秒待機
        await UniTask.Delay(TimeSpan.FromSeconds(3));

        // イベントを監視
        _target.TestSubject.Subscribe(x => Debug.Log(x)).AddTo(this);
    }
}
```
- `private async void Awake()`：`Awake`の中で`await`を使うために、メソッドに`async`を付けています。`async void`はイベントハンドラなど限られた場面以外では推奨されないことが多いですが、Unityのイベント関数（`Awake`など）に`async`を付けてUniTaskの非同期処理を書く、というのはUniTaskではよく使われるパターンです。
- `await UniTask.Delay(TimeSpan.FromSeconds(3));`：ここで3秒待つことで、意図的に「Subject側がデータを発行し終わった後」にSubscribeするタイミングを作り出しています。
- `_target.TestSubject.Subscribe(x => Debug.Log(x)).AddTo(this);`：3秒遅れてSubscribeしているにもかかわらず、`ReplaySubject`のおかげで過去に発行された`0`, `1`, `2`をすべて受け取ることができます。もしこれが通常の`Subject<int>`であれば、3秒後にSubscribeしても、その後発行される値がなければ何も受け取れずに終わっていたはずです（2-1レッスンの内容を思い出しましょう）。

## 実行結果の例
Consoleには、以下のような順番でログが出力されます。

```
通知完了                （Play直後、SampleReplaySubject.Startの最後で出力）
（3秒後）
0                       （SampleObserverがSubscribeした瞬間、まとめて再生される）
1
2
```
`OnNext`を呼んだだけではログは出ず、あくまで`Subscribe`した側で`Debug.Log(x)`を実行して初めてConsoleに表示される点に注意してください。

## よくあるつまずきポイント
- 「`0`, `1`, `2`はいつ発行された値なのか」を誤解しがちです。実際に発行されたのはPlay開始直後（0秒台）ですが、Consoleにログとして表示されるのは3秒後、`SampleObserver`がSubscribeした瞬間です。「発行のタイミング」と「ログに表示されるタイミング」がズレていることに注意してください。
- 通常の`Subject<int>`との違いを、動作の違いとしてきちんと説明できるようにしておきましょう。もし`SampleReplaySubject`の型を`Subject<int>`に書き換えて同じ手順で試すと、3秒後にSubscribeしても`0`, `1`, `2`は一切表示されなくなります（余裕があれば実際に試してみることをお勧めします）。
- 自作のMonoBehaviourクラス名を、使っているライブラリの型名とうっかり同じにしてしまうと、コード補完やコンパイルエラーで混乱の原因になります。このレッスンの`SampleReplaySubject`という命名（`ReplaySubject`そのものではない）を、良い実例として覚えておきましょう。

## 理解度チェック
1. `ReplaySubject<T>`が、通常の`Subject<T>`と違う点は何でしたか。
2. このレッスンのサンプルで、`SampleObserver`が3秒待ってからSubscribeしているにもかかわらず、`0`, `1`, `2`をすべて受け取れるのはなぜですか。
3. このMonoBehaviourのクラス名が、R3が提供する型と同じ`ReplaySubject`ではなく`SampleReplaySubject`になっているのはなぜですか。
