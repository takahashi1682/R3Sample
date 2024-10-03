using R3;
using UnityEngine;

namespace Projects._2_Subject._2_3_Observableの終了とエラーを監視する
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
                }, ex =>
                {
                    // OnErrorResume
                    Debug.LogError(ex);
                }, _ =>
                {
                    // OnCompleted
                    Debug.Log("OnCompleted");
                })
                .AddTo(this);
        }
    }
}