using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConsoleApp
{
    class Program
    {
        static bool exit = false;

        static bool Leave()
        {
            bool flag = false;
            bool leave = false;

            Console.WriteLine("Do you want to go back to Menu press (1) or leave the Parking  press (2)....");

            while (!flag)
            {
                switch (Console.ReadLine().ToLower())
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
                        Console.WriteLine("Uncorrect answer try again");
                        break;
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
