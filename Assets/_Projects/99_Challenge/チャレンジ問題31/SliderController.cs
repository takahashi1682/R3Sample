using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Projects._99_Challenge.チャレンジ問題31
{
    public class SliderController : MonoBehaviour
    {
        [SerializeField] private Challenge _challenge;
        [SerializeField] private Slider _slider;
        [SerializeField] private Graphic _fill;

        private void Start()
        {
            // 時間比率をスライダーに反映する(1行)
            _challenge.CurrentRate.Subscribe(rate => _slider.value = rate).AddTo(this);

            // 開始イベントが発火したらスライダーの色を赤に変更する
            _challenge.OnStart.Subscribe(_ => _fill.color = Color.red).AddTo(this);

            // 時間比率が1になったらスライダーの色を緑に変更する
            _challenge.OnComplete.Subscribe(_ => _fill.color = Color.green).AddTo(this);
        }
    }
}