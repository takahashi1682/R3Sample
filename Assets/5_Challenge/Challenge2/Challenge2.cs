using System;
using Common;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _5_Challenge.Challenge2
{
    public class Challenge2 : MonoBehaviour, ITextBinder, ISliderBinder
    {
        [SerializeField] private SerializableReactiveProperty<int> _health = new(1000);
        [SerializeField] private SerializableReactiveProperty<int> _maxHealth = new(1000);

        private readonly Subject<int> _damage = new();
        private readonly Subject<int> _recovery = new();

        // 残りHPをUIに表示する
        public Observable<string> TextValue =>
            Observable.CombineLatest(_health, _maxHealth).Select(_ => $"{_health.Value} / {_maxHealth.Value}");
        public Observable<float> SliderValue =>
            Observable.CombineLatest(_health, _maxHealth).Select(_ => (float)_health.Value / _maxHealth.Value);

        public void Start()
        {
            _health.AddTo(this);
            _maxHealth.AddTo(this);
            _damage.AddTo(this);
            _recovery.AddTo(this);
        }

        public void SetClampHealth(int value)
        {
            _health.Value = Mathf.Clamp(value, 0, _maxHealth.Value);
        }

        public void SetMaxHealth(int value)
        {
            _maxHealth.Value = value;
            if (_health.Value > _maxHealth.Value)
            {
                _health.Value = _maxHealth.Value;
            }
        }

        public void OnDamage(int damage) => _damage.OnNext(damage);
        public void OnRecovery(int recovery) => _recovery.OnNext(recovery);
    }
}