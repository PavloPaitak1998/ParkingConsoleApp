using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    public class Transaction
    {
        public DateTime TransactionTime { get; set; }
        public int CarId { get; set; }
        public double Payment { get; set; }

        public override string ToString()
        {
            return "TransactionTime: " + TransactionTime + " Car Id: " + CarId + " Payment: " + Payment;
        }
    }
}
