using R3;
using UnityEngine;

namespace Projects._10_Observableの合成._10_2_Concat
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
                .Concat(
                    _subject1, // 順序1
                    _subject2 // 順序2(1が完了した後に流れる)
                ).Subscribe(
                    onNext: x => Debug.Log($"next: {x}"),
                    onCompleted: _ => Debug.Log("completed.")
                ).AddTo(this);

            // それぞれのSubjectにデータを流す
            _subject1.OnNext("Subject1");
            _subject2.OnNext("Subject2"); // 流れない
            _subject1.OnCompleted(); // この時点でsubject2が購読される
            
            _subject1.OnNext("Subject1"); // 流れない
            _subject2.OnNext("Subject2");
            _subject2.OnCompleted();
        }
    }
}