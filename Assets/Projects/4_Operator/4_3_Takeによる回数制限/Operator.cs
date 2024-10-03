using System;
using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._4_Operator._4_3_Takeによる回数制限
{
    public class Operator : MonoBehaviour, ITextBinder
    {
        public readonly Subject<int> UpdateSubject = new();
        private int _frameCount;

        // UI表示用
        public ReadOnlyReactiveProperty<string> Text =>
            UpdateSubject.Select(x => x.ToString()).ToReadOnlyReactiveProperty();

        private void Start()
        {
            UpdateSubject.AddTo(this);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(3) // 3回だけ発行する
                .Subscribe(_ => UpdateSubject.OnNext(++_frameCount),
                    _ => Debug.Log("完了"))
                .AddTo(this);
        }
    }
}