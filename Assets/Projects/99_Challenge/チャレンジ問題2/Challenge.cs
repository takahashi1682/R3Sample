using Projects.CharacterParameter;
using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題2
{
    public class Challenge : MonoBehaviour, ITextBinder, ISliderBinder
    {
        [SerializeField] private Health _health;

        private readonly Subject<int> _damage = new();
        private readonly Subject<int> _recovery = new();

        // UI表示用
        public ReadOnlyReactiveProperty<string> Text =>
            Observable.CombineLatest(_health.Current, _health.Max).Select(p => $"{p[0]} / {p[1]}")
                .ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<float> SliderValue => _health.Rate.ToReadOnlyReactiveProperty();

        private void Start()
        {
            _damage.AddTo(this);
            _recovery.AddTo(this);
            // -- ここに処理を追加してください --
        }

        /// <summary>
        /// ダメージボタンの押下処理
        /// </summary>
        /// <param name="value"></param>
        public void OnDamageButtonClicked(int value) => _damage.OnNext(value);

        /// <summary>
        /// 回復ボタンの押下処理
        /// </summary>
        /// <param name="value"></param>
        public void OnRecoveryButtonClicked(int value) => _recovery.OnNext(value);
    }
}