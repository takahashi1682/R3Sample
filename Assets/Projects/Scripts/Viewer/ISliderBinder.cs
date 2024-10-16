using R3;

namespace Projects.Viewer
{
    public interface ISliderBinder
    {
        ReadOnlyReactiveProperty<float> BindSlider { get; }
    }
}