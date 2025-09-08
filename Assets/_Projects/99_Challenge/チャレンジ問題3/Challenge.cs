using _Projects.Scripts.CharacterParameter;
using _Projects.Scripts.Viewer;
using R3;
using R3.Triggers;
using UnityEngine;

namespace _Projects._99_Challenge.チャレンジ問題3
{
    public class Challenge : MonoBehaviour, ITextBinder, ISliderBinder
    {
        [SerializeField] private Health _health;
        [SerializeField] private GameObject _player;
        [SerializeField] private Transform _fire;

        private readonly Subject<int> _hitDamage = new();
        private Vector3 _startPos;
        private Vector3 _endPos;

        // UI表示用
        public ReadOnlyReactiveProperty<string> BindText =>
            Observable.CombineLatest(_health.Current, _health.Max).Select(p => $"{p[0]} / {p[1]}")
                .ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<float> BindSlider => _health.Rate.ToReadOnlyReactiveProperty();

        private void Start()
        {
            _hitDamage.AddTo(this);
            _hitDamage.Subscribe(x => _health.Sub(x)).AddTo(this);

            _startPos = _fire.position;
            _endPos = _startPos + Vector3.right * 2;

            // 炎を左右に移動させる
            this.FixedUpdateAsObservable().Subscribe(_ =>
                _fire.position = Vector3.Lerp(_startPos, _endPos, Mathf.PingPong(Time.time, 1))).AddTo(this);

            // 炎がプレイヤーに当たっている間、ダメージを与える
            _player.OnTriggerStay2DAsObservable()
                .Subscribe(_ => _hitDamage.OnNext(10)).AddTo(this);
        }
    }
}