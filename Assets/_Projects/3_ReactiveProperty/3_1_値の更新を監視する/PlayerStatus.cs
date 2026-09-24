using R3;
using UnityEngine;

namespace _Projects._3_ReactiveProperty._3_1_値の更新を監視する
{
    public class PlayerStatus : MonoBehaviour
    {
        // インスペクターに表示される
        public SerializableReactiveProperty<bool> _isJump = new();
        public ReadOnlyReactiveProperty<bool> IsJump => _isJump;


        private void Start()
        {
            _isJump.Value = true; // 値変更
            bool c = IsJump.CurrentValue;

            // このクラスが破棄されたら自動でDispose（停止）するようにする
            _isJump.AddTo(this);
        }
    }
}