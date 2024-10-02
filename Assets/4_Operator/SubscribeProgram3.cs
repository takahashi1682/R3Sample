using System;
using UnityEngine;
using R3;

namespace _4_Operator
{
    public class SubscribeProgram3 : MonoBehaviour
    {
        [SerializeField] private OperatorProgram2 _target;

        private void Start()
        {
            _target.Timer
                .Subscribe(
                    UpdateTime, // 制限時間の更新(onNext)
                    _ => GameOver()) // 時間切れ時の処理(onCompleted)
                .AddTo(this);
        }

        private void UpdateTime(int time) => Debug.Log($"Time: {time}");
        private void GameOver() => Debug.Log("GameOver");
    }
}