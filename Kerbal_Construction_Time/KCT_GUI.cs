using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kerbal_Construction_Time
{
    public static class KCT_GUI
    {
        private static Rect windowPosition = new Rect(200, 200, 350, 200);
        private static GUIStyle windowStyle = new GUIStyle(HighLogic.Skin.window);

        public static void drawWindow(int windowID)
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
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUI.DragWindow();

        }

        public static void SetWindowPosition(GUI.WindowFunction OnWindow)
        {
            windowPosition = GUI.Window(1234, windowPosition, OnWindow, "VAB+SPH Time Clock", windowStyle);

        }

    }

}
