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
        public static KCTVessel activeVessel = null;
        public static int activeVesselIndex;
        public static List<KCTVessel> vesselList = new List<KCTVessel>();
        public static int totalCost;
        public static bool canWarp = false, warpRateReached = false;
        public static bool showMainGUI = false, showEditorGUI = false, showSOIAlert = false;
        public static string lastSOIVessel = "";
        public static Dictionary<string, string> vesselDict = new Dictionary<string, string>();
        public static List<VesselType> VesselTypesForSOI = new List<VesselType>() { VesselType.Base, VesselType.Lander, VesselType.Probe, VesselType.Ship, VesselType.Station };
        public static List<Orbit.PatchTransitionType> SOITransitions = new List<Orbit.PatchTransitionType> { Orbit.PatchTransitionType.ENCOUNTER, Orbit.PatchTransitionType.ESCAPE };

        public static void SetVesselList(List<KCTVessel> newVesselList)
        {
            vesselList = newVesselList;

        }

    }

}

/*Zachary Eck (EkkuZakku), previous copyright holder, hereby grants magico13 the copyright, license, and code for Kerbal Construction Time.

Copyright (c) 2014, magico13, new license holder and owner of Kerbal Construction Time code.
All rights reserved.

Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This project is in no way associated with nor endorsed by Squad.
*/