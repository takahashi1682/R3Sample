using R3;

namespace _Projects.Scripts.Viewer
{
    public interface ISliderBinder
    {
        ReadOnlyReactiveProperty<float> BindSlider { get; }
    }
}