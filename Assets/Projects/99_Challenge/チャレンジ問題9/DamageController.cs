using Projects.CharacterParameter;
using R3.Triggers;
using UnityEngine;
using R3;
using TNRD;

namespace Projects._99_Challenge.チャレンジ問題9
{
    public class DamageController : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<IHealth> _health;

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable().Subscribe(OnHit).AddTo(this);
        }

        private void OnHit(Collider2D coll) => _health.Value.Sub(100);
    }
}