using UnityEngine;

namespace _Projects._50_SerializableInterface._0_概要
{
    public class Robot : AbstractRobot, ICharacter
    {
        [SerializeField] private string _name;
        public string Name => _name;
    }
}