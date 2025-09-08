using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題101
{
    public class BlockFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private float _interval = 4f; // 生成間隔
        [SerializeField] private float _minGapY = 3f; // 最低限の間隔Y
        [SerializeField] private float _stageHeight = 5f; // ステージの高さ
        [SerializeField] private float _blockMoveSpeed = 1f; // ブロックの移動速度
        [SerializeField] private Vector3 _blockSpawnPos; // ブロックの生成位置X
        [SerializeField] private float _blockDestroyX = -10f; // ブロックの破棄位置X

        private readonly List<Transform> _blocks = new(); // 全てのブロックリスト

        private void Start()
        {
            // ブロック生成開始
            BlockSpawn(destroyCancellationToken).Forget();
        }

        private void FixedUpdate()
        {
            // リストに登録されている全てのブロックを左に移動する
            foreach (var block in _blocks.ToList())
            {
                // ブロックを左に移動
                //              += Vector3.left * (_blockMoveSpeed * Time.deltaTime);

                // ブロックのX座標が_blockDestroyX未満になったら破棄する
                // if (                             )
                // {
                //     // リストからブロックを削除(1行)
                //     
                //     // ブロックオブジェクトを破棄する(1行)
                // }
            }
        }

        private async UniTask BlockSpawn(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // 上下のブロックを生成する
                var block1 = Instantiate(_blockPrefab).transform;
                var block2 = Instantiate(_blockPrefab).transform;

                // 生成したブロックをリストに登録(2行)


                // 上のブロックの高さをランダムに設定
                var blockSize1 = block1.localScale;
                blockSize1.y = UnityEngine.Random.Range(0, _stageHeight - _minGapY);
                block1.localScale = blockSize1;
                // block1.position = _blockSpawnPos - スケールYの半分の位置

                // 下のブロックの位置を調整
                var blockSize2 = block2.localScale;
                blockSize2.y = _stageHeight - blockSize1.y - _minGapY;
                block2.localScale = blockSize2;
                // block2.position = _blockSpawnPos から次の高さを引く→  _minGapY + ブロック1の高さ + ブロック2のスケールYの半分の位置
                
                await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: token);
            }
        }
    }
}