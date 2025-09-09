using System;
using UnityEngine;

namespace _Projects._51_Enum._0_概要
{
    public class SampleEnum : MonoBehaviour
    {
        [SerializeField] private int _armyType;

        private void Start()
        {
            switch (_armyType)
            {
                case 0:
                    Debug.Log("Player");
                    break;
                case 1:
                    Debug.Log("Enemy");
                    break;
                case 2:
                    Debug.Log("Object");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}