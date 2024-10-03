using R3;
using UnityEngine;

namespace Projects._2_Subject._2_2_データを監視する
{
    /// <summary>
    ///  イベントを監視(Observer)するクラス
    /// </summary>
    public class Observer : MonoBehaviour
    {
        [SerializeField] private Subject _target;

        private void Awake()
        {
            // UpdateSubjectを購読する
            _target.UpdateSubject
                .Subscribe(x =>
                {
                    // OnNextが呼ばれた時の処理
                    Debug.Log($"frame: {x}");
                })
                .AddTo(this); // このコンポーネントが破棄されたら購読を解除
        }
    }
}