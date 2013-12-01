using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerbal_Construction_Time
{
    static class KCT_Utilities
    {
        public static string getFormatedTime(double time)
        {
            StringBuilder formatedTime = new StringBuilder();
            formatedTime.AppendFormat("{0,2:0} days, ", time);
            time = time % 86400;
            formatedTime.AppendFormat("{0,2:0} hours, ", time);
            time = time % 3600;
            formatedTime.AppendFormat("{0,2:0} minutes, ", time);
            time = time % 60;
            formatedTime.AppendFormat("{0,2:0} seconds", time);

            return formatedTime.ToString();
        }
    }

}
