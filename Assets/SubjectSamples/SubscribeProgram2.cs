using R3;
using UnityEngine;

namespace SubjectSamples
{
    /// <summary>
    /// イベントを購読(Subscribe)するクラス
    /// </summary>
    public class SubscribeProgram2 : MonoBehaviour
    {
        [SerializeField] private SubjectProgram2 _target;

        private void Awake()
        {
            // SubjectProgramのTimerSubjectを購読する
            _target.TimerSubject
                .Subscribe(
                    // onNext発行時に指定された引数はxで取得できる
                    x => Debug.Log($"{x}s"))
                .AddTo(this);
        }
    }
}