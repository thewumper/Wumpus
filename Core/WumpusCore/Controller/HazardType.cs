using System;

namespace WumpusCore.Controller
{

    public enum HazardType
    {
        Wumpus,
        Vat,
        Rat,
        Bats,
        Acrobat
    }
    public static class RoomTypeExtensions {
        public static HazardType? ToHazard(this GameLocations.GameLocations.RoomType room)
        {
            switch (room)
            {
                case GameLocations.GameLocations.RoomType.Rats:
                    return HazardType.Rat;
                case GameLocations.GameLocations.RoomType.Vats:
                    return HazardType.Vat;
                case GameLocations.GameLocations.RoomType.Bats:
                    return HazardType.Bats;
                case GameLocations.GameLocations.RoomType.Acrobat:
                    return HazardType.Acrobat;
                default:
                    return null;
            }
        }

        public static string GetHint(this HazardType hazard)
        {
            switch (hazard)
            {
                case HazardType.Bats:
                    return "You hear the flapping of wings";
                case HazardType.Wumpus:
                    return "You smell the Wumpus";
                case HazardType.Vat:
                    return "You hear bubbling";
                case HazardType.Rat:
                    return "You hear skittering";
                case HazardType.Acrobat:
                    return "You hear circus music";
                default:
                    throw new ArgumentOutOfRangeException(nameof(hazard), hazard, null);
            }
        }
    }
}