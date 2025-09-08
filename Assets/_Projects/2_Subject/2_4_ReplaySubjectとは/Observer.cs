using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace _Projects._2_Subject._2_4_ReplaySubjectとは
{
    /// <summary>
    ///  イベントを監視(Observer)するクラス
    /// </summary>
    public class Observer : MonoBehaviour
    {
        [SerializeField] private ReplaySubject _target;

        private async void Awake()
        {
            // 3秒待機
            await UniTask.Delay(TimeSpan.FromSeconds(3));

            // イベントを監視
            _target.TestSubject.Subscribe(x => Debug.Log(x)).AddTo(this);
        }
    }
}