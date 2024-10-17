using R3;
using UnityEngine;

namespace Projects._10_Observableの合成._10_1_Merge
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
                .Merge(_subject1, _subject2)
                .Subscribe(
                    onNext: x => Debug.Log($"next: {x}"),
                    onCompleted: _ => Debug.Log("completed.")
                ).AddTo(this);

            // それぞれのSubjectにデータを流す
            _subject1.OnNext("Subject1に流した値"); // next: 0.
            _subject2.OnNext("Subject2に流した値"); // next: 1.

            // それぞれのSubjectを完了する
            _subject1.OnCompleted();
            _subject2.OnCompleted(); // (既に完了してるので、2回目は呼ばれない）
        }
    }
}