using UnityEngine;

namespace Projects._50_SerializableInterface._0_概要
{
    public class Human : AbstractHuman, ICharacter
    {
        [SerializeField] private string _age;
        public string Name => _age;
    }
}