using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace WumpusCore.Battle
{
    /// <summary>
    /// A structure of battle
    /// Consists of a set of battle modes multiple of which may not occupy the same input regime
    /// </summary>
    public class BattleStructure
    {
        /// <summary>
        /// The types of battle that comprise the current mode
        /// </summary>
        public BattleType[] Types;

        /// <summary>
        /// The input zones that are currently occupied
        /// </summary>
        public InputZone[] occupiedZones
        {
            get
            {
                Stack<InputZone> zones = new Stack<InputZone>(Types.Length);
                foreach (BattleType type in Types)
                {
                    InputZone[] pushZones = type.InputRegimes();
                    foreach (InputZone pushZone in pushZones)
                    {
                        if (zones.Contains(pushZone))
                        {
                            throw new ArgumentException("Input zones may not be occupied multiple times");
                        }
                        zones.Push(pushZone);
                    }
                }
                return zones.ToArray();
            }
        }

        public BattleStructure(BattleType[] types)
        {
            this.Types = types;
            var obj = occupiedZones;
        }
    }
}