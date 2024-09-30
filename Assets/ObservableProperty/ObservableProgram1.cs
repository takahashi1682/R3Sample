using R3;
using R3.Triggers;
using UnityEngine;

namespace ObservableProperty
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class SubjectProgram1 : MonoBehaviour
    {
        private void Awake()
        {
            // R3ではUnityの様々なイベントをObservableに変換することが出来ます
            // これにより非同期処理や複雑なゲームロジックを簡潔に管理することが出来ます

            // マイフレーム呼ばれる処理
            this.UpdateAsObservable().Subscribe(_ => Debug.Log("Update")).AddTo(this);
            // マイフレーム（Updateの後に）呼ばれる処理
            this.LateUpdateAsObservable().Subscribe(_ => { }).AddTo(this);
            // 固定フレームレートで呼ばれる処理
            this.FixedUpdateAsObservable().Subscribe(_ => { }).AddTo(this);

            // このオブジェクトがアクティブになった時に呼ばれる処理
            this.OnEnableAsObservable().Subscribe(_ => { }).AddTo(this);
            // このオブジェクトが非アクティブになった時に呼ばれる処理
            this.OnDisableAsObservable().Subscribe(_ => { }).AddTo(this);

            // このオブジェクトが破棄された時に呼ばれる処理
            this.OnDestroyAsObservable().Subscribe(_ => { }).AddTo(this);

            // このオブジェクトが可視になった時に呼ばれる処理
            this.OnBecameInvisibleAsObservable().Subscribe(_ => { }).AddTo(this);
            // このオブジェクトが可視でなくなった時に呼ばれる処理
            this.OnBecameVisibleAsObservable().Subscribe(_ => { }).AddTo(this);

            // このオブジェクトが衝突した時に呼ばれる処理
            this.OnTriggerEnterAsObservable().Subscribe(_ => { }).AddTo(this);
            // このオブジェクトが衝突している間呼ばれる処理
            this.OnTriggerExitAsObservable().Subscribe(_ => { }).AddTo(this);
            // このオブジェクトが衝突している間呼ばれる処理
            this.OnTriggerStayAsObservable().Subscribe(_ => { }).AddTo(this);

            // このオブジェクトが衝突した時に呼ばれる処理
            this.OnCollisionEnterAsObservable().Subscribe(_ => { }).AddTo(this);
            // このオブジェクトが衝突している間呼ばれる処理
            this.OnCollisionExitAsObservable().Subscribe(_ => { }).AddTo(this);
            // このオブジェクトが衝突している間呼ばれる処理
            this.OnCollisionStayAsObservable().Subscribe(_ => { }).AddTo(this);

            // マウス関係のイベント
            this.OnMouseEnterAsObservable().Subscribe(_ => { }).AddTo(this);
            this.OnMouseOverAsObservable().Subscribe(_ => { }).AddTo(this);
            this.OnMouseExitAsObservable().Subscribe(_ => { }).AddTo(this);
            this.OnMouseUpAsObservable().Subscribe(_ => { }).AddTo(this);
            this.OnMouseDownAsObservable().Subscribe(_ => { }).AddTo(this);
            this.OnMouseUpAsButtonAsObservable().Subscribe(_ => { }).AddTo(this);
            this.OnMouseDragAsObservable().Subscribe(_ => { }).AddTo(this);
        }
    }
}