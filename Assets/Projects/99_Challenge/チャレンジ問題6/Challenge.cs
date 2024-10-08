using System;
using Projects.Viewer;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projects._99_Challenge.チャレンジ問題6
{
    public class Challenge : MonoBehaviour, ITextBinder
    {
        [SerializeField] private GameObject _cubePrefab;
        private readonly ReactiveProperty<int> _count = new(1000);

        // UI表示用
        public ReadOnlyReactiveProperty<string> Text => _count.Select(x => x.ToString()).ToReadOnlyReactiveProperty();

        private void Start()
        {
            _count.AddTo(this);
            // -- ここに処理を追加してください --

        }
    }
}