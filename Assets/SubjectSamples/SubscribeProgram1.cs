using R3;
using UnityEngine;

namespace SubjectSamples
{
    /// <summary>
    /// イベントを購読(Subscribe)するクラス
    /// </summary>
    public class SubscribeProgram1 : MonoBehaviour
    {
        [SerializeField] private SubjectProgram1 _target;

        private void Awake()
        {
            // SubjectProgramのUpdateSubjectを購読する
            _target.UpdateSubject
                .Subscribe(_ =>
                    {
                        // OnNextが呼ばれた時の処理
                        Debug.Log("Update");
                    },
                    onCompleted: result =>
                    {
                        // OnCompleteが呼ばれた時の処理
                        Debug.Log("OnComplete");
                    })
                .AddTo(this); // このコンポーネントが破棄されたら購読を解除
        }
    }
}