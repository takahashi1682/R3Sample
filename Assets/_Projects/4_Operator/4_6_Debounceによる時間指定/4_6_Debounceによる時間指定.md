# 4-6. Debounceによる時間指定

## この回で学ぶこと
- `Debounce` オペレーターで、「入力が止まってから一定時間後」にだけ処理を実行する方法
- `ThrottleFirst`（前回）と `Debounce`（今回）の違いを整理する
- `OnMouseEnterAsObservable` / `OnMouseExitAsObservable` を使ったマウス操作の Observable 化

## 前提知識
- 4-5 で学んだ `ThrottleFirst` の時間制御の考え方
- `R3.Triggers` 名前空間のマウスイベント系メソッド（`OnMouseEnterAsObservable` など）
- `Collider` が無いとマウスイベントが発生しないという Unity の基礎知識

## 概念解説
`Debounce` は「入力が続いている間はタイマーをリセットし続け、入力が止まってから指定時間が経過したときにだけ、最後の値を1回だけ流す」オペレーターです。

身近な例で言うと、**エレベーターの「開く」ボタンの自動ドアタイマー**に似ています。人が乗り込んでくるたびにタイマーがリセットされ、しばらく誰も乗ってこなくなってから初めてドアが閉まります。人が来るたびにタイマーが延長される点が、`Debounce` の一番の特徴です。

```csharp
_cube.OnMouseEnterAsObservable()
    .Debounce(TimeSpan.FromSeconds(_waitSeconds))
    .Subscribe(_ => _render.material.color = Color.green)
    .AddTo(_cube);
```

このサンプルでは、カーソルが Cube に乗ってから（`OnMouseEnter`）`Debounce` で3秒間の猶予を置き、その間ずっとカーソルが Cube 上にとどまっていれば、3秒後に色が緑に変わります。

ここで重要なのは、`OnMouseEnterAsObservable()` は「カーソルが乗った瞬間」にしか値を流さないイベントだという点です。つまり `Debounce` が待っているのは「新しい `OnMouseEnter` が来ないこと」であり、「マウスが動いていないこと」ではありません。このサンプルの Cube は左右に動き続けているため、実質的には「カーソルが一度乗ってから3秒間、Cube 側からもう一度カーソルの真下に来られない（＝再度 `OnMouseEnter` が発生しない）限り、色が変わる」という挙動になります。

## シーンの操作方法
1. Play してシーンを実行します。Cube は赤色で左右に往復移動しています。
2. マウスカーソルを動く Cube の上に重ねてみましょう。
3. カーソルを乗せてから3秒間、色が変わらないことを確認します（`Wait Seconds` のデフォルト値は3秒）。
4. 3秒経過すると Cube の色が緑に変わることを確認します。
5. カーソルを Cube から外すと、すぐに赤色へ戻ることを確認します（`OnMouseExit` は `Debounce` を挟まず即座に反映されるため）。
6. Hierarchy の `Scripts` オブジェクトの `SampleOperator` コンポーネントで `Wait Seconds` の値を短くして、挙動の変化を確認してみましょう。

## コード解説
```csharp
public class SampleOperator : MonoBehaviour
{
    [SerializeField] private float _waitSeconds = 3;
    [SerializeField] private Transform _cube;
    [SerializeField] private Renderer _render;

    public void Start()
    {
        _render.material.color = Color.red;

        _cube.OnMouseEnterAsObservable()
            .Debounce(TimeSpan.FromSeconds(_waitSeconds))
            .Subscribe(_ => _render.material.color = Color.green)
            .AddTo(_cube);

        _cube.OnMouseExitAsObservable()
            .Subscribe(_ => _render.material.color = Color.red)
            .AddTo(_cube);

        _cube.FixedUpdateAsObservable()
            .Subscribe(
                _ => _cube.position = new Vector3(Mathf.PingPong(Time.time * 2, 5), _cube.position.y))
            .AddTo(_cube);
    }
}
```
- `_cube.OnMouseEnterAsObservable().Debounce(...)`：`Debounce` を挟んでいるのは「色を緑に変える」処理だけです。マウスが乗った瞬間に即座に色を変えたいわけではなく、「一定時間乗り続けた」ことを確認してから変えたい、という意図がここに表れています。
- `_cube.OnMouseExitAsObservable().Subscribe(...)`：こちらには `Debounce` を挟んでいません。カーソルが外れたら**即座に**赤に戻したいからです。「入るときはじっくり待つが、出るときは即反応する」という非対称な設計は、実際のゲームUIの演出でもよく使われる考え方です。
- `.AddTo(_cube)`：これまでの `.AddTo(this)` と違い、ここでは `_cube`（`Transform`）を破棄の基準にしています。`AddTo` は「このコンポーネント（正確には GameObject）が破棄されたら一緒に Dispose する」という意味なので、Cube 自身が破棄されたタイミングでこれらの購読も片付くようにしている、という違いに注目してください。
- `_cube.FixedUpdateAsObservable()`：4-1 で学んだのと同じ Trigger 系メソッドで、Cube を左右に往復させています。この移動処理があることで、「動いているターゲットに対してマウスを合わせ続ける」という、より実践的な操作感になっています。

## 実行結果の例
- カーソルを Cube に乗せた瞬間：色はまだ赤のまま
- 乗せてから3秒後（カーソルを外していなければ）：色が緑に変わる
- 緑になった後にカーソルを外す：即座に赤へ戻る

## よくあるつまずきポイント
- `Debounce` と `ThrottleFirst` の違いが曖昧になりがちです。`ThrottleFirst` は「最初の入力をすぐ通し、その後一定時間は無視する」オペレーターですが、`Debounce` は逆に「入力が止まってから一定時間後に、最後の値を1回だけ通す」オペレーターです。
- このサンプルの `Debounce` が待っているのは「マウスが動いていないこと」ではなく「新しい `OnMouseEnter` イベントが発生しないこと」である点を誤解しやすいです。カーソルが Cube の上で小刻みに動いていても、`OnMouseEnter` が再発火しない限りタイマーはリセットされません。
- マウスイベントを使う都合上、Cube に `Collider` が付いていないとそもそも `OnMouseEnter`/`OnMouseExit` が発火しません。もし挙動が確認できない場合は、まず Collider の有無を確認しましょう。

## 理解度チェック
1. `Debounce` と `ThrottleFirst` の動作の違いを説明してください。
2. このサンプルで `OnMouseExit` 側に `Debounce` を付けていないのはなぜだと考えられますか。
3. `.AddTo(_cube)` と `.AddTo(this)` は何が違いますか。
