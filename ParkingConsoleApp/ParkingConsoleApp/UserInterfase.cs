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
        static StateObjClass stateObjFor_timerTransactions;
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
            stateObjFor_timerTransactions = null;

        }

        public static ICar CarInfo()
        {
            int id = 0;
            CarType carType = new CarType();

            Console.WriteLine("Please input information about car.");
            while (true)
            {
                try
                {
                    Console.WriteLine("Id :");
                    id = int.Parse(Console.ReadLine());
                    if (id < 0)
                    {
                        throw new FormatException();
                    }
                    break;
                }
                catch (FormatException)
                {

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Uncorrect format entered data!");
                    Console.ForegroundColor = ConsoleColor.White;                    
                    continue;
                }
            }

            while (true)
            {
                try
                {
                    Console.WriteLine("Type of your car (passenger, truck, bus, motorcycle) :");
                    string str = Console.ReadLine().ToLower();

                    Console.Clear();

                    switch (str)
                    {
                        case "bus":
                            carType = CarType.Bus;
                            break;
                        case "passenger":
                            carType = CarType.Passenger;
                            break;
                        case "truck":
                            carType = CarType.Truck;
                            break;
                        case "motorcycle":
                            carType = CarType.Motorcycle;
                            break;
                        default:
                            throw new UncorrectFormatOfCar("It's uncorrect car type please try again !");
                    }
                    break;
                }
                catch (UncorrectFormatOfCar e)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }


            }
            
            

            if (parking.Cars.Exists(c => c.Id == id))
            {
                return parking.Cars.Find(c => c.Id == id);
            }
            else
            {
                return new Car { Id = id, TypeCar = carType };
            }


        }

        public static void ActionInfo()
        {
            Console.WriteLine("\n\tMenu");
            Console.WriteLine("Choose your action");
            Console.Write("1-Add car on parking\n");
            Console.Write("2-Remove car from parking\n");
            Console.Write("3-Refill your balance\n");
            Console.Write("4-Get number of Free Parking Spaces\n");
            Console.Write("5-Get number of Busy Parking Spaces\n");
            Console.Write("6-Get current balance of  Parking\n");
            Console.Write("7-Get Transactions Sum of  Parking in 1 min\n");
            Console.Write("8-Get Transactions History of  Parking in 1 min\n");

            Console.WriteLine("9-Leave parking");

        }

        public static void Action()
        {
            int act = 0; ;

            while (true)
            {
                try
                {
                    act = int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Uncorrect format entered data!");
                    Console.ForegroundColor = ConsoleColor.White;
                    ActionInfo();
                    continue;
                }
                
                if (act==1 && parking.FreeParkingSpace == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The Parking is full if you want to add car you must do it later.\n Please try another action\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    ActionInfo();
                    continue;
                }
                break;
            }
            

            Console.Clear();
            ICar car = null;

            switch (act)
            {
                case 1:
                    {
                        bool flag = true;
                        while (flag)
                        {
                            try
                            {
                                car = CarInfo();
                                car.Balance = GetBalance();
                                parking.AddCar(car);
                                flag = false;
                            }
                            catch (NullReferenceException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            catch (CarAlreadyExistException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }

                        StateObjClass stateObj = new StateObjClass();
                        statesOC.Add(stateObj);

                        stateObj.TimerCanceled = false;
                        stateObj.car = car;

                        Timer timer = new Timer(parking.Charge, stateObj, Settings.TimeOut, Settings.TimeOut);
                        stateObj.TimerReference = timer;

                        if (timerTransactions == null)
                        {
                            stateObjFor_timerTransactions = new StateObjClass
                            {
                                TimerCanceled = false
                            };

                            timerTransactions = new Timer(parking.WriteTransactions, stateObjFor_timerTransactions, 0, 60000);
                            stateObjFor_timerTransactions.TimerReference = timerTransactions;
                        }
                        break;

                    }
                case 2:
                    {
                        car = CarInfo();
                        bool flag = true;
                        while (flag)
                        {
                            try
                            {
                                parking.RemoveCar(car);
                                flag = false;
                            }
                            catch (OutOfBalanceException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                car.Balance = GetBalance();
                            }
                            catch (CarNotExistException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                car = CarInfo();
                            }
                            catch(NullReferenceException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                car = CarInfo();
                            }
                        }
                        
                        var stateObjClass = statesOC.Find(st => st.car.Id == car.Id);
                        stateObjClass.TimerCanceled = true;
                        statesOC.Remove(stateObjClass);

                        if (parking.BusyParkingSpace == 0)
                        {
                            stateObjFor_timerTransactions.TimerCanceled = true;
                        }
                        break;
                    }                  
                case 3:
                    {
                        car = CarInfo();
                        var balance = GetBalance();
                        parking.RefillBalance(car, balance);
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("Amount of Free Parking Spaces {0}", parking.FreeParkingSpace);
                        break;

                    }
                case 5:
                    {
                        Console.WriteLine("Amount of Busy Parking Spaces {0}",parking.BusyParkingSpace);
                        break;

                    }
                case 6:
                    {
                        Console.WriteLine("Parking Balance: {0}",parking.Balance);
                        break;
                    }
                case 7:
                    {
                        Console.Clear();
                        parking.ReadSumTransactions();
                        break;
                    }
                case 8:
                    {
                        Console.Clear();
                        parking.ReadTransactionsHistory();
                        break;
                    }
                case 9:
                    {
                        break;
                    }
            }
        }

        public static double GetBalance()
        {
            double balance = 0;
            while (true)
            {
                try
                {
                    Console.WriteLine("Please input balance of the car");
                    balance = double.Parse(Console.ReadLine());
                    if (balance<0)
                    {
                        throw new FormatException();
                    }
                    Console.Clear();
                    return balance;
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Uncorrect format entered data!");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
            }
            

        }

        static void Message(string message)
        {
            Console.WriteLine(message);
        }
    }
}
