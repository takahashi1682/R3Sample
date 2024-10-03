using UnityEngine;
using R3;

namespace Projects._2_Subject._2_1_データを発行する
{
    public class Subject : MonoBehaviour
    {
        private　readonly Subject<int> _updateSubject = new();

        // 他クラスから参照するためのプロパティ
        public Subject<int> UpdateSubject => _updateSubject;

        private int _count;

        private void Start()
        {
            // このコンポーネントが破棄されたらイベントの発行を終了
            // この処理を書かないとオブジェクトが破棄された後も処理が動き続ける→メモリーリークの原因になる
            _updateSubject.AddTo(this);
        }

        private void Update()
        {
            // イベントを発行
            // 第一引数は購読者に送るデータ（特になければUnit.Default）を指定する
            UpdateSubject.OnNext(_count++);
        }
    }
}