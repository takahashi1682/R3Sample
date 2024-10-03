using R3;
using UnityEngine;

namespace Projects._4_Operator._4_1_Whereによる条件指定
{
    // 列挙型定数
    public enum EMoveMode
    {
        None = 0,
        Pingpong = 1,
        Rotate = 2
    }

    public class Operator : MonoBehaviour
    {
        // ゲーム状態
        [SerializeField] private SerializableReactiveProperty<EMoveMode> _moveMode = new(EMoveMode.None);
        public ReadOnlyReactiveProperty<EMoveMode> MoveMode => _moveMode;
    }
}