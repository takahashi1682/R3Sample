using R3;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題12
{
    /// <summary>
    /// 10_Observableの合成 の復習課題です。
    /// プレイヤーと敵、両方の準備が完了したタイミングで"Battle Start!"を出力してください。
    /// </summary>
    public class Challenge : MonoBehaviour
    {
        private readonly Subject<bool> _playerReady = new();
        private readonly Subject<bool> _enemyReady = new();

        private void Start()
        {
            _playerReady.AddTo(this);
            _enemyReady.AddTo(this);

            // -- ここに処理を追加してください --
            // ヒント: Observable.CombineLatestで2つのSubjectを合成し、
            //        両方がtrueになった時だけログを出力する（Whereで絞り込む）
        }

        [ContextMenu("プレイヤー準備完了")]
        public void OnPlayerReady() => _playerReady.OnNext(true);

        [ContextMenu("敵準備完了")]
        public void OnEnemyReady() => _enemyReady.OnNext(true);
    }
}
