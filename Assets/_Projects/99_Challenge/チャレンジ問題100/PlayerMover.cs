using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Projects._99_Challenge.チャレンジ問題100
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private SpriteAnimation _animation;
        [SerializeField] private float _upSpeed = 2;
        [SerializeField] private float _downSpeed = -3;
        [SerializeField] private float _moveRange = 2;

        private readonly ReactiveProperty<bool> _isFlying = new();
        private Keyboard _keyboard;
        private Vector3 _startPosition;
        private float _y;

        public ReadOnlyReactiveProperty<bool> IsFlying => _isFlying;

        private void Start()
        {
            _keyboard = Keyboard.current;
            _startPosition = transform.position;
        }

        private void Update()
        {
            // 入力を受け取る
            _isFlying.Value = _keyboard.spaceKey.isPressed;
        }

        private void FixedUpdate()
        {
            // 上昇と下降
            _y += _isFlying.Value
                ? Time.deltaTime * _upSpeed
                : Time.deltaTime * _downSpeed;

            // ①上下の移動範囲を制限(1行)

            // ②_startPositionを基準に_y分移動する(1行)
            
        }
    }
}