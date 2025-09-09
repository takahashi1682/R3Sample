using System;
using UnityEngine;

namespace _Projects._51_Enum._2_EnumのFlags属性について
{
    [Flags]
    public enum EEventFlags
    {
        IsDoorOpen = 1 << 0,
        IsDoorLocked = 1 << 1,
        IsKeyObtained = 1 << 2,
        IsTreasureFound = 1 << 3,
        IsBossDefeated = 1 << 4,
        IsPuzzleSolved = 1 << 5,
        IsItemUsed = 1 << 6,
        IsTrapTriggered = 1 << 7,
        IsSecretFound = 1 << 8,
        IsQuestStarted = 1 << 9,
        IsQuestClear = 1 << 10,
        IsQuestFailed = 1 << 11,
        IsSavePoint = 1 << 12,
        IsGameOver = 1 << 13,
        IsLevelUp = 1 << 14,
        IsNpcTalked = 1 << 15,
        IsNpcJoined = 1 << 16,
        IsNpcLeft = 1 << 17,
        IsSkillLearned = 1 << 18,
        IsShopVisited = 1 << 19,
        IsItemBought = 1 << 20,
        IsItemSold = 1 << 21,
        IsAchievement = 1 << 22,
        IsStageClear = 1 << 23,
        IsDamageTaken = 1 << 24,
        IsHealUsed = 1 << 25,
        IsBuffActive = 1 << 26,
        IsDebuffActive = 1 << 27,
        IsStealth = 1 << 28,
        IsCutsceneSeen = 1 << 29,
        IsEndingSeen = 1 << 30,
        IsSecretMode = 1 << 31 // 32個目 (符号ビット)
    }

    public class EnumSample : MonoBehaviour
    {
        // UnityのInspectorから複数フラグを指定できる
        [SerializeField] private EEventFlags _eventFlags;

        private void Start()
        {
            // 現在のフラグ状態を int 値として出力
            int flagValue = (int)_eventFlags;
            Debug.Log($"現在のフラグ値 (int): {flagValue}");

            // 設定されているフラグをすべて列挙して出力
            foreach (EEventFlags flag in Enum.GetValues(typeof(EEventFlags)))
            {
                // if (flag == EEventFlags.None) continue;

                if (_eventFlags.HasFlag(flag))
                {
                    Debug.Log($"ON: {flag} ({(int)flag})");
                }
            }
        }
    }
}