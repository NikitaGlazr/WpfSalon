using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSalon
{
    internal class Helper
    {
        public static Entities ent;

        public static Entities GetContext()
        {
            if (ent == null) {
                ent = new Entities();
            }
            return ent;
        }
    } 
}

