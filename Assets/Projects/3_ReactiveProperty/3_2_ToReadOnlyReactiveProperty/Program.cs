using R3;
using UnityEngine;

namespace Projects._3_ReactiveProperty._3_2_ToReadOnlyReactiveProperty
{
    public class Program : MonoBehaviour
    {
        [SerializeField] private SerializableReactiveProperty<float> _currentHeath = new(100);
        [SerializeField] private SerializableReactiveProperty<float> _maxHeath = new(100);

        // 通常のReactivePropertyを使う場合
        public ReactiveProperty<float> CurrentHeath => _currentHeath;
        public ReactiveProperty<float> MaxHeath => _maxHeath;

        // 体力の割合を返すプロパティ
        private ReadOnlyReactiveProperty<float> _rate;
        // _rateがnullならRateを生成して返す　→　2回目以降は_rateを返す
        // これにより、Rateを2回以上呼び出しても、Observable.CombineLatestは1回しか呼ばれない
        public ReadOnlyReactiveProperty<float> HeathRate =>　_rate ??=
            Observable.CombineLatest(_currentHeath, _maxHeath)
                .Select(x => x[0] / x[1]).ToReadOnlyReactiveProperty();

        // 体力が0以下かどうかを返すプロパティ
        private ReadOnlyReactiveProperty<bool> _isDead;
        public ReadOnlyReactiveProperty<bool> IsDead => _isDead ??=
            _currentHeath.Select(x => x <= 0).ToReadOnlyReactiveProperty();

        private void Awake()
        {
            _currentHeath.AddTo(this);
            _maxHeath.AddTo(this);
        }

        public void SetValue(float currentHeath)
        {
            _currentHeath.Value = currentHeath;
        }
    }
}