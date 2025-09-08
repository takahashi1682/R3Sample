using System.Globalization;
using _Projects.Scripts.Viewer;
using R3;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題10
{
    public class Challenge : MonoBehaviour, ITextBinder
    {
        [SerializeField] private SimpleTImer _oddTimer;
        [SerializeField] private SimpleTImer _evenTimer;

        private readonly ReactiveProperty<float> _time = new();
        public ReadOnlyReactiveProperty<string> BindText => _time.Select(x => x.ToString(CultureInfo.InvariantCulture))
            .ToReadOnlyReactiveProperty();

        private void Start()
        {
            _time.AddTo(this);
            // 処理追加
        }
    }
}