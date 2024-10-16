using UnityEngine;

namespace Projects._50_SerializeInterface._3_Interfaceの継承とGetComponent
{
    public class Bob : MonoBehaviour, IHuman
    {
        public string Name => "Bob";
        public int Age => 20;
    }
}