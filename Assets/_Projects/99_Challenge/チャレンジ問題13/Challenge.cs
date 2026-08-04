using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題13
{
    /// <summary>
    /// 30_UniTask の復習課題です。
    /// ランダムで成功/失敗する非同期処理を、失敗した場合は最大3回までリトライしてください。
    /// 3回失敗しても成功しなければ「失敗しました」とログを出力してください。
    /// </summary>
    public class Challenge : MonoBehaviour
    {
        [SerializeField] private int _maxRetryCount = 3;

        [ContextMenu("実行")]
        public void Execute()
        {
            RunWithRetry(destroyCancellationToken).Forget();
        }

        /// <summary>
        /// 成功するまで（最大_maxRetryCount回まで）リトライする
        /// </summary>
        private async UniTask RunWithRetry(CancellationToken token)
        {
            // -- ここに処理を追加してください --
            // ヒント: for文で_maxRetryCount回ループしてTryProcessAsyncを呼び出す
            //        成功したらreturn、失敗したら次のループへ
            //        ループを抜けても成功していなければ「失敗しました」とログを出力する
        }

        /// <summary>
        /// 1秒待機した後、50%の確率で成功/失敗する処理
        /// </summary>
        private async UniTask<bool> TryProcessAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            var isSuccess = UnityEngine.Random.value >= 0.5f;
            Debug.Log(isSuccess ? "成功" : "失敗...リトライします");
            return isSuccess;
        }
    }
}
