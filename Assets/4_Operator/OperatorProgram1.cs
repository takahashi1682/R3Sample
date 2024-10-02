using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace _4_Operator
{
    public enum EMoveMode { None, Pingpong, Rotate }

    public class OperatorProgram1 : MonoBehaviour
    {
        // ゲーム状態
        [SerializeField] private SerializableReactiveProperty<EMoveMode> _moveMode = new(EMoveMode.None);
        public ReadOnlyReactiveProperty<EMoveMode> MoveMode => _moveMode;

        private async void Start()
        {
            _moveMode.Value = EMoveMode.Pingpong;

            await UniTask.Delay(TimeSpan.FromSeconds(10));

            _moveMode.Value = EMoveMode.Rotate;
        }
    }
}