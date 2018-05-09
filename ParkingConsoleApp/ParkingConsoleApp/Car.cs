using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    class Car
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public CarType TypeCar { get; set; }
    }

    public enum CarType
    {
        Passenger,
        Truck,
        Bus,
        Motorcycle
    }
}
