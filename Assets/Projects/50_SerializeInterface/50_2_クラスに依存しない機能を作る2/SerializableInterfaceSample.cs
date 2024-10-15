using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._50_SerializeInterface._50_2_クラスに依存しない機能を作る2
{
    public class SerializableInterfaceSample : MonoBehaviour,
        ITextBinder, // TextBinderで使用するインターフェース
        ISliderBinder // SliderBinderで使用するインターフェース
    {
        [SerializeField] private SerializableReactiveProperty<string> _text = new("表示したい値");
        [SerializeField] private SerializableReactiveProperty<float> _value = new(0.5f);

        // プロパティのgetで表示したい値をBinderに引き渡す
        public ReadOnlyReactiveProperty<string> Text => _text;
        public ReadOnlyReactiveProperty<float> SliderValue => _value;
    }
}