using _Projects.Scripts.CharacterParameter;
using _Projects.Scripts.Viewer;
using R3;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題1
{
    public class Challenge : MonoBehaviour, ITextBinder, ISliderBinder
    {
        [SerializeField] private Health _health;

        // UI表示用
        public ReadOnlyReactiveProperty<string> BindText =>
            Observable.CombineLatest(_health.Current, _health.Max).Select(p => $"{p[0]} / {p[1]}")
                .ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<float> BindSlider => _health.Rate.ToReadOnlyReactiveProperty();

        private void Start()
        {
            // -- ここに処理を追加してください --

        }
    }
}