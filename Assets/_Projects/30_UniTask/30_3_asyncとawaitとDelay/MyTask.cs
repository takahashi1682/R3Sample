using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Projects._30_UniTask._30_3_asyncとawaitとDelay
{
    public class MyTask : MonoBehaviour
    {
        // async は非同期処理を行うメソッドに付けるキーワードです
        private async void Start()
        {
            // await で非同期処理が完了するまで待機する
            await UniTask.Delay(TimeSpan.FromSeconds(3)); // 3秒待機する

            Debug.Log("3秒経過しました");
        }
    }
}