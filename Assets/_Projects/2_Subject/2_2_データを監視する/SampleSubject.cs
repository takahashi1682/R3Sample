using R3;
using UnityEngine;

namespace _Projects._2_Subject._2_2_データを監視する
{
    /// <summary>
    ///  イベントを発行するクラス
    /// </summary>
    public class SampleSubject : MonoBehaviour
    {
        // private　readonly Subject<Unit> _updateSubject = new();
        // public Subject<Unit> UpdateSubject => _updateSubject;

        // 上の2行を省略して1行で書くこともできる
        public Subject<int> UpdateSubject { get; } = new();

        private int _count;

        private void Start()
        {
            // このコンポーネントが破棄されたらイベントの発行を終了
            // この処理を書かないとオブジェクトが破棄された後も処理が動き続ける→メモリーリークの原因になる
            UpdateSubject.AddTo(this);
        }

        private void Update()
        {
            // イベントを発行
            // 第一引数は購読者に送るデータ（特になければUnit.Default）を指定する
            UpdateSubject.OnNext(_count++);
        }
    }
}