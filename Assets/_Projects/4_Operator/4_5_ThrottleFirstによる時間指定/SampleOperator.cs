using System;
using _Projects.Scripts.Viewer;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Projects._4_Operator._4_5_ThrottleFirstによる時間指定
{
    public class SampleOperator : MonoBehaviour, ITextBinder
    {
        [SerializeField] private float _waitSeconds = 1;
        [SerializeField] private Button _button;

        private readonly ReactiveProperty<int> _count = new(0);
        public ReadOnlyReactiveProperty<string> BindText => _count.Select(x => x.ToString()).ToReadOnlyReactiveProperty();

        public void Start()
        {
            _count.AddTo(this);

            // ボタンの押下を監視
            _button.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(_waitSeconds)) // クリックされてから1秒間は何もしない
                .Subscribe(_ => _count.Value++)
                .AddTo(this);
        }
    }
}