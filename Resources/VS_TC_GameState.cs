﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VAB_SPH_TimeClock
{
    public static class VS_TC_GameState
    {
        public static String LastSaveGameName = "";
        public static GameScenes LastGUIScene = GameScenes.LOADING;
        public static Vessel LastVessel = null;
        public static CelestialBody LastSOIBody = null;
        public static ITargetable LastVesselTarget = null;

        public static String CurrentSaveGameName = "";
        public static GameScenes CurrentGUIScene = GameScenes.LOADING;
        public static Vessel CurrentVessel = null;
        public static CelestialBody CurrentSOIBody = null;
        public static ITargetable CurrentVesselTarget = null;

        public static Boolean ChangedSaveGameName { get { return (LastSaveGameName != CurrentSaveGameName); } }
        public static Boolean ChangedGUIScene { get { return (LastGUIScene != CurrentGUIScene); } }
        public static Boolean ChangedVessel { get { if (LastVessel == null) return true; else return (LastVessel != CurrentVessel); } }
        public static Boolean ChangedSOIBody { get { if (LastSOIBody == null) return true; else return (LastSOIBody != CurrentSOIBody); } }
        public static Boolean ChangedVesselTarget { get { if (LastVesselTarget == null) return true; else return (LastVesselTarget != CurrentVesselTarget); } }

        //The current UT time - for alarm comparison
        //public static KACTime CurrentTime = new KACTime();*********************************************************************************************************************************************
        //public static KACTime LastTime = new KACTime();

        public static Boolean CurrentlyUnderWarpInfluence = false;
        public static DateTime CurrentWarpInfluenceStartTime;

        //Are we flying any ship?
        public static Boolean IsFlightMode
        {
            get { return FlightGlobals.fetch != null && FlightGlobals.ActiveVessel != null; }
        }

        public static Boolean PauseMenuOpen
        {
            get
            {
                try { return PauseMenu.isOpen; }
                catch (Exception)
                {
                    //if we cant read it it cant be open.
                    return false;
                }
            }
        }

        public static Boolean FlightResultsDialogOpen
        {
            get
            {
                try { return FlightResultsDialog.isDisplaying; }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        //Does the active vessel have any manuever nodes
        public static Boolean ManeuverNodeExists
        {
            get
            {
                Boolean blnReturn = false;
                if (IsFlightMode)
                {
                    if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
                    {
                        if (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes != null)
                        {
                            if (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count > 0)
                            {
                                blnReturn = true;
                            }
                        }
                    }
                }
                return blnReturn;
            }
        }

        //public static ManeuverNode ManeuverNodeFuture
        //{
        //    get
        //    {
        //        return FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.OrderBy(x => x.UT).FirstOrDefault(x => x.UT > KACWorkerGameState.CurrentTime.UT);
        //    }
        //}

        //public static List<ManeuverNode> ManeuverNodesFuture
        //{
        //    get
        //    {
        //        return (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.OrderBy(x => x.UT).SkipWhile(x => x.UT < KACWorkerGameState.CurrentTime.UT).ToList<ManeuverNode>());
        //    }
        //}


        public static Boolean SOIPointExists
        {
            get
            {
                Boolean blnReturn = false;

                if (FlightGlobals.ActiveVessel != null)
                {
                    if (FlightGlobals.ActiveVessel.orbit != null)
                    {
                        List<Orbit.PatchTransitionType> SOITransitions = new List<Orbit.PatchTransitionType> { Orbit.PatchTransitionType.ENCOUNTER, Orbit.PatchTransitionType.ESCAPE };
                        blnReturn = SOITransitions.Contains(FlightGlobals.ActiveVessel.orbit.patchEndTransition);
                    }
                }

                return blnReturn;
            }
        }

        //public static Boolean ApPointExists
        //{
        //    get
        //    {
        //        Boolean blnReturn = false;

        //        if (FlightGlobals.ActiveVessel != null)
        //        {
        //            if (FlightGlobals.ActiveVessel.orbit != null)
        //            {
        //                if (FlightGlobals.ActiveVessel.orbit.timeToAp > 0
        //                    && ((CurrentTime.UT + FlightGlobals.ActiveVessel.orbit.timeToAp) < FlightGlobals.ActiveVessel.orbit.EndUT))
        //                    blnReturn = true;
        //            }
        //        }
        //        return blnReturn;
        //    }
        //}
        //public static Boolean PePointExists
        //{
        //    get
        //    {
        //        Boolean blnReturn = false;

        //        if (FlightGlobals.ActiveVessel != null)
        //        {
        //            if (FlightGlobals.ActiveVessel.orbit != null)
        //            {
        //                if (FlightGlobals.ActiveVessel.orbit.timeToPe > 0
        //                    && ((CurrentTime.UT + FlightGlobals.ActiveVessel.orbit.timeToPe) < FlightGlobals.ActiveVessel.orbit.EndUT))
        //                    blnReturn = true;
        //            }
        //        }
        //        return blnReturn;
        //    }
        //}

        //do null checks on all these!!!!!
        public static void SetCurrentGUIStates()
        {
            VS_TC_GameState.CurrentGUIScene = HighLogic.LoadedScene;
        }

        public static void SetLastGUIStatesToCurrent()
        {
            VS_TC_GameState.LastGUIScene = VS_TC_GameState.CurrentGUIScene;
        }

        public static void SetCurrentFlightStates()
        {
            if (HighLogic.CurrentGame != null)
                VS_TC_GameState.CurrentSaveGameName = HighLogic.CurrentGame.Title;
            else
                VS_TC_GameState.CurrentSaveGameName = "";

            //try { KACWorkerGameState.CurrentTime.UT = Planetarium.GetUniversalTime(); }****************************************************************************************************************
            //catch (Exception) { }
            //if (Planetarium.fetch!=null) KACWorkerGameState.CurrentTime.UT = Planetarium.GetUniversalTime();

            if (VS_TC_GameState.CurrentGUIScene == GameScenes.FLIGHT)
            {
                VS_TC_GameState.CurrentVessel = FlightGlobals.ActiveVessel;
                VS_TC_GameState.CurrentSOIBody = FlightGlobals.ActiveVessel.mainBody;
                VS_TC_GameState.CurrentVesselTarget = FlightGlobals.fetch.VesselTarget;
            }
            else
            {
                VS_TC_GameState.CurrentVessel = null;
                VS_TC_GameState.CurrentSOIBody = null;
                VS_TC_GameState.CurrentVesselTarget = null;
            }
        }

        public static void SetLastFlightStatesToCurrent()
        {
            VS_TC_GameState.LastSaveGameName = VS_TC_GameState.CurrentSaveGameName;
            //KACWorkerGameState.LastTime = KACWorkerGameState.CurrentTime;******************************************************************************************************************************
            VS_TC_GameState.LastVessel = VS_TC_GameState.CurrentVessel;
            VS_TC_GameState.LastSOIBody = VS_TC_GameState.CurrentSOIBody;
            VS_TC_GameState.LastVesselTarget = VS_TC_GameState.CurrentVesselTarget;
        }
    }
}