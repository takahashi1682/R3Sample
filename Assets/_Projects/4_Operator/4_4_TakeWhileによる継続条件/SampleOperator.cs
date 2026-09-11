using System;
using _Projects.Scripts.Viewer;
using R3;
using UnityEngine;

namespace _Projects._4_Operator._4_4_TakeWhileによる継続条件
{
    public class SampleOperator : MonoBehaviour, ITextBinder
    {
        public readonly Subject<int> UpdateSubject = new();
        private int _frameCount;

        // UI表示用
        private ReadOnlyReactiveProperty<string> _bindText;
        public ReadOnlyReactiveProperty<string> BindText => _bindText ??=
            UpdateSubject.Select(x => x.ToString()).ToReadOnlyReactiveProperty().AddTo(this);

        private void Start()
        {
            // readonlyにAddToは不要だが、念のためDisposeされるようにしておく
            UpdateSubject.AddTo(this);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .TakeWhile(_ => _frameCount < 3) // 3回だけ発行する
                .Subscribe(
                    _ => UpdateSubject.OnNext(++_frameCount),
                    _ => Debug.Log("完了"))
                .AddTo(this);
        }
    }
}