using System;
using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題7
{
    public class Challenge : MonoBehaviour, ITextBinder
    {
        private readonly ReactiveProperty<int> _count = new();
        public ReadOnlyReactiveProperty<int> Count => _count;

        // UI表示用
        public ReadOnlyReactiveProperty<string> Text => _count.Select(x => x.ToString()).ToReadOnlyReactiveProperty();

        private void Start()
        {
            _count.AddTo(this);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(_ => ++_count.Value)
                .Subscribe(x => Debug.Log(x))
                .AddTo(this);
        }
    }
}