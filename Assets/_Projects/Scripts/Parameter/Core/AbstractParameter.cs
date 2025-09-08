using _Projects.Scripts.Viewer;
using R3;
using UnityEditor;
using UnityEngine;

namespace _Projects.Scripts.Parameter.Core
{
    public interface IParameter
    {
        ReadOnlyReactiveProperty<float> Max { get; }
        ReadOnlyReactiveProperty<float> Current { get; }
        ReadOnlyReactiveProperty<float> Rate { get; }
        void SetMax(float max);
        void SetClampValue(float value);
        void Add(float value);
        void Sub(float value);
        void Full();
    }

    /// <summary>
    /// 汎用的なパラメータクラス
    /// </summary>
    public abstract class AbstractParameter : MonoBehaviour, IParameter, ISliderBinder
    {
        [SerializeField] protected SerializableReactiveProperty<float> _max = new(1000);
        [SerializeField] protected SerializableReactiveProperty<float> _current = new(1000);
        [SerializeField] private bool _isShowGizmos;
        [SerializeField] private Vector3 _labelOffset = Vector3.right;

        public ReadOnlyReactiveProperty<float> Max => _max;
        public ReadOnlyReactiveProperty<float> Current => _current;
        public ReadOnlyReactiveProperty<float> Rate
            => _current.Select(v => v / _max.Value).ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<float> BindSlider => Rate;

        protected void Awake()
        {
            _max.AddTo(this);
            _current.AddTo(this);
        }

        public void SetMax(float max)
        {
            _max.Value = max;
            SetClampValue(_current.Value);
        }

        public void SetClampValue(float value) => _current.Value = Mathf.Clamp(value, 0, _max.Value);
        public void Add(float value) => SetClampValue(_current.Value + value);
        public void Sub(float value) => SetClampValue(_current.Value - value);
        public void Full() => _current.Value = _max.Value;

#if UNITY_EDITOR
        /// <summary>
        /// デバック用
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!_isShowGizmos) return;

            var info = $"{_current.Value}/{_max.Value}";
            var pos = transform.position + _labelOffset;
            Handles.Label(pos, info, GUI.skin.box);
        }
#endif
    }
}