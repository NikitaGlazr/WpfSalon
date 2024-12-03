using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSalon
{
    internal class Helper
    {
        public static EntitiesBd ent;

        public static EntitiesBd GetContext()
        {
            if (ent == null) {
                ent = new EntitiesBd();
            }
            return ent;
        }
    } 
}

