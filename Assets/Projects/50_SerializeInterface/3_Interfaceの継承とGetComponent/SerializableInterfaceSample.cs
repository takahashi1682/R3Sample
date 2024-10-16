using TNRD;
using UnityEngine;

namespace Projects._50_SerializeInterface._3_Interfaceの継承とGetComponent
{
    public class SerializableInterfaceSample : MonoBehaviour
    {
        [SerializeField] private Bob _bob; // Bobクラスを取得する
        [SerializeField] private SerializableInterface<IHuman> _human; // IHumanのインスタンスを取得する
        [SerializeField] private SerializableInterface<ICharacter> _character; // ICharacterのインスタンスを取得する

        private void Start()
        {
            Debug.Log(_bob.Name);
            Debug.Log(_human.Value.Name);
            Debug.Log(_character.Value.Name);

            // クラスからGetComponentでインターフェイスを取得する
            var human = _bob.GetComponent<IHuman>(); // BobのIHumanを取得
            var character = _bob.GetComponent<ICharacter>(); // BobのICharacterを取得
            Debug.Log(human.Name);
            Debug.Log(character.Name);
        }
    }
}