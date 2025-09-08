using R3;
using UnityEngine;

namespace _Projects._2_Subject._2_3_Observableの終了とエラーを監視する
{
    /// <summary>
    ///  イベントを監視(Observer)するクラス
    /// </summary>
    public class Observer : MonoBehaviour
    {
        [SerializeField] private Subject _target;

        private void Awake()
        {
            _target.TestSubject
                .Subscribe(x =>
                {
                    // OnNext
                    Debug.Log(x);
                }, onErrorResume: error =>
                {
                    // OnErrorResume
                    Debug.LogError(error);
                }, onCompleted: _ =>
                {
                    // OnCompleted
                    Debug.Log("OnCompleted");
                })
                .AddTo(this);
        }
    }
}