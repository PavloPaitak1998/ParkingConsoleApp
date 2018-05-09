using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    public static class Settings
    {
        public static int TimeOut { get; set; }
        public static Dictionary<CarType, int> Price { get; set; }
        public static int ParkingSpace { get; set; }
        public static double Fine { get; set; }

        static Settings()
        {
            Price = new Dictionary<CarType, int>
            {
                { CarType.Bus, 2 },
                { CarType.Motorcycle, 1 },
                { CarType.Passenger, 3 },
                { CarType.Truck, 5 }
            };

            TimeOut = 3000;
            ParkingSpace = 3;
            Fine = 2;
        }
    }
}
