using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._50_SerializableInterface._2_クラスに依存しない機能を作る2
{
    public class SerializableInterfaceSample : MonoBehaviour,
        ITextBinder, // TextBinderで使用するインターフェース
        ISliderBinder // SliderBinderで使用するインターフェース
    {
        [SerializeField] private SerializableReactiveProperty<string> _bindText = new("表示したい値");
        [SerializeField] private SerializableReactiveProperty<float> _value = new(0.5f);

        // TextBinderに表示する値を引き渡す
        public ReadOnlyReactiveProperty<string> BindText => _bindText;

        // SliderBinderに表示する値を引き渡す
        public ReadOnlyReactiveProperty<float> BindSlider => _value;

        // [SerializeField] private TextMeshProUGUI _textComponent;
        // [SerializeField] private Slider _slider;

        private void Start()
        {
            _bindText.AddTo(this);
            _value.AddTo(this);

            // 値が更新されたらUIに反映する
            // _text.Subscribe(x => _textComponent.text = x).AddTo(this);
            // _value.Subscribe(x => _slider.value = x).AddTo(this);
        }
    }
}