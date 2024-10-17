using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題11
{
    public enum EJanken
    {
        Rock,
        Scissors,
        Paper
    }

    public class Challenge : MonoBehaviour, ITextBinder
    {
        private readonly Subject<EJanken> _player = new();
        private readonly Subject<EJanken> _enemy = new();
        private readonly Subject<string> _message = new();

        public ReadOnlyReactiveProperty<string> BindText => _message.ToReadOnlyReactiveProperty();

        private void Start()
        {
            _player.AddTo(this);
            _enemy.AddTo(this);
            _message.AddTo(this);

            // ----------ここに処理追加--------------
            Observable
                .ZipLatest(_player, _enemy)
                .Subscribe(x => Jagged(x[0], x[1]))
                .AddTo(this);
        }
        
        /// <summary>
        ///  じゃんけんの勝敗を判定する
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        private void Jagged(EJanken player, EJanken enemy)
        {
            var result = (3 + enemy - player) % 3;
            _message.OnNext(result switch
            {
                0 => "Draw",
                1 => "Win",
                2 => "Lose",
                _ => "Error"
            });
        }

        // ボタンから呼ばれるメソッド
        public void OnPlayerButtonClicked(int janken) => _player.OnNext((EJanken)janken);
        public void OnEnemyButtonClicked(int janken) => _enemy.OnNext((EJanken)janken);
    }
}