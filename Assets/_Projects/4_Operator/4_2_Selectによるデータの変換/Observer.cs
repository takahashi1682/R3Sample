using _Projects.Scripts.Viewer;
using R3;
using UnityEngine;

namespace _Projects._4_Operator._4_2_Selectによるデータの変換
{
    public class Observer : MonoBehaviour, ITextBinder
    {
        [SerializeField] private Operator _target;

        // UI表示用の変数
        private readonly ReactiveProperty<string> _value = new();
        public ReadOnlyReactiveProperty<string> BindText => _value;

        private void Start()
        {
            _target.UpdateSubject
                .Select(x => x.ToString("N0")) // 受け取った値をカンマ区切りの文字列に変換
                .Subscribe(x => _value.Value = x)
                .AddTo(this);
        }
    }
}