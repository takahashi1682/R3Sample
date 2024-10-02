using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _4_Operator
{
    public class OperatorProgram3ThrottleLast : MonoBehaviour, ISliderBinder
    {
        [SerializeField] private float _waitSeconds = 3;
        [SerializeField] private Button _button;
        private TextMeshProUGUI _text;

        private readonly SerializableReactiveProperty<float> _progress = new();
        public Observable<float> SliderValue => _progress;

        public void Start()
        {
            _text = _button.GetComponentInChildren<TextMeshProUGUI>();

            // クリックされてから1秒間は何もしない
            _button.OnClickAsObservable()
                .ThrottleLast((_, ct) => UpdateGaugeAsync(ct))
                .Subscribe(_ =>
                {
                    _text.text = "OnNext!!";
                    Debug.Log("ThrottleLast");
                })
                .AddTo(this);
        }

        /// <summary>
        /// プログレスの更新（非同期処理）
        /// </summary>
        /// <param name="ct"></param>
        private async UniTask UpdateGaugeAsync(CancellationToken ct)
        {
            _text.text = "処理中...";
            _progress.Value = 0;

            var currentTime = 0f;
            // キャンセルされるまで、かつ進捗が1になるまで待つ
            while (!ct.IsCancellationRequested && _progress.Value < 1)
            {
                await UniTask.Yield(); // 1フレーム待つ
                currentTime += Time.deltaTime;

                // 進捗を更新(計算結果を0~1に制限)
                _progress.Value = Mathf.Clamp01(currentTime / _waitSeconds);
            }

            if (!ct.IsCancellationRequested)
            {
                _text.text = "完了!!";
            }
        }
    }
}