using R3;
using R3.Triggers;
using UnityEngine;

namespace Projects._1_ObserverとObserver._1_1_UnityのイベントをObservableに変換する
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class Observable1 : MonoBehaviour
    {
        [SerializeField] private GameObject _target;

        private void Awake()
        {
            // R3ではUnityの様々なイベントをObservableに変換することが出来ます
            // これにより非同期処理や複雑なゲームロジックを簡潔に管理することが出来ます

            // マイフレーム呼ばれる処理
            _target.UpdateAsObservable().Subscribe(_ => { }).AddTo(_target);
            // マイフレーム（Updateの後に）呼ばれる処理
            _target.LateUpdateAsObservable().Subscribe(_ => { }).AddTo(_target);
            // 固定フレームレートで呼ばれる処理
            _target.FixedUpdateAsObservable().Subscribe(_ => { }).AddTo(_target);

            // このオブジェクトがアクティブになった時に呼ばれる処理
            _target.OnEnableAsObservable().Subscribe(_ => Debug.Log("OnEnableAsObservable")).AddTo(_target);
            // このオブジェクトが非アクティブになった時に呼ばれる処理
            _target.OnDisableAsObservable().Subscribe(_ => Debug.Log("OnDisableAsObservable")).AddTo(_target);

            // このオブジェクトが破棄された時に呼ばれる処理
            _target.OnDestroyAsObservable().Subscribe(_ => Debug.Log("OnDestroyAsObservable")).AddTo(_target);

            // このオブジェクトが可視になった時に呼ばれる処理
            _target.OnBecameInvisibleAsObservable().Subscribe(_ => Debug.Log("OnBecameInvisibleAsObservable")).AddTo(_target);
            // このオブジェクトが可視でなくなった時に呼ばれる処理
            _target.OnBecameVisibleAsObservable().Subscribe(_ => Debug.Log("OnBecameVisibleAsObservable")).AddTo(_target);

            // このオブジェクトが衝突した時に呼ばれる処理
            _target.OnTriggerEnter2DAsObservable().Subscribe(_ => Debug.Log("OnTriggerEnter2DAsObservable")).AddTo(_target);
            // このオブジェクトが衝突している間呼ばれる処理
            _target.OnTriggerExit2DAsObservable().Subscribe(_ => Debug.Log("OnTriggerExit2DAsObservable")).AddTo(_target);
            // このオブジェクトが衝突している間呼ばれる処理
            _target.OnTriggerStay2DAsObservable().Subscribe(_ => Debug.Log("OnTriggerStay2DAsObservable")).AddTo(_target);

            // このオブジェクトが衝突した時に呼ばれる処理
            _target.OnCollisionEnter2DAsObservable().Subscribe(_ => Debug.Log("OnCollisionEnter2DAsObservable"))
                .AddTo(_target);
            // このオブジェクトが衝突している間呼ばれる処理
            _target.OnCollisionExit2DAsObservable().Subscribe(_ => Debug.Log("OnCollisionExit2DAsObservable")).AddTo(_target);
            // このオブジェクトが衝突している間呼ばれる処理
            _target.OnCollisionStay2DAsObservable().Subscribe(_ => Debug.Log("OnCollisionStay2DAsObservable")).AddTo(_target);

            // マウス関係のイベント
            _target.OnMouseEnterAsObservable().Subscribe(_ => Debug.Log("OnMouseEnterAsObservable")).AddTo(_target);
            _target.OnMouseOverAsObservable().Subscribe(_ => { }).AddTo(_target);
            _target.OnMouseExitAsObservable().Subscribe(_ => { }).AddTo(_target);
            _target.OnMouseUpAsObservable().Subscribe(_ => { }).AddTo(_target);
            _target.OnMouseDownAsObservable().Subscribe(_ => { }).AddTo(_target);
            _target.OnMouseUpAsButtonAsObservable().Subscribe(_ => { }).AddTo(_target);
            _target.OnMouseDragAsObservable().Subscribe(_ => { }).AddTo(_target);
        }
    }
}