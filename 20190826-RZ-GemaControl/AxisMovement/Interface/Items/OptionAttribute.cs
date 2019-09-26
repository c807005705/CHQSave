using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Items
{
    public class OptionAttribute : Attribute
    {
        public string Msg = "";
        public OptionAttribute(string msg)
        {
            this.Msg = msg;
        }
    }
}
