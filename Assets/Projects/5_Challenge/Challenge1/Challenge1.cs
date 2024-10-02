using System;
using Projects.CharacterParameter;
using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._5_Challenge.Challenge1
{
    public class Challenge1 : MonoBehaviour, ITextBinder, IBinderRate
    {
        [SerializeField] private Health _health;

        // UI表示用
        public ReadOnlyReactiveProperty<string> Text =>
            Observable.CombineLatest(_health.Current, _health.Max).Select(p => $"{p[0]} / {p[1]}")
                .ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<float> Rate => _health.Rate.ToReadOnlyReactiveProperty();

        private void Start()
        {
            // -- ここに処理を追加してください --

        }
    }
}