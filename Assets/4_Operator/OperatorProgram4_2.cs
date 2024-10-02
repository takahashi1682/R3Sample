using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _4_Operator
{
    public class OperatorProgram4_2 : MonoBehaviour
    {
        [SerializeField] private float _waitSeconds = 1;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        public void Start()
        {
            var count = 0;

            // クリックされてから1秒間は何もしない
            _button.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(_waitSeconds))
                .Subscribe(_ => _text.text = $"{++count}")
                .AddTo(this);
        }
    }
}