# 4-7. AwaitOperation

## この回で学ぶこと
- `SubscribeAwait` を使って、購読処理の中で `async`/`await` の非同期処理を実行する方法
- `AwaitOperation`（`Sequential` / `Drop` / `Switch` / `Parallel`）が、非同期処理の「多重実行」をどう制御するかの違い
- `CancellationToken` を非同期処理に渡し、安全に処理を中断できるようにする理由
- Observable と UniTask（`async`/`await`）を橋渡しする考え方（この後の UniTask の単元につながる内容です）

## 前提知識
- 4-1〜4-6 で学んだ各種オペレーター、特に `Button.OnClickAsObservable()`（4-5 で使用）
- C# の `async`/`await` の基本文法（UniTask 単元より前ですが、最低限の読み方を掴んでおくと理解が早まります）
- `CancellationToken` が「処理の中断を伝えるための合図」であるという基礎知識

## 概念解説
これまでの `Subscribe` は「値が流れてきたら、その場で同期的に処理を実行する」ものでした。しかし実際の開発では、「ボタンを押したら3秒かけてスライダーが動くアニメーション処理を実行する」のように、**時間のかかる非同期処理**を Observable の購読の中で実行したい場面が出てきます。

`SubscribeAwait` は、その名の通り「`Subscribe` の中で `await` を使えるようにする」オペレーターです。ただし、非同期処理は完了するまでに時間がかかるため、**その処理が終わる前に次のイベント（例えば連打）が来たらどうするか**を決めておく必要があります。この「同時に複数の非同期処理が発生しそうになったときの交通整理のルール」を指定するのが `AwaitOperation` です。このシーンでは4つのルールを扱います。

- **Sequential（順番待ち）**：新しいクリックが来ても、前のTaskが終わるまでキューに積んで待たせ、順番に実行する
- **Drop（無視）**：Taskが実行中の間に来たクリックは、まるごと捨てる
- **Switch（乗り換え）**：新しいクリックが来たら、実行中のTaskをキャンセルして新しいTaskに切り替える
- **Parallel（並行実行）**：クリックが来るたびに、同時並行でTaskを実行する

これは、**券売機の行列のさばき方**に例えられます。Sequential は「後から来た人は列の後ろに並ばせる」、Drop は「今対応中なら新しく来た人は追い返す」、Switch は「今の対応を中断して新しく来た人を優先する」、Parallel は「窓口を増やして全員同時に対応する」というイメージです。どれが正しいというわけではなく、**処理の性質に応じて選ぶべきもの**です。

## シーンの操作方法
このシーンには4つのボタン（`Sequential` / `Drop` / `Switch` / `Parallel`）があり、それぞれが**同じ1本のスライダー**を操作する、別々の `AwaitOperationSample` コンポーネントに接続されています。

1. Play してシーンを実行します。
2. まず `Sequential` ボタンを1回押し、スライダーが3秒かけて 0 → 1 まで動くのを確認します。
3. `Sequential` ボタンをスライダーが動いている最中に連打してみましょう。クリックした回数分、動作が終わるたびに**また最初から3秒アニメーションが繰り返される**（＝クリックがキューに積まれて順番に処理される）ことを確認します。
4. 次に `Drop` ボタンで同様に連打してみます。今度はアニメーション中の連打は無視され、最初のアニメーションが終わるまで反応しないことを確認します。
5. `Switch` ボタンで連打してみます。連打するたびに進行中のアニメーションが中断され、また 0 から新しいアニメーションが始まり直すことを確認します。
6. `Parallel` ボタンで連打してみます。内部的には複数の非同期処理が同時に走りますが、スライダー自体は1つしかないため、複数の更新処理が同じスライダーを取り合うような、他の3つとは異なる挙動になることを確認しましょう。
7. 4つのボタンを見比べて、同じ「3秒かけてスライダーを動かす」処理でも、連打したときの見え方がまったく違うことを体感してください。

## コード解説
```csharp
public class AwaitOperationSample : MonoBehaviour
{
    [SerializeField] private AwaitOperation _awaitOperation;
    [SerializeField] private Button _button;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _waitTime = 3f;

    private void Start()
    {
        _button.OnClickAsObservable()
            .SubscribeAwait(async (_, ct) =>
                {
                    await UpdateSlider(ct);
                },
                _awaitOperation) // 複数の非同期処理の制御設定
            .AddTo(this);
    }

    private async UniTask UpdateSlider(CancellationToken token)
    {
        var elapsedTime = 0f;
        while (elapsedTime < _waitTime && !token.IsCancellationRequested)
        {
            elapsedTime += Time.deltaTime;
            var rate = Mathf.Clamp01(elapsedTime / _waitTime);
            _slider.value = rate;
            await UniTask.Yield(token);
        }
    }
}
```
- `SubscribeAwait(async (_, ct) => { await UpdateSlider(ct); }, _awaitOperation)`：`Subscribe` とよく似ていますが、ラムダ式が `async` になっており、第2引数として `CancellationToken`（`ct`）を受け取れる点が違います。第2引数の `_awaitOperation` に、先ほど説明した4つの制御モードのいずれかを渡します。このシーンでは、4つのボタンそれぞれに異なる `AwaitOperation` の値が Inspector で設定されています。
- `CancellationToken ct` は「この非同期処理を途中でやめるべきかどうか」を伝えるための合図です。`Switch` モードのように「前の処理を中断して新しい処理に切り替える」動作は、この `CancellationToken` を使って前の `UpdateSlider` に「もう中断していいよ」と伝えることで実現されています。
- `UpdateSlider` メソッドのループの中で `!token.IsCancellationRequested` を毎回チェックしているのがポイントです。これを確認せずに `while` を回し続けると、キャンセルされたはずの古い処理がスライダーを更新し続けてしまい、`Switch` や `Drop` の意図した挙動が壊れてしまいます。**非同期処理を安全に中断可能にするには、処理の途中で定期的に `CancellationToken` の状態を確認する必要がある**という、非同期プログラミングの重要な作法がここに表れています。
- `await UniTask.Yield(token)`：ここにも `token` を渡すことで、`Yield`（1フレーム待つ）のタイミングでもキャンセルの合図を検知できるようにしています。
- この `SubscribeAwait` は、Observable（R3）の世界と `async`/`await`（UniTask）の世界を橋渡しする役割を持っています。この後の UniTask の単元では、`async`/`await` そのものをより詳しく学びますが、今回のサンプルは「Observable の購読から非同期処理を安全に呼び出すには、こういう考慮が必要になる」という予告編として捉えてください。

## 実行結果の例
- `Sequential`：連打した回数分、3秒アニメーションが順番に、途切れず連続して再生される
- `Drop`：アニメーション中の連打はすべて無視され、最初の1回分の3秒アニメーションだけが再生される
- `Switch`：連打するたびに、それまでのアニメーションが打ち切られ、0からアニメーションが再スタートする
- `Parallel`：連打した回数分の非同期処理が同時に走り、スライダーの値が複数の処理から競合して更新される様子が見られる

## よくあるつまずきポイント
- 4つのボタンが「同じスクリプト」の「異なる設定違い」であることに気づかず、コードが4パターンあると勘違いしてしまう。実際には `AwaitOperationSample` は1つのクラスで、Inspector 上の `Await Operation` の値だけが4つのボタンで異なります。
- `CancellationToken` を「使わなくても動くから省略していいもの」だと考えてしまう。`Switch` や `Drop` のように処理を打ち切る必要があるモードでは、`CancellationToken` を正しく扱っていないと、キャンセルしたはずの古い処理が居座り続けてスライダーの値を意図せず書き換えてしまいます。
- `Parallel` の結果が「一番自然」に見えるかもしれませんが、同じリソース（この場合はスライダー）を複数の非同期処理が同時に触るとどうなるかを実際に確認することで、なぜ用途に応じて `Sequential` や `Switch` を選ぶ必要があるのかを体感するのがこのシーンの狙いです。

## 理解度チェック
1. `AwaitOperation` の `Sequential` と `Drop` は、連打したときの挙動がそれぞれどう違いますか。
2. `UpdateSlider` メソッドの `while` ループの中で `!token.IsCancellationRequested` をチェックしているのはなぜですか。チェックを外すと、`Switch` モードでどんな不具合が起きると考えられますか。
3. `SubscribeAwait` が橋渡ししている2つの世界（Observable の世界と、もう1つ）は何ですか。
