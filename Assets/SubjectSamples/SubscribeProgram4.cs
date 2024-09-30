using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace SubjectSamples
{
    /// <summary>
    /// イベントを購読(Subscribe)するクラス
    /// </summary>
    public class SubscribeProgram4 : MonoBehaviour
    {
        [SerializeField] private SubjectProgram4 _target;

        private async void Awake()
        {
            // 1秒待機する
            await UniTask.Delay(TimeSpan.FromSeconds(3));

            Debug.Log("3秒遅れてからイベントを購読する");

            _target.MessageSubject
                .Subscribe(onNext: Debug.Log)
                .AddTo(this);
        }
    }
}