using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Projects._30_UniTask._30_8_Forgetで非同期処理を実行する
{
    public class MyTask : MonoBehaviour
    {
        // awaitしない場合は、async不要
        private void Start()
        {
            // キャンセルトークンを生成する
            var token = destroyCancellationToken;

            // 一定時間後にログを出力する非同期処理を実行する
            DelayLog(5, "5秒経過しました", token).Forget();
            DelayLog(3, "3秒経過しました", token).Forget();
            DelayLog(1, "1秒経過しました", token).Forget();
            
            Debug.Log("スタート");
        }

        private async UniTask DelayLog(float second, string message, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(second), cancellationToken: token);
            Debug.Log(message);
        }
    }
}