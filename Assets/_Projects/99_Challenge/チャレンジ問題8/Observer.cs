using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題8
{
    public class Observer : MonoBehaviour
    {
        [SerializeField] private Challenge _challenge;

        private void Start()
        {
            // --- ここに処理を追加してください ---
        }

        private void OnHit(Collider2D coll) => Debug.Log("Hit: " + coll.name);
    }
}