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
                    if (v.situation == Vessel.Situations.PRELAUNCH && v != KCT_GameStates.activeVessel)
                    {
                        FlightGlobals.SetActiveVessel(v);

                    }

                }
                KCT_GameStates.warpedOnce = false;

            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label(KCT_GameStates.activeVessel.Parts.Count.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_GameStates.buildTime.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_Utilities.getFormatedTime(KCT_GameStates.finishDate - KCT_GameStates.UT), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_GameStates.UT.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label("", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
