using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題32
{
    public class Challenge : MonoBehaviour
    {
        [SerializeField] private GameObject _blockPrefab; // 生成するブロックのプレハブ
        [SerializeField] private float _spawnHeight = -3; // 生成位置の高さ
        [SerializeField] private int _width = 10; // 横に生成するブロックの数
        [SerializeField] private int _height = 3; // 縦に生成するブロックの数
        [SerializeField] private float _spawnInterval = 0.1f; // ブロック生成間隔

        private void Start()
        {
            // SpawnBlocks().Forget();
        }

        // SpawnBlocksメソッドを実装してください
    }
}