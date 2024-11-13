using Projects.CharacterParameter;
using TNRD;
using UnityEngine;
using UnityEngine.UI;
using R3;

namespace Projects._99_Challenge.チャレンジ問題9
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private SerializableInterface<IHealth> _player;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TMPro.TextMeshProUGUI _hpText;

        private void Start()
        {
            // プレイヤーのSliderを更新する処理（1行）

            // プレイヤーのTextを更新する処理（1行）※hpとmaxの値を結合してSubscribeすること
        }
    }
}