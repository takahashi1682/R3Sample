using R3;
using UnityEngine;

namespace Projects._4_Operator._4_2_Selectによるデータの変換
{
    public class Operator : MonoBehaviour
    {
        public readonly Subject<int> UpdateSubject = new();
        private int _frameCount;

        public void Awake()
        {
            UpdateSubject.AddTo(this);
        }

        private void Update()
        {
            UpdateSubject.OnNext(++_frameCount);
        }
    }
}