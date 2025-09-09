using System;
using R3;
using R3.Triggers;
using UnityEngine;

namespace _Projects._4_Operator._4_6_Debounceによる時間指定
{
    public class SampleOperator : MonoBehaviour
    {
        [SerializeField] private float _waitSeconds = 3;
        [SerializeField] private Transform _cube;
        [SerializeField] private Renderer _render;

        public void Start()
        {
            _render.material.color = Color.red;

            // マウスが乗ってから3秒後に色を変える
            _cube.OnMouseEnterAsObservable()
                .Debounce(TimeSpan.FromSeconds(_waitSeconds))
                .Subscribe(_ => _render.material.color = Color.green)
                .AddTo(_cube);

            // マウスが外れたら色を赤に戻す
            _cube.OnMouseExitAsObservable()
                .Subscribe(_ => _render.material.color = Color.red)
                .AddTo(_cube);

            // Cubeを左右に移動
            _cube.FixedUpdateAsObservable()
                .Subscribe(
                    _ => _cube.position = new Vector3(Mathf.PingPong(Time.time * 2, 5), _cube.position.y))
                .AddTo(_cube);
        }
    }
}