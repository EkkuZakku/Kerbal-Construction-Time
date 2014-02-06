using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerbal_Construction_Time
{
    public class KCTVessel
    {
        public Vessel vessel;
        public bool builtOnce = false;
        public double buildTime = 0, finishDate;

        public KCTVessel(Vessel vessel)
        {
            this.vessel = vessel;

        }

    }

}

/*Zachary Eck (EkkuZakku), previous copyright holder, hereby grants magico13 the copyright, license, and code for Kerbal Construction Time.

Copyright (c) 2014, magico13, new license holder and owner of Kerbal Construction Time code.
All rights reserved.

Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This project is in no way associated with nor endorsed by Squad.
*/