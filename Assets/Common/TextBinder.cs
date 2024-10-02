using UnityEngine;
using R3;
using TMPro;
using TNRD;

namespace Common
{
    public interface ITextBinder
    {
        Observable<string> TextValue { get; }
    }

    /// <summary>
    /// Observableの値をTextにバインドする
    /// </summary>
    public class TextBinder : MonoBehaviour
    {
        [SerializeField] private string _textFormat = "{0}";
        [SerializeField] private SerializableInterface<ITextBinder> _target;

        private void Start()
        {
            if (TryGetComponent<TextMeshProUGUI>(out var text))
            {
                _target.Value.TextValue.Subscribe(x => text.text = string.Format(_textFormat, x)).AddTo(this);
            }
        }
    }
}