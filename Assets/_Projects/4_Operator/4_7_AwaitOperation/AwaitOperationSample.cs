using System;
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
                        // 非同期処理を実行（終わるまで待機）
                        await UpdateSlider(ct);
                    },
                    _awaitOperation) // 複数の非同期処理の制御設定
                .AddTo(this);
        }

        /// <summary>
        /// スライダーの値を更新する（指定した時間待機して、その進捗率をスライダーに反映する）
        /// </summary>
        /// <param name="token"></param>
        private async UniTask UpdateSlider(CancellationToken token)
        {
            var elapsedTime = 0f;
            //       現在の時間  < 待機時間　かつ　キャンセルトークンがキャンセルされていない場合
            while (elapsedTime < _waitTime && !token.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;// 経過時間を加算
                //                          現在の時間 / 待機時間　→　0~1の範囲に収める
                var rate = Mathf.Clamp01(elapsedTime / _waitTime);
                _slider.value = rate; // スライダーにセット
                await UniTask.Yield(token); // 1フレーム待機（キャンセル可能）
            }
        }
    }
}