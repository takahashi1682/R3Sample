using System;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Projects._99_Challenge.チャレンジ問題30
{
    public class ThrowObjectController : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToThrow;
        [SerializeField] private Transform _throwPoint;
        [SerializeField] private float _throwForce = 10f;
        [SerializeField] private Canvas _canvas;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;

            this.UpdateAsObservable()
                .Where(_ => Mouse.current.leftButton.wasPressedThisFrame)
                .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
                .Subscribe(_ => Throw())
                .AddTo(this);
        }

        private void Throw()
        {
            var mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
            mousePos.z = 0;

            var direction = (mousePos - _throwPoint.position).normalized;
            var throwObject = Instantiate(_objectToThrow, _throwPoint.position, _throwPoint.rotation);

            var rb = throwObject.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * _throwForce, ForceMode2D.Impulse);
        }
    }
}