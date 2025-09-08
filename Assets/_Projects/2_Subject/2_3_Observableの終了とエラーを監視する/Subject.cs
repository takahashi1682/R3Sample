using System;
using R3;
using UnityEngine;

namespace _Projects._2_Subject._2_3_Observableの終了とエラーを監視する
{
    /// <summary>
    ///  イベントを発行するクラス
    /// </summary>
    public class Subject : MonoBehaviour
    {
        public Subject<int> TestSubject { get; } = new();

        private void Start()
        {
            TestSubject.AddTo(this);

            TestSubject.OnNext(0);
            TestSubject.OnNext(1);

            // エラーを発行
            TestSubject.OnErrorResume(new Exception("エラー発生"));

            // イベントの発行を終了(破棄)
            TestSubject.OnCompleted();
        }
    }
}