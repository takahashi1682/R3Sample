using Projects.CharacterParameter;
using Projects.Viewer;
using R3;
using UnityEngine;

namespace Projects._5_Challenge.Challenge2
{
    public class Challenge2 : MonoBehaviour, ITextBinder, IBinderRate
    {
        [SerializeField] private Health _health;

        private readonly Subject<int> _damage = new();
        private readonly Subject<int> _recovery = new();

        // UI表示用
        public ReadOnlyReactiveProperty<string> Text =>
            Observable.CombineLatest(_health.Current, _health.Max).Select(p => $"{p[0]} / {p[1]}")
                .ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<float> Rate => _health.Rate.ToReadOnlyReactiveProperty();

        private void Start()
        {
            _damage.AddTo(this);
            _recovery.AddTo(this);
            // -- ここに処理を追加してください --

        }

        public void OnDamage(int value) => _damage.OnNext(value);
        public void OnRecovery(int value) => _recovery.OnNext(value);
    }
}