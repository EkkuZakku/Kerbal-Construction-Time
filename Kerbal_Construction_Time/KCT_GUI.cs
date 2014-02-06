using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kerbal_Construction_Time
{
    public static class KCT_GUI
    {
        private static Rect iconPosition = new Rect(Screen.width / 4, Screen.height - 30, 110, 30);
        private static Rect mainWindowPosition = new Rect(Screen.width / 3.5f, Screen.height / 3.5f, 350, 200);
        private static Rect editorWindowPosition = new Rect(Screen.width / 3.5f, Screen.height / 3.5f, 275, 90);
        private static Rect SOIAlertPosition = new Rect(Screen.width / 3, Screen.height / 3, 250, 100);
        private static GUIStyle windowStyle = new GUIStyle(HighLogic.Skin.window);

        public static void SetGUIPositions(GUI.WindowFunction OnWindow)
        {
            if (GUI.Button(iconPosition, "KCT", GUI.skin.button))
            {
                if (HighLogic.LoadedScene == GameScenes.FLIGHT)
                {
                    KCT_GameStates.showMainGUI = !KCT_GameStates.showMainGUI;
                }
                else if ((HighLogic.LoadedScene == GameScenes.EDITOR) || (HighLogic.LoadedScene == GameScenes.SPH))
                {
                    KCT_GameStates.showEditorGUI = !KCT_GameStates.showEditorGUI;
                }

            }

            if (KCT_GameStates.showMainGUI)
            {
                mainWindowPosition = GUILayout.Window(8950, mainWindowPosition, KCT_GUI.DrawMainGUI, "Kerbal Construction Time", windowStyle);

            }

            if (KCT_GameStates.showEditorGUI)
            {
                editorWindowPosition = GUILayout.Window(8950, editorWindowPosition, KCT_GUI.DrawEditorGUI, "Kerbal Construction Time", windowStyle);

            }

            if (KCT_GameStates.showSOIAlert)
            {
                SOIAlertPosition = GUILayout.Window(8951, SOIAlertPosition, KCT_GUI.DrawSOIAlertWindow, "SOI Change", windowStyle);

            }

        }

        public static void DrawGUIs(int windowID)
        {

            if (KCT_GameStates.showMainGUI)
            {
                DrawMainGUI(windowID);
            }

            if (KCT_GameStates.showEditorGUI)
            {
                DrawEditorGUI(windowID);
            }

            if (KCT_GameStates.showSOIAlert == true)
            {
                DrawSOIAlertWindow(windowID + 1);

            }

        }

        public static void DrawMainGUI(int windowID)
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
                if (FlightGlobals.ActiveVessel.id != KCT_GameStates.activeVessel.vessel.id)
                {
                    FlightGlobals.SetActiveVessel(KCT_GameStates.activeVessel.vessel);
                }
                KCT_GameStates.canWarp = true;

            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label(KCT_GameStates.activeVessel.vessel.Parts.Count.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_GameStates.activeVessel.buildTime.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_Utilities.GetFormatedTime(KCT_GameStates.activeVessel.finishDate - KCT_GameStates.UT), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_GameStates.UT.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (GUILayout.Button("Stop warp", GUILayout.ExpandWidth(true)))
            {
                KCT_GameStates.canWarp = false;
                TimeWarp.SetRate(0, true);

            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUI.DragWindow();

        }

        private static void DrawEditorGUI(int windowID)
        {
            double buildTime = KCT_Utilities.GetBuildTime(EditorLogic.fetch.ship.Parts);

            GUILayout.BeginVertical();
            GUILayout.Label("Estimated Build Time:", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Label(KCT_Utilities.GetFormatedTime(buildTime), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();

            GUI.DragWindow();

        }

        public static void DrawSOIAlertWindow(int windowID)
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

/*Zachary Eck (EkkuZakku), previous copyright holder, hereby grants magico13 the copyright, license, and code for Kerbal Construction Time.

Copyright (c) 2014, magico13, new license holder and owner of Kerbal Construction Time code.
All rights reserved.

Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This project is in no way associated with nor endorsed by Squad.
*/