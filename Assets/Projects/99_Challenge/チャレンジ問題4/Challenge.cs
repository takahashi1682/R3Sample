using System;
using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題4
{
    public class Challenge : MonoBehaviour, ITextBinder
    {
        private readonly ReactiveProperty<string> _text = new();
        public ReadOnlyReactiveProperty<string> Text => _text;

        private void Start()
        {
            _text.AddTo(this);

            Observable.EveryUpdate()
                .Select(_ => DateTime.Now.Millisecond)
//                .Select(_ => DateTime.Now.Millisecond)
                .Subscribe(x => _text.Value = x.ToString())
                .AddTo(this);
        }
    }
}