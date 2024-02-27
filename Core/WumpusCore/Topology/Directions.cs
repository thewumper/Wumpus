using System;
using System.Collections.Generic;

namespace WumpusCore.Topology
{
    public enum Directions
    {
        North,
        NorthEast,
        SouthEast,
        South,
        SouthWest,
        NorthWest
    }

    static class DirectionHelper
    {
        private static readonly Dictionary<Directions, String> DirectionToShortName = new Dictionary<Directions, String>()
        {
            { Directions.North     , "N"  },
            { Directions.NorthEast , "NE" },
            { Directions.SouthEast , "SE" },
            { Directions.South     , "S"  },
            { Directions.SouthWest , "SW" },
            { Directions.NorthWest , "NW" },
        };
        
        private static readonly Dictionary<String, Directions> ShortNameToDirection = new Dictionary<String, Directions>()
        {
            { "N"  , Directions.North     },
            { "NE" , Directions.NorthEast },
            { "SE" , Directions.SouthEast },
            { "S"  , Directions.South     },
            { "SW" , Directions.SouthWest },
            { "NW" , Directions.NorthWest },
        };
        
        public static Directions GetDirectionFromShortName(String direction)
        {
            return ShortNameToDirection[direction];
        }
    }
}