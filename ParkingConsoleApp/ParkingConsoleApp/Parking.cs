using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    class Parking
    {
        readonly object locker = new object();
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instance { get { return lazy.Value; } }
        private Parking()
        {
        }

        public List<ICar> Cars { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public double Balance { get; private set; }
    }
}
