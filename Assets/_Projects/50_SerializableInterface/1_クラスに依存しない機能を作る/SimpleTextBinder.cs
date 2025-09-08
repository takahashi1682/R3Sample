using TMPro;
using TNRD;
using UnityEngine;

namespace _Projects._50_SerializableInterface._1_クラスに依存しない機能を作る
{
    public interface ISimpleTextBinder
    {
        string Text { get; }
    }

    /// <summary>
    ///  値をテキストにバインドする機能
    /// </summary>
    public class SimpleTextBinder : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<ISimpleTextBinder> _target;
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            _text.text = _target.Value.Text;
        }
    }
}