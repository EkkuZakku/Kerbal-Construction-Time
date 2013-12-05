using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kerbal_Construction_Time
{
    public static class KCT_GUI
    {
        private static Rect iconPosition = new Rect(5, 55, 110, 30);
        private static Rect mainWindowPosition = new Rect(200, 200, 350, 200);
        private static Rect SOIAlertPosition = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
        private static GUIStyle windowStyle = new GUIStyle(HighLogic.Skin.window);

        public static void SetGUIPositions(GUI.WindowFunction OnWindow)
        {
            if (GUI.Button(iconPosition, "Show/Hide KCT", GUI.skin.button))
            {
                KCT_GameStates.showMainGUI = !KCT_GameStates.showMainGUI;
            }

            if (KCT_GameStates.showMainGUI)
            {
                mainWindowPosition = GUILayout.Window(8950, mainWindowPosition, KCT_GUI.drawMainGUI, "Kerbal Construction Time", windowStyle);

            }

            if (KCT_GameStates.showSOIAlert)
            {
                SOIAlertPosition = GUILayout.Window(8951, SOIAlertPosition, KCT_GUI.drawSOIAlertWindow, "SOI Change", windowStyle);

            }

        }

        public static void drawGUIs(int windowID)
        {

            if (KCT_GameStates.showMainGUI)
            {
                drawMainGUI(windowID);
            }

            if (KCT_GameStates.showSOIAlert == true)
            {
                drawSOIAlertWindow(windowID + 1);

            }

        }

        public static void drawMainGUI(int windowID)
        {
            //GUIStyle mySty = new GUIStyle(GUI.skin.button);
            //mySty.normal.textColor = mySty.focused.textColor = Color.white;
            //mySty.hover.textColor = mySty.active.textColor = Color.yellow;
            //mySty.onNormal.textColor = mySty.onFocused.textColor = mySty.onHover.textColor = mySty.onActive.textColor = Color.green;
            //mySty.padding = new RectOffset(16, 16, 8, 8);

            //sets the layout for the GUI, which is pretty much just some debug stuff for me.
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("#Parts", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label("Build Time (s)", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label("Build Time Remaining: ", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label("UT: ", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (GUILayout.Button("Warp until ready.", GUILayout.ExpandWidth(true)))
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v.situation == Vessel.Situations.PRELAUNCH && v != KCT_GameStates.activeVessel.vessel)
                    {
                        FlightGlobals.SetActiveVessel(v);

                    }

                }
                KCT_GameStates.canWarp = true;

            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label(KCT_GameStates.activeVessel.vessel.Parts.Count.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_GameStates.buildTime.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_Utilities.getFormatedTime(KCT_GameStates.finishDate - KCT_GameStates.UT), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_GameStates.UT.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (GUILayout.Button("Stop warp", GUILayout.ExpandWidth(true)))
            {
                KCT_GameStates.canWarp = false;
                TimeWarp.SetRate(0, true);

            }
            GUILayout.Label(KCT_GameStates.totalCost.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUI.DragWindow();

        }

        public static void drawSOIAlertWindow(int windowID)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("   Warp stopped due to SOI change.", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label("Vessel name: " + KCT_GameStates.lastSOIVessel, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (GUILayout.Button("Close", GUILayout.ExpandWidth(true)))
            {
                KCT_GameStates.showSOIAlert = false;
            }
            GUILayout.EndVertical();
            GUI.DragWindow();

        }

    }

}

/*Copyright (c) 2013, Zachary Eck (a.k.a. Ekku Zakku) All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This project is in no way associated with nor endorsed by Squad.
*/