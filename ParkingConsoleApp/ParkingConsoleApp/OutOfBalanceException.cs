using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    class OutOfBalanceException : Exception
    {
        public OutOfBalanceException(string message)
       : base(message)
        { }

    }
}
