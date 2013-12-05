using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerbal_Construction_Time
{
    static class KCT_Utilities
    {
        /// <summary>
        /// Formats a string from a time value into days, hours, minutes, and seconds.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <returns></returns>
        public static string getFormatedTime(double time)
        {
            StringBuilder formatedTime = new StringBuilder();
            formatedTime.AppendFormat("{0,2:0} days, ", Math.Floor(time / 86400));
            time = time % 86400;
            formatedTime.AppendFormat("{0,2:0} hours, ", Math.Floor(time / 3600));
            time = time % 3600;
            formatedTime.AppendFormat("{0,2:0} minutes, ", Math.Floor(time / 60));
            time = time % 60;
            formatedTime.AppendFormat("{0,2:0} seconds", time);

            return formatedTime.ToString();

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