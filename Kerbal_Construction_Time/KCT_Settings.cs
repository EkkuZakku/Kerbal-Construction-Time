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

/*Copyright (c) 2013, Zachary Eck (a.k.a. Ekku Zakku) All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This project is in no way associated with nor endorsed by Squad.
*/