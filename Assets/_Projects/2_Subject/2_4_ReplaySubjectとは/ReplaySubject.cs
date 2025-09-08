using R3;
using UnityEngine;

namespace _Projects._2_Subject._2_4_ReplaySubjectとは
{
    /// <summary>
    ///  イベントを発行するクラス
    /// </summary>
    public class ReplaySubject : MonoBehaviour
    {
        public ReplaySubject<int> TestSubject { get; } = new();

        private void Start()
        {
            TestSubject.AddTo(this);

            TestSubject.OnNext(0);
            TestSubject.OnNext(1);
            TestSubject.OnNext(2);

            Debug.Log("通知完了");
        }
    }
}