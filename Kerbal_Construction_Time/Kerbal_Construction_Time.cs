using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;

namespace Kerbal_Construction_Time
{

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Kerbal_Construction_Time : MonoBehaviour
    {
        private static Rect windowPosition = new Rect(200, 200, 350, 200);
        private static GUIStyle windowStyle = null;
        private int totalCost, buildTime = 0;
        private double finishDate, UT;
        private Vessel activeVessel;
        private bool builtOnce = false, warpedOnce = true;
        private Dictionary<string, string> lstVessels = new Dictionary<string, string>();
        public List<VesselType> VesselTypesForSOI = new List<VesselType>() { VesselType.Base, VesselType.Lander, VesselType.Probe, VesselType.Ship, VesselType.Station };
        public List<Orbit.PatchTransitionType> SOITransitions = new List<Orbit.PatchTransitionType> { Orbit.PatchTransitionType.ENCOUNTER, Orbit.PatchTransitionType.ESCAPE };

        public void Awake()
        {
            RenderingManager.AddToPostDrawQueue(0, OnDraw);

        }

        private void OnDraw()
        {
            windowPosition = GUI.Window(1234, windowPosition, OnWindow, "VAB+SPH Time Clock", windowStyle);

        }

        public void Start()
        {
            windowStyle = new GUIStyle(HighLogic.Skin.window);
            UT = Planetarium.GetUniversalTime();

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

            activeVessel = FlightGlobals.fetch.activeVessel;

            if (activeVessel.situation == Vessel.Situations.PRELAUNCH && builtOnce == false)
            {
                PreBuild();

            }

            builtOnce = true;

        }

        public void FixedUpdate()
        {
            try
            {
                if (warpedOnce == false && (UT = Planetarium.GetUniversalTime()) < finishDate)
                {
                    int warpRate = TimeWarp.CurrentRateIndex;

                    if (SOIAlert())
                    {
                        TimeWarp.SetRate(0, true);
                        warpedOnce = true;

                    }
                    else
                    {
                        if ((finishDate - UT) < Math.Pow(4, warpRate) && (finishDate - UT) < Math.Pow(4, warpRate - 1))// || SOIAlert())
                        {
                            TimeWarp.SetRate(--warpRate, true);

                        }
                        else if ((finishDate - UT) > Math.Pow(4, warpRate) && TimeWarp.CurrentRateIndex < 7)// && !SOIAlert())
                        {
                            TimeWarp.SetRate(++warpRate, true);

                        }

                    }

                }
                else if (warpedOnce == false && activeVessel.situation == Vessel.Situations.PRELAUNCH)
                {
                    ManageFuel(activeVessel, true);

                    warpedOnce = true;

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
            ManageFuel(activeVessel, false);

            foreach (Part p in activeVessel.Parts)
            {
                totalCost += p.partInfo.cost;

            }

            buildTime = totalCost;
            finishDate = UT + buildTime;

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
                if (VesselTypesForSOI.Contains(v.vesselType))// && SOITransitions.Contains(v.orbit.patchEndTransition))
                {
                    if (v != activeVessel)
                    {
                        if (!lstVessels.ContainsKey(v.id.ToString()))
                        {
                            lstVessels.Add(v.id.ToString(), v.mainBody.bodyName);
                            print("Vessel " + v.id.ToString() + " added to lstVessels.");

                        }
                        else if (v.mainBody.bodyName != lstVessels[v.id.ToString()])
                        {
                            print("Vessel " + v.id.ToString() + " SOI change.");
                            lstVessels[v.id.ToString()] = v.mainBody.bodyName;
                            return true;

                        }

                    }

                }

            }

            return false;

        }

        private void OnWindow(int windowID)
        {
            GUIStyle mySty = new GUIStyle(GUI.skin.button);
            mySty.normal.textColor = mySty.focused.textColor = Color.white;
            mySty.hover.textColor = mySty.active.textColor = Color.yellow;
            mySty.onNormal.textColor = mySty.onFocused.textColor = mySty.onHover.textColor = mySty.onActive.textColor = Color.green;
            mySty.padding = new RectOffset(16, 16, 8, 8);

            //sets the layout for the GUI, which is pretty much just some debug stuff for me.
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("#Parts", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label("Build Time (s)", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label("Finish Time: ", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label("UT: ", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (GUILayout.Button("Warp until ready.", GUILayout.ExpandWidth(true)))
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v.situation == Vessel.Situations.PRELAUNCH && v != activeVessel)
                    {
                        FlightGlobals.SetActiveVessel(v);

                    }

                }
                warpedOnce = false;

            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label(activeVessel.Parts.Count.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(buildTime.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(finishDate.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(Planetarium.GetUniversalTime().ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            //DragWindow makes the window draggable. The Rect specifies which part of the window it can by dragged by, and is 
            //clipped to the actual boundary of the window. You can also pass no argument at all and then the window can by
            //dragged by any part of it. Make sure the DragWindow command is AFTER all your other GUI input stuff, or else
            //it may "cover up" your controls and make them stop responding to the mouse.
            GUI.DragWindow();

        }

    }

}
