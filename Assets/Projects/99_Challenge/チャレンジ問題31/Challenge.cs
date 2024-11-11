using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題31
{
    public class Challenge : MonoBehaviour
    {
        // 最大時間
        [SerializeField] private float _maxInterval = 3;

        // 現在時間
        private readonly ReactiveProperty<float> _currentTime = new();
        // 開始イベント
        public readonly Subject<Unit> OnStart = new();
        // 完了イベント
        public readonly Subject<Unit> OnComplete = new();

        // 時間の比率を返す
        public ReadOnlyReactiveProperty<float> CurrentRate =>
            _currentTime.Select(x => Mathf.Clamp01(x / _maxInterval)).ToReadOnlyReactiveProperty();

        private bool _isRunning;

        // キャンセル用トークン
        private CancellationTokenSource _cts = new();

        private void Start()
        {
            _currentTime.AddTo(this);
            OnStart.AddTo(this);
            OnComplete.AddTo(this);
        }

        /// <summary>
        /// スタートボタン押下処理
        /// </summary>
        public void OnStartButtonClicked()
        {
            // 既に実行中なら何もしない

            // イベントの開始を通知

            //  キャンセルトークンをリセット

            // タスク実行

            // フラグをリセット
        }

        /// <summary>
        ///  キャンセルボタン押下処理
        /// </summary>
        public void OnCancelButtonClicked()
        {
            // 値をリセット
            // フラグをリセット
            // キャンセルトークンをキャンセル
        }

        private async UniTask Test(CancellationTokenSource token)
        {
            // 一定時間経過するまでループ
            // while ( && !token.IsCancellationRequested)
            {
                // 時間を進める

                // 1フレーム待機
            }

            // イベントの完了を通知
        }
    }
}