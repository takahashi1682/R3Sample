using R3;
using UnityEngine;

namespace _1_SubjectSamples
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class SubjectProgram2 : MonoBehaviour
    {
        // private readonly Subject<Unit> _updateSubject = new();
        // public Subject<Unit> UpdateSubject => _updateSubject;

        // ↓↓↓　上の定義は下の定義と同じ意味です ↓↓↓

        // 時間経過を通知するためのSubject
        // 他クラスから参照するためのプロパティ
        public Subject<int> TimerSubject { get; } = new();

        private int _count;

        private void Start()
        {
            // このコンポーネントが破棄されたらイベントの発行を終了
            TimerSubject.AddTo(this);

            // CountUpを1秒おきに呼び出す
            InvokeRepeating(nameof(CountUp), 0, 1);
        }

        private void CountUp()
        {
            TimerSubject.OnNext(_count++);
        }
    }
}