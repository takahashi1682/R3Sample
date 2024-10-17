using System;
using System.Globalization;
using Projects.Viewer;
using UnityEngine;
using R3;

namespace Projects._99_Challenge.チャレンジ問題10
{
    public class SimpleTImer : MonoBehaviour, ITextBinder
    {
        [SerializeField] private SerializableReactiveProperty<float> _time = new();
        public ReactiveProperty<float> Time => _time;

        [SerializeField] private float _addTime = 1;
        [SerializeField] private float _startInterval = 1;
        [SerializeField] private float _initialTime = 2;

        public ReadOnlyReactiveProperty<string> BindText => Time.Select(x => x.ToString(CultureInfo.InvariantCulture))
            .ToReadOnlyReactiveProperty();

        private void Start()
        {
            _time.AddTo(this);
            Observable.Timer(TimeSpan.FromSeconds(_startInterval), TimeSpan.FromSeconds(_initialTime))
                .Subscribe(_ => _time.Value += _addTime)
                .AddTo(this);
        }
    }
}