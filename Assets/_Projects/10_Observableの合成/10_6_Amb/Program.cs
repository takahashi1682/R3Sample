using R3;
using UnityEngine;

namespace _Projects._10_Observableの合成._10_6_Amb
{
    public class Program : MonoBehaviour
    {
        // 合成するObservable
        private readonly Subject<string> _subject1 = new();
        private readonly Subject<string> _subject2 = new();

        private void Start()
        {
            _subject1.AddTo(this);
            _subject2.AddTo(this);

            // 2つのSubjectを合成して購読する
            var subject = Observable
                .Amb(
                    _subject1,
                    _subject2
                ).Subscribe(
                    x => Debug.Log("next." + x),
                    onCompleted: _ => Debug.Log("completed.")
                ).AddTo(this);

            // それぞれのSubjectにデータを流す
            _subject2.OnNext("Subject2"); // next. 2
            _subject1.OnNext("Subject1"); // 流れない

            _subject2.OnNext("Subject2"); // next. 3
            _subject1.OnNext("Subject1"); // 流れない

            _subject2.OnCompleted(); // completed.
            _subject1.OnCompleted();
        }
    }
}