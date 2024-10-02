using R3;
using UnityEngine;

namespace _1_SubjectSamples
{
    /// <summary>
    /// イベントを購読(Subscribe)するクラス
    /// </summary>
    public class SubscribeProgram3 : MonoBehaviour
    {
        [SerializeField] private SubjectProgram3 _target;

        private void Awake()
        {
            // SubjectProgramのTimerSubjectを購読する
            _target.TimerSubject
                .Subscribe(
                    x => Debug.Log($"{x}s"),
                    onCompleted: _ => Debug.Log("OnCompleted"),

                    // onErrorResume: x => Debug.LogError(x)) // 省略前
                    // 呼び出すメソッドが一つで引数も一つの場合以下のように省略できる
                    onErrorResume: Debug.LogError
                )
                .AddTo(this);
        }
    }
}