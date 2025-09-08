using _Projects.Scripts.CharacterParameter;
using R3;
using R3.Triggers;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題101
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private HitDamage _hitDamage;
        [SerializeField] private string _blockLayerName = "Block";
        [SerializeField] private float _blockHitDamage = 100;

        private void Start()
        {
            // ブロックに当たったらダメージを受ける
            this.OnTriggerEnter2DAsObservable().Subscribe(x =>
            {
                if (x.gameObject.layer == LayerMask.NameToLayer(_blockLayerName))
                {
                    _health.Sub(_blockHitDamage);
                }
            });
        }
    }
}