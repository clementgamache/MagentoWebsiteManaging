using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoToDatabaseConverter
{
    static class Utilities
    {
        public static string getOriginalSku(string value)
        {
            return value.Replace("JE_POSTER_", "").Replace("HA_POSTER_", "").Replace("PH_POSTER_", "").Replace("C_POD_", "").Replace("F_POD_", "");
        }



        public static string getRatio(string firstSize)
        {
            string[] sizeTab = firstSize.Split('x');
            double small = Double.Parse(sizeTab[0]);
            double big = Double.Parse(sizeTab[1]);
            if (small > big)
            {
                double temp = small;
                small = big;
                big = temp;
            }
            double dblRatio = big / small;
            if (dblRatio <= 1) return "1:1";
            else if (dblRatio <= 1.42) return "4:3";
            else if (dblRatio < 1.75) return "3:2";
            else if (dblRatio < 2.5) return "2:1";
            else return "3:1";

        }
    }
}
