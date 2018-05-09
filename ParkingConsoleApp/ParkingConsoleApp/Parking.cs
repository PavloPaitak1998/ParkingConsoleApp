using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    class Parking
    {
        public List<ICar> Cars { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public double Balance { get; private set; }
    }
}
