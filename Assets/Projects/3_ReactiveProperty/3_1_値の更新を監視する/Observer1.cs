using R3;
using UnityEngine;

namespace Projects._3_ReactiveProperty._3_1_値の更新を監視する
{
    /// <summary>
    /// イベントを監視(Observer)するクラス
    /// </summary>
    public class Observer1 : MonoBehaviour
    {
        [SerializeField] private ReactiveProperty1 _program1;

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