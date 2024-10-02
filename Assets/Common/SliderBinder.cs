using R3;
using TNRD;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public interface ISliderBinder
    {
        Observable<float> SliderValue { get; }
    }

    /// <summary>
    ///  Observableの値をSliderに反映する
    /// </summary>
    public class SliderBinder : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<ISliderBinder> _target;

        private void Start()
        {
            if (TryGetComponent<Slider>(out var slider))
            {
                _target.Value.SliderValue.Subscribe(x => slider.value = Mathf.Clamp01(x)).AddTo(this);
            }
        }
    }
}