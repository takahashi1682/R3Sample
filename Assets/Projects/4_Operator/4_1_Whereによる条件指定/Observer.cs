using R3;
using R3.Triggers;
using UnityEngine;

namespace Projects._4_Operator._4_1_Whereによる条件指定
{
    public class Observer : MonoBehaviour
    {
        [SerializeField] private Operator _target;
        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;

            // GameStateの値がMoveの時にMoveメソッドを実行
            this.FixedUpdateAsObservable()
                // Whereメソッドで条件を指定する
                // 条件がTrueの場合、次のオペレーターに値を流す
                .Where(_ => _target.MoveMode.CurrentValue == EMoveMode.Pingpong)
                .Subscribe(_ => Move()
                ).AddTo(this);

            // GameStateの値がRotateの時にRotateメソッドを実行
            this.FixedUpdateAsObservable()
                // Whereメソッドで条件を指定する
                // 条件がTrueの場合、次のオペレーターに値を流す
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