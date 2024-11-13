using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題100
{
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _target;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private int _flameRate = 10;
        private int _delayMilliSeconds;
        private int _index;

        private void Start()
        {
            SetFlameRate(_flameRate);
            Animation(destroyCancellationToken).Forget();
        }

        private async UniTask Animation(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // ①画像を切り替える

                await UniTask.Delay(_delayMilliSeconds, cancellationToken: token);
            }
        }

        public void SetFlameRate(int flameRate)
        {
            _flameRate = flameRate;
            _delayMilliSeconds = 1000 / flameRate;
        }
    }
}