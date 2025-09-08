using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Projects._30_UniTask._30_7_バックグラウンドで別シーンをロードする
{
    public class MyTask : MonoBehaviour
    {
        [SerializeField] private string _nextSceneName = "NextScene";
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _transitionButton;
        [SerializeField] private Slider _progressSlider;
        private bool _isLoadStart;
        private AsyncOperation _operation;

        private void Awake()
        {
            // 最初は押せなくする
            _transitionButton.interactable = false;

            // ボタン押下処理
            _loadButton.onClick.AddListener(() => { _isLoadStart = true; });
            _transitionButton.onClick.AddListener(() => { _operation.allowSceneActivation = true; });
        }

        private async void Start()
        {
            // ボタンが押されるまで待機する
            await UniTask.WaitUntil(() => _isLoadStart);

            // シーンを非同期でロードする
            await LoadSceneAsync(_nextSceneName);

            // 完了時に呼ばれる処理
            _loadButton.interactable = false;
            _transitionButton.interactable = true;
        }

        /// <summary>
        /// 別シーンを非同期でロードする
        /// </summary>
        /// <param name="sceneName"></param>
        private async UniTask LoadSceneAsync(string sceneName)
        {
            // ロード処理を開始する
            _operation = SceneManager.LoadSceneAsync(sceneName);

            // ロードが完了しても自動で遷移しないようにする
            _operation.allowSceneActivation = false;

            // ロードが完了するまで待機する
            // progressは0.9までしか進まないので、それ以降は自前で待機する(Unityの仕様）
            while (_operation.progress < 0.9f)
            {
                // 進捗を表示する
                _progressSlider.value = _operation.progress;

                // 1フレーム待機する
                await UniTask.Yield();
            }

            _progressSlider.value = 1.0f;
        }
    }
}