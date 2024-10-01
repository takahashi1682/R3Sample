using R3;
using UnityEngine;

namespace SubjectSamples
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class SubjectProgram1 : MonoBehaviour
    {
        // 時間経過を通知するためのSubject
        // 他クラスから参照するためのプロパティ
        private　readonly Subject<Unit> _updateSubject = new();
        public Subject<Unit> UpdateSubject => _updateSubject; // 他クラスから参照するためのプロパティ

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
            UpdateSubject.OnNext(Unit.Default);

            // 1秒経過したらイベントの発行を終了
            if (Time.time > 1)
            {
                // イベントの発行を終了
                // これ以降はOnNextは呼ばれない
                UpdateSubject.OnCompleted();
            }
        }
    }
}