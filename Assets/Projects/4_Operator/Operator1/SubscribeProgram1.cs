using System;
using UnityEngine;
using R3;
using R3.Triggers;

namespace _4_Operator
{
    public class SubscribeProgram1 : MonoBehaviour
    {
        [SerializeField] private OperatorProgram1 _target;
        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;

            // GameStateの値がMoveの時にMoveメソッドを実行
            this.FixedUpdateAsObservable()
                .Where(_ => _target.MoveMode.CurrentValue == EMoveMode.Pingpong) // 実行条件
                .Subscribe(_ => Move()
                ).AddTo(this);

            // GameStateの値がRotateの時にRotateメソッドを実行
            this.FixedUpdateAsObservable()
                .Where(_ => _target.MoveMode.CurrentValue == EMoveMode.Rotate) // 実行条件
                .Subscribe(_ => Rotate()
                ).AddTo(this);
        }
        
        private void Move() =>
            transform.position = _startPosition + new Vector3(Mathf.PingPong(Time.time, 1), 0);

        private void Rotate() =>
            transform.position = _startPosition + new Vector3(Mathf.Cos(Time.time), Mathf.Sin(Time.time), 0);
    }
}