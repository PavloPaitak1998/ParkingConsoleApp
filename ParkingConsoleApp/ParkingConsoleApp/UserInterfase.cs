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
        
        static StateObjClass stateObjFor_timerTransactions; //Announcement State of the timer invoking method that records transactions history
        static List<StateObjClass> statesOC = new List<StateObjClass>(); //Announcement List of States of the timers invoking Charge method

        static Parking parking;

        //Announcement the timer invoking method that records transactions history
        static Timer timerTransactions;

        //Initialization of default values
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

            //Сhecking and processing the input of the Id
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

            //Сhecking and processing the input of the Car Type
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

            //Checking the existence of the Car
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
            Console.Write("9-Get amount of Parking Spaces\n");

            Console.WriteLine("0-Leave parking");

        }

        public static void Action()
        {
            int act = 0; ;
            //Сhecking and processing the input of the action
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

                //Сhecking the input of the action 1, 2, 3
                if (act==1 && parking.FreeParkingSpace == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The Parking is full if you want to add car you must do it later.\n Please try another action\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    ActionInfo();
                    continue;
                }
                else if ((act==2 || act==3) && parking.BusyParkingSpace == 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The parking is empty, you can't remove the car or refill balance");
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
                        //Сhecking and processing the method AddCar
                        while (true)
                        {
                            try
                            {
                                car = CarInfo();
                                car.Balance = GetBalance();
                                parking.AddCar(car);
                                break;
                            }
                            catch (NullReferenceException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                continue;
                            }
                            catch (CarAlreadyExistException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                continue;
                            }
                        }

                        //Initialization of State of the timer invoking method that charges payment for parking space
                        StateObjClass stateObj = new StateObjClass();
                        statesOC.Add(stateObj);//add to list

                        stateObj.TimerCanceled = false;
                        stateObj.car = car;

                        //Initialization of the timer invoking method that charges payment for parking space
                        Timer timer = new Timer(parking.Charge, stateObj, Settings.TimeOut, Settings.TimeOut);
                        stateObj.TimerReference = timer;

                        //Checking the timer invoking method that records transactions whether it exists
                        //If no, initialize the timer
                        if (timerTransactions == null)
                        {
                            //Initialization State of the timer invoking method that records transactions
                            stateObjFor_timerTransactions = new StateObjClass
                            {
                                TimerCanceled = false
                            };

                            //Initialization the timer invoking method that records transactions
                            timerTransactions = new Timer(parking.WriteTransactions, stateObjFor_timerTransactions, 0, 60000);
                            stateObjFor_timerTransactions.TimerReference = timerTransactions;
                        }
                        break;

                    }
                case 2:
                    {
                        //Сhecking and processing the existence of the Car
                        while (true)
                        {
                            try
                            {
                                car = CarInfo();
                                if (!parking.Exist(car))
                                {
                                    throw new CarNotExistException($"Car with this Id: {car.Id} and Type: {car.TypeCar} not exist.\n Please try to input another car information");
                                }
                                break;
                            }
                            catch (CarNotExistException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                continue;
                            }
                        }

                        //Сhecking and processing the method RemoveCar
                        while (true)
                        {
                            try
                            {
                                parking.RemoveCar(car);
                                break;
                            }
                            catch (OutOfBalanceException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                car.Balance = GetBalance();
                                continue;
                            }
                            catch(NullReferenceException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                car = CarInfo();
                                continue;
                            }
                        }

                        //Find and stop the timer which was fixed by this car
                        var stateObjClass = statesOC.Find(st => st.car.Id == car.Id);
                        stateObjClass.TimerCanceled = true;
                        statesOC.Remove(stateObjClass);

                        //Check Busy Parking Spaces. If Busy Parking Spaces=0 , stop the timer which invoking WriteTransactions()
                        if (parking.BusyParkingSpace == 0)
                        {
                            stateObjFor_timerTransactions.TimerCanceled = true;
                        }
                        break;
                    }                  
                case 3:
                    {
                        //Сhecking and processing the existence of the Car
                        while (true)
                        {
                            try
                            {
                                car = CarInfo();
                                if (!parking.Exist(car))
                                {
                                    throw new CarNotExistException($"Car with this Id: {car.Id} and Type: {car.TypeCar} not exist.\n Please try to input another car information");
                                }
                                break;
                            }
                            catch (CarNotExistException e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                continue;
                            }                            
                        }
                        
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
                        Console.WriteLine("Amount of the Parking Spaces {0}",Settings.ParkingSpace);
                        break;
                    }
                case 0:
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
