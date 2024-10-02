using R3;

namespace Projects.Viewer
{
    public interface IBinderRate
    {
        ReadOnlyReactiveProperty<float> Rate { get; }
    }
}