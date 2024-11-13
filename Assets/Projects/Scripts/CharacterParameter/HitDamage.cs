using R3;
using TNRD;
using UnityEngine;

namespace Projects.CharacterParameter
{
    public interface IHitDamageObservable
    {
        Observable<float> DamageSubject { get; }
    }

    public interface IHitDamage
    {
        void ApplyDamage(float damage);
    }

    public class HitDamage : MonoBehaviour, IHitDamage, IHitDamageObservable
    {
        [SerializeField] private SerializableInterface<IHealth> _health;

        // ダメージを受けたときのイベント
        private readonly Subject<float> _damageSubject = new();
        public Observable<float> DamageSubject => _damageSubject;

        private void Start()
        {
            _damageSubject.AddTo(this);
        }

        /// <summary>
        ///  ダメージを受ける
        /// </summary>
        /// <param name="damage"></param>
        public void ApplyDamage(float damage)
        {
            _health.Value.Sub(damage);
            _damageSubject.OnNext(damage);
        }
    }
}