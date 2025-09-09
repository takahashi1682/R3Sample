using R3;
using UnityEngine;

namespace _Projects._4_Operator._4_1_Whereによる条件指定
{
    // 列挙型定数
    public enum EMoveMode
    {
        None = 0,
        Pingpong = 1,
        Rotate = 2
    }

    public class SampleOperator : MonoBehaviour
    {
        // ゲーム状態
        [SerializeField] private SerializableReactiveProperty<EMoveMode> _moveMode = new(EMoveMode.None);
        public ReadOnlyReactiveProperty<EMoveMode> MoveMode => _moveMode;
    }
}