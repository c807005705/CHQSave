using ControlDec;
using Device_Link_LTSMC;
using Interface.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moveAxis
{
    public class MoveXYZ :ImoveAxis
    {
        public LTSMC_Axis Axis { get; set; }
        ConDevice conDevice = new ConDevice();
        public void Axis_X()
        {
            
        }

        public void Axis_Y()
        {
            Axis.SetRunDistanceAsync(10000);
        }

        public void Axis_Z()
        {
            Axis.SetRunDistanceAsync(10000);
        }
    }
}
