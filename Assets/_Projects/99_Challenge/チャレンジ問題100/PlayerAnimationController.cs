using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題100
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private SpriteAnimation _animation;
        [SerializeField] private int _defaultFrameRate = 10; // デフォルトのフレームレート
        [SerializeField] private int _flyingFrameRate = 40; // 飛ぶ時のフレームレート

        private void Start()
        {
            // ①PlayerMoverのIsFlyingの値が変更されたら、SpriteAnimationのFrameRateを変更する(1行程度)
        }
    }
}