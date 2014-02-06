using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kerbal_Construction_Time
{
    class KCT_Settings
    {

        public KCT_Settings()
        {

        }

        public void Load()
        {
            Kerbal_Construction_Time.Print("Attempting to load KCT save file.");
            try
            {
                string VesselListName = String.Format("VesselList-{0}", HighLogic.CurrentGame.Title);
                KSP.IO.PluginConfiguration configFile = KSP.IO.PluginConfiguration.CreateForType<Kerbal_Construction_Time>();
                configFile.load();
                
                // Add some settings here, like Career Mode Only.

                try
                {
                    configFile.GetValue<Kerbal_Construction_Time>(VesselListName);

                }
                catch (KSP.IO.IOException)
                {
                    configFile.SetValue(VesselListName, KCT_GameStates.vesselList);
                    Kerbal_Construction_Time.Print("Added new Config File entry, VesselList-" + HighLogic.CurrentGame.Title);

                }
                
            }
            catch (Exception ex)
            {
                Kerbal_Construction_Time.Print("Failed to load settings.\n" + ex.Message);

            }

        }

        public void Save()
        {
            Kerbal_Construction_Time.Print("Attempting to save KCT save file.");
            try
            {
                string VesselListName = String.Format("VesselList-{0}", HighLogic.CurrentGame.Title);
                KSP.IO.PluginConfiguration configFile = KSP.IO.PluginConfiguration.CreateForType<Kerbal_Construction_Time>();
                configFile.load();

                // Add settings to save here

                configFile.SetValue(VesselListName, KCT_GameStates.vesselList);
                
            }
            catch (Exception ex)
            {
                Kerbal_Construction_Time.Print("Failed to save settings.\n" + ex.Message);

            }

        }

    }

}

/*Zachary Eck (EkkuZakku), previous copyright holder, hereby grants magico13 the copyright, license, and code for Kerbal Construction Time.

Copyright (c) 2014, magico13, new license holder and owner of Kerbal Construction Time code.
All rights reserved.

Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This project is in no way associated with nor endorsed by Squad.
*/