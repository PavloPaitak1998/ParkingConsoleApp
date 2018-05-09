using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    public interface ICar
    {
        int Id { get; set; }
        double Balance { get; set; }
        CarType TypeCar { get; set; }
    }
}
