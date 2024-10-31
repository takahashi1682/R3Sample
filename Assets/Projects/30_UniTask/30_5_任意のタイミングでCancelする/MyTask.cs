using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Projects._30_UniTask._30_5_任意のタイミングでCancelする
{
    public class MyTask : MonoBehaviour
    {
        // キャンセルトークン
        private readonly CancellationTokenSource _token = new();

        private async void Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: _token.Token); // 3秒待機する

            Debug.Log("3秒経過しました");
        }

        public void OnCancel()
        {
            // トークンをキャンセルする
            _token.Cancel();
        }
    }
}