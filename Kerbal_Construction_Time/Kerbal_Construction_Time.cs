using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;

namespace Kerbal_Construction_Time
{

    //[KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    //public class KCT_Tracking_Station : Kerbal_Construction_Time
    //{

    //}

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class KCT_Flight : Kerbal_Construction_Time
    {

    }

    [KSPAddon(KSPAddon.Startup.EditorVAB, false)]
    public class KCT_VABEditor : Kerbal_Construction_Time
    {

    }

    [KSPAddon(KSPAddon.Startup.EditorSPH, false)]
    public class KCT_SPHEditor : Kerbal_Construction_Time
    {

    }

    public class Kerbal_Construction_Time : MonoBehaviour
    {

        public void Awake()
        {
            RenderingManager.AddToPostDrawQueue(0, OnDraw);

        }

        private void OnDraw()
        {
            KCT_GUI.SetGUIPositions(OnWindow);

        }

        private void OnWindow(int windowID)
        {
            KCT_GUI.DrawGUIs(windowID);

        }

        public void Start()
        {
            //// Vessel activeVessel = FlightGlobals.fetch.ActiveVessel;
            // ConstructionVessel newConstructionVessel = new ConstructionVessel(activeVessel);
            // pause
            // INPUT: ask user whether to warp through build, simulate a launch, or go to tracking station
            // if TrackingStation
            //     removeVessel(launchedVessel);
            //     quicksave();
            //     add vessel to Construction list
            // else if SimulateLaunch
            //     quicksave();
            //     WAIT_INPUT: when done with launch (press End Simulation button),
            //     INPUT: ask user whether to modify or go through with launch
            //     if Modify
            //         Revert to VAB
            //     else if Launch
            //         goto Warp to launch
            /// else if Warp to launch
            ////     removeFuel(launchedVessel);
            ///     (automatically runs with Update()) Time Warp until any ship's SOI change alert or another ship ready for launch
            ///     if SOI alert
            ///         add vessel to Construction list
            //     if another ship is ready for launch and user wants to launch ready ship
            //         removeVessel(launchedVessel);
            //         addVessel(readyVessel);
            ////     Stop warp at build complete
            ////     Allow ship to launch (load with fuel)
            //

            KCT_GameStates.UT = Planetarium.GetUniversalTime();

            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                KCT_GameStates.showEditorGUI = false;
                KCT_GameStates.showMainGUI = true;

                if ((KCT_GameStates.activeVessel == null) || (FlightGlobals.fetch.activeVessel != KCT_GameStates.activeVessel.vessel))
                {
                    KCT_GameStates.activeVessel = new KCTVessel(FlightGlobals.fetch.activeVessel);

                }
                else
                {
                    KCT_GameStates.activeVessel.vessel = FlightGlobals.fetch.activeVessel;
                }

                foreach (KCTVessel kctv in KCT_GameStates.vesselList)
                {
                    if (KCT_GameStates.activeVessel.vessel.id == kctv.vessel.id)
                    {
                        KCT_GameStates.activeVessel = kctv;
                        KCT_GameStates.activeVesselIndex = KCT_GameStates.vesselList.IndexOf(kctv);
                        break;

                    }

                }

                if (!KCT_GameStates.vesselList.Contains(KCT_GameStates.activeVessel))
                {
                    KCT_GameStates.vesselList.Add(KCT_GameStates.activeVessel);

                }

                if ((KCT_GameStates.activeVessel.vessel.situation == Vessel.Situations.PRELAUNCH) && (KCT_GameStates.activeVessel.builtOnce == false))
                {
                    PreBuild();

                }

                KCT_GameStates.vesselList[KCT_GameStates.activeVesselIndex].builtOnce = true;
                KCT_GameStates.activeVessel.builtOnce = true;

            }
            else if ((HighLogic.LoadedScene == GameScenes.EDITOR) || (HighLogic.LoadedScene == GameScenes.SPH))
            {
                KCT_GameStates.showMainGUI = false;
                KCT_GameStates.showSOIAlert = false;
                KCT_GameStates.showEditorGUI = true;
            }


        }

        public void FixedUpdate()
        {
            KCT_GameStates.UT = Planetarium.GetUniversalTime();

            try
            {
                if ((KCT_GameStates.canWarp == true) && (KCT_GameStates.UT < KCT_GameStates.activeVessel.finishDate))
                {
                    int warpRate = TimeWarp.CurrentRateIndex;

                    if (SOIAlert())
                    {
                        TimeWarp.SetRate(0, true);
                        KCT_GameStates.canWarp = false;
                        KCT_GameStates.warpRateReached = false;

                    }
                    else
                    {
                        if (((KCT_GameStates.activeVessel.finishDate - KCT_GameStates.UT) < Math.Pow(4, warpRate)) &&
                            ((KCT_GameStates.activeVessel.finishDate - KCT_GameStates.UT) < Math.Pow(4, warpRate - 1)))
                        {
                            TimeWarp.SetRate(--warpRate, true);

                        }
                        else if (warpRate == 0 && KCT_GameStates.warpRateReached)
                        {
                            KCT_GameStates.canWarp = false;
                            KCT_GameStates.warpRateReached = false;

                        }
                        else if ((warpRate < 7) && ((KCT_GameStates.activeVessel.finishDate - KCT_GameStates.UT) > Math.Pow(4, warpRate)))
                        {
                            TimeWarp.SetRate(++warpRate, true);
                            KCT_GameStates.warpRateReached = true;

                        }

                    }

                }
                else if ((KCT_GameStates.canWarp == true) && (KCT_GameStates.activeVessel.vessel.situation == Vessel.Situations.PRELAUNCH))
                {
                    ManageFuel(KCT_GameStates.activeVessel.vessel, true);

                    KCT_GameStates.canWarp = false;
                    KCT_GameStates.warpRateReached = false;

                }
            }
            catch (IndexOutOfRangeException e)
            {
                print(e.Message);
                print(e.StackTrace);

            }

        }

        /// <summary>
        /// Gathers the build time.
        /// </summary>
        /// TODO: Make this method also manage multiple vessels to build.
        private void PreBuild()
        {
            ManageFuel(KCT_GameStates.activeVessel.vessel, false);

            KCT_GameStates.activeVessel.buildTime = KCT_Utilities.GetBuildTime(KCT_GameStates.activeVessel.vessel.Parts);
            KCT_GameStates.activeVessel.finishDate = KCT_GameStates.UT + KCT_GameStates.activeVessel.buildTime;

            //warpedOnce = false;

        }

        /// <summary>
        /// Completely fill or remove fuel from a vessel.
        /// </summary>
        /// <param name="vessel">Vessel to manage fuel</param>
        /// <param name="addFuel">True: fill fuel, False: empty fuel</param>
        public void ManageFuel(Vessel vessel, bool addFuel)
        {
            foreach (Part p in vessel.Parts)
            {
                foreach (PartResource pr in p.Resources)
                {
                    if (addFuel)
                        pr.amount = pr.maxAmount;
                    else
                        pr.amount -= pr.amount;

                }

            }

        }

        public bool SOIAlert()
        {
            foreach (Vessel v in FlightGlobals.Vessels)
            {
                if (KCT_GameStates.VesselTypesForSOI.Contains(v.vesselType))// && SOITransitions.Contains(v.orbit.patchEndTransition))
                {
                    if (v != KCT_GameStates.activeVessel.vessel)
                    {
                        if (!KCT_GameStates.vesselDict.ContainsKey(v.id.ToString()))
                        {
                            KCT_GameStates.vesselDict.Add(v.id.ToString(), v.mainBody.bodyName);
                            print("Vessel " + v.id.ToString() + " added to lstVessels.");

                        }
                        else if (v.mainBody.bodyName != KCT_GameStates.vesselDict[v.id.ToString()])
                        {
                            KCT_GameStates.lastSOIVessel = v.name;
                            print("Vessel " + v.id.ToString() + " SOI change.");
                            KCT_GameStates.vesselDict[v.id.ToString()] = v.mainBody.bodyName;
                            KCT_GameStates.showSOIAlert = true;
                            return true;

                        }

                    }

                }

            }

            return false;

        }

    }

}

/*Zachary Eck (EkkuZakku), previous copyright holder, hereby grants magico13 the copyright, license, and code for Kerbal Construction Time.

Copyright (c) 2014, magico13, new license holder and owner of Kerbal Construction Time code.
All rights reserved.

Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This project is in no way associated with nor endorsed by Squad.
*/