using R3;
using TMPro;
using TNRD;
using UnityEngine;

namespace Projects.Viewer
{
    public interface ITextBinder
    {
        ReadOnlyReactiveProperty<string> Text { get; }
    }

    /// <summary>
    ///  値をテキストにバインドする機能
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextBinder : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<ITextBinder> _target;
        [SerializeField] private string _textFormat = "{0}";

        private void Start()
        {
            if (TryGetComponent<TextMeshProUGUI>(out var text))
            {
                _target.Value.Text.Subscribe(x => text.text = string.Format(_textFormat, x)).AddTo(this);
            }
        }
    }
}