using Projects.Parameter.Core;
using UnityEngine;
using R3;

namespace Projects._99_Challenge.チャレンジ問題60
{
    public interface IStamina : IParameter
    {
    }

    public class Stamina : AbstractParameter, IStamina
    {
    }
}