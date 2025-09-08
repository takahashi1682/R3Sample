using R3;
using R3.Triggers;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題8
{
    public class Challenge : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private Transform _fire;

        private Vector3 _startPos;
        private Vector3 _endPos;

        // --- ここに処理を追加してください ---

        private void Start()
        {
            _startPos = _fire.position;
            _endPos = _startPos + Vector3.right * 2;

            // 炎を左右に移動させる
            this.FixedUpdateAsObservable().Subscribe(_ =>
                _fire.position = Vector3.Lerp(_startPos, _endPos, Mathf.PingPong(Time.time, 1))).AddTo(this);

            // --- ここに処理を追加してください ---
        }
    }
}