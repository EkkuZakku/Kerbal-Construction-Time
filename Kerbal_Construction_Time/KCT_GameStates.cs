using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerbal_Construction_Time
{
    public static class KCT_GameStates
    {
        public static double UT;
        public static Vessel activeVessel;
        public static int totalCost, buildTime = 0;
        public static double finishDate;
        public static bool builtOnce = false, warpedOnce = true;
        public static bool showMainGUI = true, showSOIAlert = false;
        public static string lastSOIVessel = "";
        public static Dictionary<string, string> lstVessels = new Dictionary<string, string>();
        public static List<VesselType> VesselTypesForSOI = new List<VesselType>() { VesselType.Base, VesselType.Lander, VesselType.Probe, VesselType.Ship, VesselType.Station };
        public static List<Orbit.PatchTransitionType> SOITransitions = new List<Orbit.PatchTransitionType> { Orbit.PatchTransitionType.ENCOUNTER, Orbit.PatchTransitionType.ESCAPE };
        //public static GameScenes currentScene;

    }

}
