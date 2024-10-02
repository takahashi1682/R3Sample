using System;
using Projects.CharacterParameter;
using R3;
using UnityEngine;

namespace Projects._5_Challenge.Challenge1
{
    public class Challenge1 : MonoBehaviour
    {
        [SerializeField] private Health _health;

        public void Start()
        {
            // -- ここに処理を追加してください --
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => _health.Sub(100)).AddTo(this);
        }
    }
}