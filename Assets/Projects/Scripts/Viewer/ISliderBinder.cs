using R3;

namespace Projects.Viewer
{
    public interface ISliderBinder
    {
        ReadOnlyReactiveProperty<float> SliderValue { get; }
    }
}