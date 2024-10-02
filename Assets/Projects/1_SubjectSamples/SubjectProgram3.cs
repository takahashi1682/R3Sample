using System;
using R3;
using UnityEngine;

namespace _1_SubjectSamples
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class SubjectProgram3 : MonoBehaviour
    {
        // 時間経過を通知するためのSubject
        public Subject<int> TimerSubject { get; } = new();

        private void Start()
        {
            // このコンポーネントが破棄されたらイベントの発行を終了
            TimerSubject.AddTo(this);

            TimerSubject.OnNext(1);
            TimerSubject.OnNext(2);

            // エラーを発行
            TimerSubject.OnErrorResume(new SystemException("無効な値です"));

            TimerSubject.OnNext(3);
            TimerSubject.OnCompleted();
        }
    }
}