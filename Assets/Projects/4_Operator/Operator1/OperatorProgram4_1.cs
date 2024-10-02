using System;
using R3;
using R3.Triggers;
using UnityEngine;

namespace _4_Operator
{
    public class OperatorProgram4_1 : MonoBehaviour
    {
        [SerializeField] private float _waitSeconds = 3;
        [SerializeField] private Renderer _render;

        public void Start()
        {
            _render.material.color = Color.red;

            // マウスが乗ってから3秒後に色を変える
            this.OnMouseEnterAsObservable()
                .Debounce(TimeSpan.FromSeconds(_waitSeconds))
                .Subscribe(_ => _render.material.color = Color.green)
                .AddTo(this);

            // マウスが外れたら色を赤に戻す
            this.OnMouseExitAsObservable()
                .Subscribe(_ => _render.material.color = Color.red)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Subscribe(
                    _ => transform.position = new Vector3(Mathf.PingPong(Time.time * 2, 5), transform.position.y))
                .AddTo(this);
        }
    }
}