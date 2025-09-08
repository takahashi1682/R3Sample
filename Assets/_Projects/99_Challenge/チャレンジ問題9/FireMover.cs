using R3;
using R3.Triggers;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題9
{
    public class FireMover : MonoBehaviour
    {
        private Vector3 _startPos;
        private Vector3 _endPos;

        private void Start()
        {
            _startPos = transform.position;
            _endPos = _startPos + Vector3.right * 2;

            // 炎を左右に移動させる
            this.FixedUpdateAsObservable().Subscribe(_ =>
                transform.position = Vector3.Lerp(_startPos, _endPos, Mathf.PingPong(Time.time, 1))).AddTo(this);
        }
    }
}