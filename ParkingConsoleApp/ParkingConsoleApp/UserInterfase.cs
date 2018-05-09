using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
        public static class UserInterfase
     {
        static StateObjClass stateObj2;
        static List<StateObjClass> statesOC = new List<StateObjClass>();
        static Parking parking;

        static Timer timerTransactions;

        static UserInterfase()
        {
            parking = Parking.Instance;
            parking.Added += Message;
            parking.Removed += Message;
            parking.Refilled += Message;
            timerTransactions = null;
            stateObj2 = null;

        }

        public static ICar CarInfo()
        {
            int id = 0;
            CarType carType = new CarType();

            Console.WriteLine("Please input information about car.");

            Console.WriteLine("Id :");
            id = int.Parse(Console.ReadLine());

            //Console.WriteLine("Balance :");
            //balance = double.Parse(Console.ReadLine());

            Console.WriteLine("Type of your car (Passenger, Truck, Bus, Motorcycle) :");
            string str = Console.ReadLine();

            Console.Clear();

            switch (str)
            {
                case "Bus":
                    carType = CarType.Bus;
                    break;
                case "Passenger":
                    carType = CarType.Passenger;
                    break;
                case "Truck":
                    carType = CarType.Truck;
                    break;
                case "Motorcycle":
                    carType = CarType.Motorcycle;
                    break;
                default:
                    throw new Exception();
            }

            if (parking.Cars.Exists(c => c.Id == id))
            {
                return parking.Cars.Find(c => c.Id == id);
            }

            return new Car { Id = id, TypeCar = carType };

        }

        public static void ActionInfo()
        {
            Console.WriteLine("\t\t\t\t\t\tMenu");
            Console.WriteLine("Choose your action");
            Console.Write("1-Add car on parking\n");
            Console.Write("2-Remove car from parking\n");
            Console.Write("3-Refill your balance\n");
            Console.Write("4-Get number of Free Parking Space\n");
            Console.Write("5-Get number of Busy Parking Space\n");
            Console.Write("6-Get current balance of  Parking\n");
            Console.Write("7-Get Transactions of  Parking in 1 min\n");
        }

        public static void Action()
        {
            int act = int.Parse(Console.ReadLine());

            Console.Clear();
            ICar car = null;

            switch (act)
            {
                case 1:
                    car = CarInfo();
                    car.Balance = GetBalance();
                    parking.AddCar(car);
                    StateObjClass stateObj = new StateObjClass();
                    statesOC.Add(stateObj);

                    stateObj.TimerCanceled = false;
                    stateObj.car = car;
                    Timer timer = new Timer(parking.Charge, stateObj, Settings.TimeOut, Settings.TimeOut);
                    stateObj.TimerReference = timer;
                    if (timerTransactions==null)
                    {
                        stateObj2 = new StateObjClass();
                        statesOC.Add(stateObj);
                        stateObj.TimerCanceled = false;

                        timerTransactions = new Timer(parking.WriteTransactions, stateObj2, 0, 60000);
                        stateObj.TimerReference = timerTransactions;
                    }                    
                    break;
                case 2:
                    car = CarInfo();
                    parking.RemoveCar(car);                    
                    var stateObjClass = statesOC.Find(st => st.car.Id == car.Id);
                    stateObjClass.TimerCanceled = true;
                    statesOC.Remove(stateObjClass);
                    if (parking.BusyParkingSpace==0)
                    {
                        stateObj2.TimerCanceled = true;
                    }
                    break;
                case 3:
                    car = CarInfo();
                    var balance = GetBalance();
                    parking.RefillBalance(car, balance);
                    break;
                case 4:
                    Console.WriteLine(parking.FreeParkingSpace);
                    break;
                case 5:
                    Console.WriteLine(parking.BusyParkingSpace);
                    break;
                case 6:
                    Console.WriteLine(parking.Balance);
                    break;
                case 7:
                    Console.Clear();
                    parking.ReadTransactions();
                    break;
            }
        }

        public static double GetBalance()
        {
            Console.WriteLine("Please input balance of the car");
            var balance = double.Parse(Console.ReadLine());
            Console.Clear();
            return balance;

        }

        static void Message(string message)
        {
            Console.WriteLine(message);
        }
    }
}
