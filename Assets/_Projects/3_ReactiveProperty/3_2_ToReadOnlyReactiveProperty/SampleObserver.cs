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
            _slider.maxValue = _program1.MaxHealth.CurrentValue;
            _slider.value = _program1.CurrentHealth.CurrentValue;
            _slider.onValueChanged.AddListener(x => _program1.SetValue(x));

            // テキストの設定
            _program1.CurrentHealth
                .Subscribe(x => _currentHeathText.text = x.ToString("N0")).AddTo(this);
            _program1.MaxHealth
                .Subscribe(x => _maxHeathText.text = x.ToString("N0")).AddTo(this);
            _program1.HealthRate
                .Subscribe(x => _heathRateText.text = x.ToString("N2")).AddTo(this);

            _program1.IsDead
                .Subscribe(x => _deadText.text = x.ToString()).AddTo(this);
        }
    }
}