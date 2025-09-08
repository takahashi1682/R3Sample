using R3;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題60
{
    public class AutoMover : MonoBehaviour
    {
        [SerializeField] private Stamina _stamina;
        [SerializeField] private float _moveSpeed = 1.0f; // 通常速度
        [SerializeField] private float _runningSpeed = 2.0f; // 走る速度

        private float _time;
        private Vector3 _startPos;
        private Vector3 _endPos;

        private float _currentSpeed; // 現在の速度
        private readonly ReactiveProperty<bool> _isRunning = new(false); // 走るかどうか

        private void Start()
        {
            _startPos = transform.position;
            _endPos = _startPos + Vector3.right * 2;

            _isRunning.AddTo(this);
            // スタミナが0以下になったら走るのをやめる(1行程度)
            
            // 走るかどうかが変わったら速度を変更する(1行程度)
            
        }

        private void Update()
        {
            // 走るかどうか
            // _isRunning.Value = スタミナが0より多いなら  && Keyboard.current.leftShiftKey.isPressed;

            // スタミナの更新
            if (_isRunning.Value)
            {
               // スタミナの減算　Time.deltaTimeを使う
            }
            else
            {
                // スタミナの回復　Time.deltaTimeを使う
            }
        }

        private void FixedUpdate()
        {
            // 移動
            _time += Time.deltaTime * _currentSpeed;
            transform.position = Vector3.Lerp(_startPos, _endPos, Mathf.PingPong(_time, 1));
        }
    }
}