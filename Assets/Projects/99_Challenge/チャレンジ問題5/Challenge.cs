using System;
using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題5
{
    public class Challenge : MonoBehaviour, ITextBinder
    {
        private readonly ReactiveProperty<int> _count = new(1000);

        // UI表示用
        public ReadOnlyReactiveProperty<string> BindText => _count.Select(x => x.ToString()).ToReadOnlyReactiveProperty();

        private void Awake()
        {
            _count.AddTo(this);
        }
    }
}