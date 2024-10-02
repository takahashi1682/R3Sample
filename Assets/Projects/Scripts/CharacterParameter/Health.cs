using Projects.Parameter.Core;
using R3;

namespace Projects.CharacterParameter
{
    public interface IHealth : IParameter
    {
        public ReadOnlyReactiveProperty<bool> IsDead();
        public ReadOnlyReactiveProperty<bool> IsAlive();
    }

    /// <summary>
    ///  体力
    /// </summary>
    public class Health : AbstractParameter, IHealth
    {
        /// <summary>
        /// 死んでいるかどうか
        /// </summary>
        /// <returns></returns>
        public ReadOnlyReactiveProperty<bool> IsDead()
        {
            return Current.Select(v => v <= 0).ToReadOnlyReactiveProperty();
        }

        /// <summary>
        /// 生きているかどうか
        /// </summary>
        /// <returns></returns>
        public ReadOnlyReactiveProperty<bool> IsAlive()
        {
            return Current.Select(v => v > 0).ToReadOnlyReactiveProperty();
        }
    }
}