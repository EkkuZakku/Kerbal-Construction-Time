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

    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    //public class KCT_Editor : Kerbal_Construction_Time
    //{

    //}

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
            KCT_GUI.drawGUIs(windowID);

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

            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                KCT_GameStates.activeVessel = FlightGlobals.fetch.activeVessel;

                if (KCT_GameStates.activeVessel.situation == Vessel.Situations.PRELAUNCH && KCT_GameStates.builtOnce == false)
                {
                    PreBuild();

                }

                KCT_GameStates.builtOnce = true;

            }


        }

        public void FixedUpdate()
        {
            KCT_GameStates.UT = Planetarium.GetUniversalTime();

            try
            {
                if (KCT_GameStates.warpedOnce == false && (KCT_GameStates.UT) < KCT_GameStates.finishDate)
                {
                    int warpRate = TimeWarp.CurrentRateIndex;

                    if (SOIAlert())
                    {
                        TimeWarp.SetRate(0, true);
                        KCT_GameStates.warpedOnce = true;

                    }
                    else
                    {
                        if ((KCT_GameStates.finishDate - KCT_GameStates.UT) < Math.Pow(4, warpRate) &&
                            (KCT_GameStates.finishDate - KCT_GameStates.UT) < Math.Pow(4, warpRate - 1))// || SOIAlert())
                        {
                            TimeWarp.SetRate(--warpRate, true);

                        }
                        else if ((KCT_GameStates.finishDate - KCT_GameStates.UT) > Math.Pow(4, warpRate) && TimeWarp.CurrentRateIndex < 7)// && !SOIAlert())
                        {
                            TimeWarp.SetRate(++warpRate, true);

                        }

                    }

                }
                else if (KCT_GameStates.warpedOnce == false && KCT_GameStates.activeVessel.situation == Vessel.Situations.PRELAUNCH)
                {
                    ManageFuel(KCT_GameStates.activeVessel, true);

                    KCT_GameStates.warpedOnce = true;

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
            ManageFuel(KCT_GameStates.activeVessel, false);

            foreach (Part p in KCT_GameStates.activeVessel.Parts)
            {
                KCT_GameStates.totalCost += p.partInfo.cost;

            }

            KCT_GameStates.buildTime = KCT_GameStates.totalCost / 10 * KCT_GameStates.activeVessel.Parts.Count;
            KCT_GameStates.finishDate = KCT_GameStates.UT + KCT_GameStates.buildTime;

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
                    if (v != KCT_GameStates.activeVessel)
                    {
                        if (!KCT_GameStates.lstVessels.ContainsKey(v.id.ToString()))
                        {
                            KCT_GameStates.lstVessels.Add(v.id.ToString(), v.mainBody.bodyName);
                            print("Vessel " + v.id.ToString() + " added to lstVessels.");

                        }
                        else if (v.mainBody.bodyName != KCT_GameStates.lstVessels[v.id.ToString()])
                        {
                            KCT_GameStates.lastSOIVessel = v.name;
                            print("Vessel " + v.id.ToString() + " SOI change.");
                            KCT_GameStates.lstVessels[v.id.ToString()] = v.mainBody.bodyName;
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
