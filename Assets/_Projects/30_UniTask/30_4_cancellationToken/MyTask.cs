using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Projects._30_UniTask._30_4_cancellationToken
{
    public class MyTask : MonoBehaviour
    {
        private async void Start()
        {
            // このオブジェクトが破棄されたらキャンセルを発行するトークン
            var token = destroyCancellationToken;
            
            // cancellationToken: token とすることで、token がキャンセルされたら処理を中断する
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: token); // 3秒待機する

            Debug.Log("3秒経過しました");
        }
    }
}