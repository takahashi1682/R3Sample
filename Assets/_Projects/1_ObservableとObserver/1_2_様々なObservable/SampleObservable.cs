using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace _Projects._1_ObservableとObserver._1_2_様々なObservable
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class SampleObservable : MonoBehaviour
    {
        private void Awake()
        {
            // 5から始まる3つの数値を通知する
            Observable.Range(5, 3)
                .Subscribe(x => Debug.Log($"Range: {x}"))
                .AddTo(this);

            // Helloを10回通知する
            Observable.Repeat("Hello", 10)
                .Subscribe(x => Debug.Log($"Repeat: {x}"))
                .AddTo(this);

            // 3秒後に処理を実行
            Observable
                .Timer(TimeSpan.FromSeconds(3))
                .Subscribe(_ => Debug.Log("3秒経過しました"))
                .AddTo(this);

            // 1秒毎に処理を実行（繰り返し）※0秒後に開始
            Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .Subscribe(_ => Debug.Log("Timer: " + Time.time))
                .AddTo(this);

            // 1秒毎に処理を実行（繰り返し）※1秒後に開始
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