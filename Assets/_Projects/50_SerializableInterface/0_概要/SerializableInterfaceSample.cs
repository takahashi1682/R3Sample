using TNRD;
using UnityEngine;

namespace _Projects._50_SerializableInterface._0_概要
{
    // インターフェイス（機能）
    public interface ICharacter
    {
        string Name { get; }
    }

    public class SerializableInterfaceSample : MonoBehaviour
    {
        // インターフェイスをシリアライズするためのクラス(Inspectorから設定可能)
        // ICharacterを継承しているクラスなら何でも設定可能!!
        [SerializeField] private SerializableInterface<ICharacter>[] _characters;

        private void Start()
        {
            foreach (var character in _characters)
            {
                Debug.Log(character.Value.Name);
            }
        }
    }
}