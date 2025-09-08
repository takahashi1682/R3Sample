using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Projects._4_Operator._4_7_AwaitOperation
{
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
                        // 非同期処理を実行
                        await UpdateSlider(ct);
                    },
                    _awaitOperation) // 複数の非同期処理の制御設定
                .AddTo(this);
        }

        /// <summary>
        /// スライダーの値を更新する
        /// </summary>
        /// <param name="token"></param>
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
}