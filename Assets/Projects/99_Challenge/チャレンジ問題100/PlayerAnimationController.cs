using UnityEngine;
using R3;

namespace Projects._99_Challenge.チャレンジ問題100
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private SpriteAnimation _animation;
        [SerializeField] private int _defaultFlameRate = 10; // デフォルトのフレームレート
        [SerializeField] private int _flayFlameRate = 40; // 飛ぶ時のフレームレート

        private void Start()
        {
            // ①PlayerMoverのIsFlayの値が変更されたら、SpriteAnimationのFlameRateを変更する(1行程度)
            
        }
    }
}