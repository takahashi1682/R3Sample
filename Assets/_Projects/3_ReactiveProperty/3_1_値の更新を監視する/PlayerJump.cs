using R3;
using UnityEngine;

namespace _Projects._3_ReactiveProperty._3_1_値の更新を監視する
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _playerStatus;

        private void Start()
        {
            //_playerStatus.IsJump.Value = false;
            // _playerStatus.IsJump.V = 1; // 現在の値を取得する
            
            // Aの値が変わった時に呼ばれる
            _playerStatus.IsJump.Subscribe(value =>
            {
                if (value) // trueならjumpする
                {
                    OnJump();
                }
            }).AddTo(this);
        }

        private void OnJump()
        {
            Debug.Log("OnJump");
        }
    }
}