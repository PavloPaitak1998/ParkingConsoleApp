using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    class Program
    {
       public static bool exit = false;

        static bool Leave()
        {
            bool flag = false;
            bool leave = false;

            while (!flag)
            {
                Console.WriteLine("Do you want to go back to Menu press (1) or leave the Parking  press (2)....");

                switch (Console.ReadLine())
                {
                    case "2":
                        flag = true;
                        leave = true;
                        break;

                    case "1":
                        flag = true;
                        leave = false;
                        break;

                    default:
                        Console.Clear();    
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Uncorrect answer try again");
                        Console.ForegroundColor = ConsoleColor.White;

                        continue;
                }
                Console.Clear();
            }

            return leave;
        }

        static void Main(string[] args)
        {
            while (!exit)
            {
                Console.WriteLine("Hello it's parking\nChoose action");
                UserInterfase.ActionInfo();
                UserInterfase.Action();
                if (Leave())
                {
                    exit = true;
                }
                Console.Clear();
            }
        }
    }
}
