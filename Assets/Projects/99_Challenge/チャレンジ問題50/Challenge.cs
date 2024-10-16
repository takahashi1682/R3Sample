using TNRD;
using UnityEngine;

namespace Projects._99_Challenge.チャレンジ問題50
{
    public interface ICharacterStatus
    {
        string Name { get; }
        int Power { get; }
        int Defense { get; }
    }

    public class Challenge : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<ICharacterStatus>[] _status;

        private void Start()
        {
            foreach (var status in _status)
            {
                Debug.Log("Name: " + status.Value.Name);
                Debug.Log("Power: " + status.Value.Power);
                Debug.Log("Defense: " + status.Value.Defense);
            }
        }
    }
}