using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace ObservableProperty
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class SubjectProgram2 : MonoBehaviour
    {
        private void Awake()
        {
            // 3秒後に処理を実行
            Observable
                .Timer(TimeSpan.FromSeconds(3))
                .Subscribe(_ => Debug.Log("3秒経過しました"))
                .AddTo(this);

            // 1秒毎に処理を実行（繰り返し）
            Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .Subscribe(_ => Debug.Log("Timer: " + Time.time))
                .AddTo(this);

            // 1秒毎に処理を実行（繰り返し）
            Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => Debug.Log("Interval: " + Time.time))
                .AddTo(this);

            // 独自のイベントを発行する
            // 1秒おきにメッセージを通知する
            Observable.Create<string>(async (observer, ct) =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct); // 1秒待機
                    observer.OnNext("Create: ゲーム開始"); // 通知

                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct); // 1秒待機
                    observer.OnNext("Create: 敵出現"); // 通知

                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct); // 1秒待機
                    observer.OnNext("Create: タイムオーバー"); // 通知

                    observer.OnCompleted(); // 完了
                }).Subscribe(Debug.Log)
                .AddTo(this);
        }
    }
}