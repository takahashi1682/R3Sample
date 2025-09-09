using System.Globalization;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Projects._3_ReactiveProperty._3_2_ToReadOnlyReactiveProperty
{
    public class SampleObserver : MonoBehaviour
    {
        [SerializeField] private Program _program1;
        [SerializeField] private Slider _slider;
        [SerializeField] private TMPro.TextMeshProUGUI _currentHeathText;
        [SerializeField] private TMPro.TextMeshProUGUI _maxHeathText;
        [SerializeField] private TMPro.TextMeshProUGUI _heathRateText;
        [SerializeField] private TMPro.TextMeshProUGUI _deadText;

        private void Awake()
        {
            // スライダーの設定
            _slider.maxValue = _program1.MaxHeath.CurrentValue;
            _slider.value = _program1.CurrentHeath.CurrentValue;
            _slider.onValueChanged.AddListener(x => _program1.SetValue(x));

            // テキストの設定
            _program1.CurrentHeath
                .Subscribe(x => _currentHeathText.text = x.ToString("N0")).AddTo(this);
            _program1.MaxHeath
                .Subscribe(x => _maxHeathText.text = x.ToString("N0")).AddTo(this);
            _program1.HeathRate
                .Subscribe(x => _heathRateText.text = x.ToString("N2")).AddTo(this);

            // ToStringにCultureInfo.CurrentCultureを指定すると、カンマ区切りや小数点の表記が実行環境によって変わるようになる
            // 日本語環境だと小数点は「.」で区切られるがヨーロッパ圏だと「,」で区切られる
            _program1.IsDead
                .Subscribe(x => _deadText.text = x.ToString(CultureInfo.CurrentCulture)).AddTo(this);
        }
    }
}