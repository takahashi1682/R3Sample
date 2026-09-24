using System;
using _Projects.Scripts.Viewer;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Projects._4_Operator._4_8_Switchによる購読切り替え
{
    public class SampleOperator : MonoBehaviour, ITextBinder
    {
        [SerializeField] private float _interval = 1;
        [SerializeField] private Button _button;

        private readonly ReactiveProperty<int> _count = new(0);
        private ReadOnlyReactiveProperty<string> _bindText;
        public ReadOnlyReactiveProperty<string> BindText => _bindText ??=
            _count.Select(x => x.ToString()).ToReadOnlyReactiveProperty().AddTo(this);

        public void Start()
        {
            _count.AddTo(this);

            // ボタンを押すたびに「_interval秒ごとに通知するObservable」を新しく作る
            // → Observable<Observable<Unit>> になる
            _button.OnClickAsObservable()
                .Select(_ =>
                {
                    _count.Value = 0; // カウントをリセット
                    return Observable.Interval(TimeSpan.FromSeconds(_interval)); // 新しいObservableを返す
                })
                // 最後に作られたObservableだけを購読し、以前のObservableの購読は自動で解除する
                .Switch()
                .Subscribe(_ => _count.Value++)
                .AddTo(this);
        }
    }
}
