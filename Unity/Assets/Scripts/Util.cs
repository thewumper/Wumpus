using System.Collections.Generic;
using WumpusCore.Controller;

namespace WumpusUnity
{
    public static class Util
    {
        public static List<string> GetRoomHintString(List<Controller.DirectionalHint> hints )
        {
            List<string> hintString = new List<string>();
            foreach (Controller.DirectionalHint hint in hints)
            {
                foreach (RoomAnomaly anomaly in hint.Hazards)
                {
                    string hintText;
                    switch (anomaly)
                    {
                        case RoomAnomaly.Vat:
                            hintText = "You hear bubbling";
                            break;
                        case RoomAnomaly.Rat:
                            hintText = "You hear the sounds of hundreds of tiny feet";
                            break;
                        case RoomAnomaly.Wumpus:
                            hintText = "You smell a wumpus";
                            break;
                        case RoomAnomaly.Bats:
                            hintText = "You hear the flapping of wings";
                            break;
                        case RoomAnomaly.Acrobat:
                            hintText = "You hear circus music";
                            break;
                        case RoomAnomaly.Cat:
                            hintText = "You hear meowing";
                            break;
                        default:
                            hintText = "You hear unknown sound";
                            break;
                    }
                    hintString.Add(hintText);
                }
            }
            return hintString;
        }
    }
}
