using _Projects.Scripts.CharacterParameter;
using R3;
using R3.Triggers;
using TNRD;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題9
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