using System;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題14
{
    /// <summary>
    /// 51_Enum の復習課題です。
    /// [Flags]属性を使って、複数の状態異常を同時に管理できるようにしてください。
    /// </summary>
    [Flags]
    public enum EStatusEffect
    {
        None = 0,
        Poison = 1 << 0, // 毒
        Stun = 1 << 1, // 気絶
        Silence = 1 << 2 // 沈黙
    }

    public class Challenge : MonoBehaviour
    {
        [SerializeField] private EStatusEffect _currentStatus = EStatusEffect.None;

        /// <summary>
        /// 状態異常を付与する
        /// </summary>
        [ContextMenu("毒を付与")]
        public void AddPoison() => AddStatus(EStatusEffect.Poison);

        [ContextMenu("気絶を付与")]
        public void AddStun() => AddStatus(EStatusEffect.Stun);

        [ContextMenu("状態を表示")]
        public void ShowStatus() => Debug.Log($"現在の状態: {_currentStatus}");

        private void AddStatus(EStatusEffect status)
        {
            // -- ここに処理を追加してください --
            // ヒント: OR演算子(|)を使って_currentStatusにstatusを追加する
        }

        private void RemoveStatus(EStatusEffect status)
        {
            // -- ここに処理を追加してください --
            // ヒント: AND演算子(&)とビット反転(~)を使って_currentStatusからstatusを取り除く
        }

        private bool HasStatus(EStatusEffect status)
        {
            // -- ここに処理を追加してください --
            // ヒント: HasFlagメソッドを使う
            return false;
        }
    }
}
