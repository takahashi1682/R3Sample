using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題30
{
    public class BomController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _target;
        [SerializeField] private Sprite _explosionSprite;
        [SerializeField] private float _explosionTime = 3f;

        /// <summary>
        /// Startメソッドを書き換えてください
        /// </summary>
        private void Start()
        {
        }
    }
}