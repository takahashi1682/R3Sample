using R3;
using UnityEngine;

namespace _Projects._3_ReactiveProperty._3_1_値の更新を監視する
{
    /// <summary>
    /// イベントを監視(Observer)するクラス
    /// </summary>
    public class SampleObserver : MonoBehaviour
    {
        [SerializeField] private ReactiveProperty _program1;

        private void Awake()
        {
            _program1.Count1.Subscribe(count =>
            {
                // カウント1の変更を購読
                Debug.Log($"Count1: {count}");
            }).AddTo(this);

            _program1.Count2.Subscribe(count =>
            {
                // カウント2の変更を購読
                Debug.Log($"Count2: {count}");
            }).AddTo(this);
        }
    }
}