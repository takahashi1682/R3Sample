using R3;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題4
{
    public class Challenge : MonoBehaviour
    {
        private void Start()
        {
            Observable.Range(1, 10)
                .Subscribe(x => Debug.Log(x))
                .AddTo(this);
        }
    }
}