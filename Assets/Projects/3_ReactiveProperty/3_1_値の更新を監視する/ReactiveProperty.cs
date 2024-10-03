using System;
using R3;
using UnityEngine;

namespace Projects._3_ReactiveProperty._3_1_値の更新を監視する
{
    /// <summary>
    /// イベントを発行するクラス
    /// </summary>
    public class ReactiveProperty : MonoBehaviour
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
            // このクラスをDisposeするときに、内部で保持しているIDisposableもDisposeする
            _count1.AddTo(this);
            _count2.AddTo(this);

            // 1秒毎にカウントアップ
            Observable.Interval(TimeSpan.FromSeconds(1)) // 1秒間隔でストリームを流す(データの流れ）
                .Subscribe(_ => // ストリームを購読
                {
                    // 値が変わるとonNextが呼ばれる → 購読しているSubscribeが呼ばれる
                    _count1.Value++;
                    _count2.Value++;
                })
                .AddTo(this);
        }
    }
}