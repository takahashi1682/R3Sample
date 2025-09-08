using System;

namespace _Projects._51_Enum._2_概要3
{
    [Flags]
    public enum EActionStatus
    {
        IsGround = 1 << 0,
        IsJump = 1 << 1,
        IsAttack = 1 << 2,
        IsDamage = 1 << 3,
        IsDead = 1 << 4,
    }

    public class ActionStatus
    {
        // [field: SerializeField] public ReactiveProperty;
    }
}