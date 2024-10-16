using TNRD;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題51
{
    public class Challenge : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<ICharacter>[] _characters;

        private void Start()
        {
            // Player.csのICharacterのNameをログで表示する
            // Player.csのICharacterのAttack()メソッドを実行する
        }
    }
}