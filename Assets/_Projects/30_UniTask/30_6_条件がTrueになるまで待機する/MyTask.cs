using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Projects._30_UniTask._30_6_条件がTrueになるまで待機する
{
    public class MyTask : MonoBehaviour
    {
        private bool _isTrue = false;

        private async void Start()
        {
            // _isTrue が true になるまで待機する
            await UniTask.WaitUntil(() => _isTrue);

            Debug.Log("条件が true になりました");
        }

        public void OnClicked()
        {
            _isTrue = true;
        }
    }
}