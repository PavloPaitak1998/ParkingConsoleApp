using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    public class StateObjClass
    {
        // Used to hold parameters for calls to TimerTask.  
        public System.Threading.Timer TimerReference;
        public bool TimerCanceled;
        public ICar car;
    }
}
