using System;
using UnityEngine;

namespace _Projects._51_Enum._1_列挙型定数
{
    public enum ArmyType
    {
        Player = 0, // 数字は省略可能
        Enemy = 1,
        Object = 2
    }

    public class EnumSample : MonoBehaviour
    {
        [SerializeField] private ArmyType _armyType;

        private void Start()
        {
            switch (_armyType)
            {
                case ArmyType.Player:
                    Debug.Log("Player");
                    break;
                case ArmyType.Enemy:
                    Debug.Log("Enemy");
                    break;
                case ArmyType.Object:
                    Debug.Log("Object");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}