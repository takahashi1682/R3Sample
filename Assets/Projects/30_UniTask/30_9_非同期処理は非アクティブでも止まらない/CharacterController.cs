using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Projects._30_UniTask._30_9_非同期処理は非アクティブでも止まらない
{
    public class CharacterController : MonoBehaviour,
        IPointerClickHandler　// クリックイベントを受け取るためのインターフェース
    {
        /// <summary>
        /// クリック時に呼ばれるイベント
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            // 自信を非表示にする
            gameObject.SetActive(false);

            // 一定時間後に自信をアクティブにする
            ReactivateAfterDelay(destroyCancellationToken).Forget();
        }

        private async UniTask ReactivateAfterDelay(CancellationToken token)
        {
            // 1秒待機する
            await UniTask.Delay(1000, cancellationToken: token);
            gameObject.SetActive(true);
        }
    }
}