using R3;
using UnityEngine;

namespace _3_ReactiveProperty
{
    /// <summary>
    /// イベントを購読(Subscribe)するクラス
    /// </summary>
    public class SubscribeProgram1 : MonoBehaviour
    {
        [SerializeField] private ReactivePropertyProgram1 _program1;

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