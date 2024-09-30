using R3;
using UnityEngine;

namespace SubjectSamples
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class SubjectProgram4 : MonoBehaviour
    {
        // Subscribe()時にそれまでにOnNext()でされた値をすべて通知するSubject
        public ReplaySubject<string> MessageSubject { get; } = new();

        private void Start()
        {
            // このコンポーネントが破棄されたらイベントの発行を終了
            MessageSubject.AddTo(this);

            MessageSubject.OnNext("イベント1を通知");
            Debug.Log("SubjectProgram: イベント1を通知");

            MessageSubject.OnNext("イベント2を通知");
            Debug.Log("SubjectProgram: イベント2を通知");
            
            MessageSubject.OnNext("イベント3を通知");
            Debug.Log("SubjectProgram: イベント3を通知");
        }
    }
}