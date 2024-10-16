using UnityEngine;

namespace Projects._50_SerializeInterface._1_クラスに依存しない機能を作る
{
    public class SerializableInterfaceSample : MonoBehaviour, ITextBinder
    {
        [SerializeField] private string _text = "表示したい値";

        // ITextBinderに含まれるプロパティ
        public string Text => _text;
    }
}