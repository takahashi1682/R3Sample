using System;
using R3;
using UnityEngine;

namespace _4_Operator
{
    public class OperatorProgram2 : MonoBehaviour
    {
        [SerializeField] private int _maxTime = 5;
        [SerializeField] private int _currentTime;

        public Observable<int> Timer;

        public void Awake()
        {
            _currentTime = _maxTime;

            // 1秒ごとに_currentTimeを1減らし、0になったら終了するObservable
            Timer = Observable.Interval(TimeSpan.FromSeconds(1),
                    destroyCancellationToken) // このObservableが破棄された時に処理を終了するCancellationToken
                .Select(_ => _currentTime--) // 購読者に_currentTimeを通知
                .TakeWhile(x => x >= 0); // 終了条件（onCompleted）
        }
    }
}