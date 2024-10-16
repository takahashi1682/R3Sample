using TMPro;
using TNRD;
using UnityEngine;

namespace Projects._50_SerializeInterface._1_クラスに依存しない機能を作る
{
    public interface ITextBinder
    {
        string Text { get; }
    }

    /// <summary>
    ///  値をテキストにバインドする機能
    /// </summary>
    public class SimpleTextBinder : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<ITextBinder> _target;
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            _text.text = _target.Value.Text;
        }
    }
}