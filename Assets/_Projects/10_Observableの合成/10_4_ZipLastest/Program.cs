using R3;
using UnityEngine;

namespace _Projects._10_Observableの合成._10_4_ZipLastest
{
    public class Program : MonoBehaviour
    {
        // 合成するObservable
        private readonly Subject<int> _subject1 = new();
        private readonly Subject<int> _subject2 = new();

        private void Start()
        {
            _subject1.AddTo(this);
            _subject2.AddTo(this);

            // 2つのSubjectを合成して購読する
            Observable
                .ZipLatest(
                    _subject1,
                    _subject2
                ).Subscribe(
                    onNext: x =>
                    {
                        foreach (var item in x)
                        {
                            Debug.Log("next." + item);
                        }
                        Debug.Log("------------------------");
                    },
                    onCompleted: _ => Debug.Log("completed.")
                ).AddTo(this);

            // それぞれのSubjectにデータを流す
            _subject1.OnNext(1); // 流れない
            _subject2.OnNext(2); // next: 1.  >  next: 2.
            
            _subject2.OnNext(3); // 流れない
            _subject2.OnNext(4); // 流れない
            _subject1.OnNext(5); // next: 5.  >  next: 4.
            
            _subject1.OnCompleted();
            _subject2.OnCompleted(); // completed.
        }
    }
}