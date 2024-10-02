using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace _4_Operator
{
    // 列挙型定数
    public enum EMoveMode
    {
        None, // 0
        Pingpong, // 1
        Rotate // 2
    }

    public class OperatorProgram1 : MonoBehaviour
    {
        // ゲーム状態
        [SerializeField] private SerializableReactiveProperty<EMoveMode> _moveMode = new(EMoveMode.None);
        public ReadOnlyReactiveProperty<EMoveMode> MoveMode => _moveMode;

        private async void Start()
        {
            // 移動モードをPingpongに変更
            _moveMode.Value = EMoveMode.Pingpong;

            // 処理を10秒停止（次の処理に移る前に10秒待つ）
            await UniTask.Delay(TimeSpan.FromSeconds(10));

            // 移動モードをRotateに変更
            _moveMode.Value = EMoveMode.Rotate;
        }
    }
}