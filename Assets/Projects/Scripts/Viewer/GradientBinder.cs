using R3;
using TNRD;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Viewer
{
    /// <summary>
    ///  値をグラデーションにバインドする機能
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public class GradientBinder : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<ISliderBinder> _target;
        [SerializeField] private Gradient _gradient;

        private void Start()
        {
            if (TryGetComponent<Graphic>(out var graphic))
            {
                _target.Value.SliderValue.Subscribe(x => graphic.color = _gradient.Evaluate(x)).AddTo(this);
            }
        }
    }
}