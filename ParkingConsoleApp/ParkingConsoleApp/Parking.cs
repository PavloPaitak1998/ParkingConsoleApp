using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParkingConsoleApp.Program;

namespace ParkingConsoleApp
{
    public delegate void ParkingStateHandler(string message);

    class Parking
    {
        readonly object locker = new object();
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instance { get { return lazy.Value; } }
        private Parking()
        {
            Cars = new List<ICar>();
            Transactions = new List<Transaction>();
            Balance = 0;
            FreeParkingSpace = Settings.ParkingSpace;
            BusyParkingSpace = 0;
            numberOfCars = 0;
            path = "Transaction.txt";

        }

        private int numberOfCars;
        private string path;

        public event ParkingStateHandler Removed;
        public event ParkingStateHandler Added;
        public event ParkingStateHandler Refilled;

        public List<ICar> Cars { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public double Balance { get; private set; }

        public int FreeParkingSpace { get; private set; }
        public int BusyParkingSpace { get; private set; }

        public void AddCar(ICar car)
        {

            if (car == null)
            {
                throw new NullReferenceException("Sth went wrong please input Car information again!");
            }
            else if (Exist(car))
            {
                throw new CarAlreadyExistException("Car with this Id: {car.Id} and Type: {car.TypeCar} already exist.\n Please try to input another car information");
            }

            Cars.Add(car as Car);
            Added($"Car: Id={car.Id} Type={car.TypeCar} is added");
            BusyParkingSpace++;
            FreeParkingSpace--;
            numberOfCars++;
        }

        public void RemoveCar(ICar car)
        {
            if (car == null)
            {
                throw new NullReferenceException("Sth went wrong please input Car information again!");
            }
            else if (!Exist(car))
            {
                throw new CarNotExistException($"Car with this Id: {car.Id} and Type: {car.TypeCar} not exist.\n Please try to input another car information");
            }
            else if (car.Balance < 0)
            {
                throw new OutOfBalanceException("Car balance is out you have to refill Balance");
            }

            Cars.Remove(car as Car);
            Removed($"Car: Id={car.Id} Type={car.TypeCar} is removed");
            BusyParkingSpace--;
            FreeParkingSpace++;
            numberOfCars--;
        }

        public void RefillBalance(ICar car, double balance)
        {
            if (car == null)
            {
                throw new NullReferenceException();
            }
            else if (!Exist(car))
            {
                throw new Exception();
            }

            var index = Cars.IndexOf(car as Car);

            Cars[index].Balance += balance;
            Refilled($"Car's balance: Id={car.Id} Type={car.TypeCar} is refilled. Balance is {car.Balance}");
        }

        public void Charge(object obj)
        {
            if (obj as StateObjClass == null)
            {
                throw new Exception();
            }
            var StateObj = obj as StateObjClass;
            if (StateObj.TimerCanceled)
            // Dispose Requested.  
            {
                StateObj.TimerReference.Dispose();
            }

            var payment = 0.0;
            if (StateObj.car.Balance < Settings.Price[StateObj.car.TypeCar])
            {
                payment = Settings.Price[StateObj.car.TypeCar] * Settings.Fine;
                StateObj.car.Balance -= payment;
            }
            else
            {
                payment = Settings.Price[StateObj.car.TypeCar];
                StateObj.car.Balance -= payment;
            }
            lock (locker)
            {

                Balance += payment;
                Transactions.Add(new Transaction
                {
                    TransactionTime = DateTime.Now,
                    CarId = StateObj.car.Id,
                    Payment = payment
                });
                //Console.WriteLine(Transactions.Last());
            }
        }

        public void WriteTransactions(object obj)
        {

            if (obj as StateObjClass == null)
            {
                throw new Exception();
            }
            var StateObj = obj as StateObjClass;

            if (StateObj.TimerCanceled)
            // Dispose Requested.  
            {
                StateObj.TimerReference.Dispose();
            }

            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                foreach (var transaction in Transactions)
                {
                    sw.WriteLine(transaction);
                }

            }
            Transactions = new List<Transaction>();
        }

        public void ReadTransactions()
        {
            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }
        }

        private bool Exist(ICar car)
        {
            return Cars.Exists(c => c.Id == car.Id && c.TypeCar == car.TypeCar);
        }
    }
}
