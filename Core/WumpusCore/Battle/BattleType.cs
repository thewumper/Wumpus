using System.ComponentModel;

namespace WumpusCore.Battle
{
    /// <summary>
    /// A type of battle
    /// Represents what the player is expected to do in the moment
    /// </summary>
    public enum BattleType
    {
        BulletHell,
        Pattern,
        Infodump,
        Inforecall
    }

    public static class BattleTypeHelper
    {
        /// <summary>
        /// The regions of the keyboard that are occupied by the deployment of this battleType
        /// </summary>
        /// <returns>The regions of the keyboard that are occupied by the deployment of this battleType</returns>
        public static InputZone[] InputRegimes(this BattleType battleType)
        {
            switch (battleType)
            {
                case BattleType.BulletHell:
                    return new InputZone[] { InputZone.Arrows };
                case BattleType.Pattern:
                    return new InputZone[] { InputZone.Letters };
                case BattleType.Infodump:
                    return new InputZone[] { };
                case BattleType.Inforecall:
                    return new InputZone[] { InputZone.Numbers };
            }

            throw new InvalidEnumArgumentException("Input zones not assigned");
        }
    }
}