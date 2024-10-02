using System;
using R3;
using UnityEngine;

namespace _3_ReactiveProperty
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class ReactivePropertyProgram1 : MonoBehaviour
    {
        // Inspectorに表示する場合の宣言の仕方
        [SerializeField] private SerializableReactiveProperty<int> _count1 = new(0);
        // 外部に公開する場合はReadOnlyReactivePropertyを使用する（外部から代入不可にする）
        public ReadOnlyReactiveProperty<int> Count1 => _count1;

        // Inspectorに表示しない場合の宣言の仕方
        private readonly ReactiveProperty<int> _count2 = new(0);
        // 外部に公開する場合はReadOnlyReactivePropertyを使用する（外部から代入不可にする）
        public ReadOnlyReactiveProperty<int> Count2 => _count2;

        private void Start()
        {
            _count1.AddTo(this);
            _count2.AddTo(this);

            // 1秒毎にカウントアップ
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    _count1.Value++;
                    _count2.Value++;
                })
                .AddTo(this);
        }
    }
}