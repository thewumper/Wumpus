using System;
using System.Collections.Generic;

namespace WumpusCore.Topology
{
    /// <summary>
    /// A direction you can enter or exit a room from
    /// </summary>
    public enum Directions
    {
        North,
        NorthEast,
        SouthEast,
        South,
        SouthWest,
        NorthWest
    }
    /// <summary>
    /// Helper functionality to directions
    /// </summary>
    public static class DirectionHelper
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

        private static readonly Dictionary<Directions, String> DirectionToLongName = new Dictionary<Directions, String>()
        {
            { Directions.North     , "North"      },
            { Directions.NorthEast , "North East" },
            { Directions.SouthEast , "South East" },
            { Directions.South     , "South"      },
            { Directions.SouthWest , "South West" },
            { Directions.NorthWest , "North West" },
        };
        
        private static readonly Dictionary<String, Directions> LongNameToDirection = new Dictionary<String, Directions>()
        {
            { "North"      , Directions.North     },
            { "North East" , Directions.NorthEast },
            { "South East" , Directions.SouthEast },
            { "South"      , Directions.South     },
            { "South West" , Directions.SouthWest },
            { "North West" , Directions.NorthWest },
        };
        
        /// <summary>
        /// Gets the opposite direction to a direction (North goes to south, NorthEast goes to SouthWest)
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">If you somehow provide a invalid direction</exception>
        public static Directions GetInverse(this Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return Directions.South;
                case Directions.South:
                    return Directions.North;
                case Directions.NorthEast:
                    return Directions.SouthWest;
                case Directions.SouthWest:
                    return Directions.NorthEast;
                case Directions.NorthWest: 
                    return Directions.SouthEast;
                case Directions.SouthEast:
                    return Directions.NorthWest;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid direction");
            }
        }
        /// <summary>
        /// Gets a direction from a short name string (e.g. NW)
        /// </summary>
        /// <param name="direction">name of the direction</param>
        /// <returns></returns>
        public static Directions GetDirectionFromShortName(String direction)
        {
            return ShortNameToDirection[direction];
        }
        /// <summary>
        /// Gets a string representation of a direction
        /// </summary>
        /// <param name="direction">name of the direction</param>
        /// <returns></returns>
        public static String GetShortNameFromDirection(Directions direction)
        {
            return DirectionToShortName[direction];
        }

        /// <summary>
        /// Gets a direction from a long name string (e.g. North West)
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Directions GetDirectionFromLongName(String direction)
        {
            return LongNameToDirection[direction];
        }
        /// <summary>
        /// Gets a long string representation of a direction 
        /// </summary>
        /// <param name="direction">direction itself</param>
        /// <returns></returns>
        public static String GetLongNameFromDirection(Directions direction)
        {
            return DirectionToLongName[direction];
        }
    }
}