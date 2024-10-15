using R3;
using TNRD;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Viewer
{
    /// <summary>
    ///  値をスライダーにバインドする機能
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class SliderBinder : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<ISliderBinder> _target;

        private void Start()
        {
            if (TryGetComponent<Slider>(out var slider))
            {
                _target.Value.SliderValue.Subscribe(x => slider.value = x).AddTo(this);
            }
        }
    }
}