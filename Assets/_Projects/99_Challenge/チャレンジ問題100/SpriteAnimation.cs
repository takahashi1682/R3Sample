using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題100
{
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _target;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private int _frameRate = 10;
        private int _delayMilliSeconds;
        private int _index;

        private void Start()
        {
            SetFrameRate(_frameRate);
            Animation(destroyCancellationToken).Forget();
        }

        private async UniTask Animation(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // ①画像を切り替える(1行)

                await UniTask.Delay(_delayMilliSeconds, cancellationToken: token);
            }
        }

        public void SetFrameRate(int frameRate)
        {
            _frameRate = frameRate;
            _delayMilliSeconds = 1000 / frameRate;
        }
    }
}