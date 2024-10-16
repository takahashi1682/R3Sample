using UnityEngine;

namespace Projects._50_SerializeInterface._0_概要
{
    public class Robot : AbstractRobot, ICharacter
    {
        [SerializeField] private string _name;
        public string Name => _name;
    }
}