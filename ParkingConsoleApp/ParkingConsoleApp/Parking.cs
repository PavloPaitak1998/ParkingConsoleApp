﻿using System;
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
            
        }

        private int numberOfCars;

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
                throw new NullReferenceException();
            }
            else if (Exist(car))
            {
                throw new Exception();
            }
            else if (numberOfCars == Settings.ParkingSpace)
            {
                numberOfCars--;
                throw new Exception();
            }
            else if (car.Balance < Settings.Price[car.TypeCar])
            {
                throw new Exception();
            }


            Cars.Add(car);
            Added($"Car: Id={car.Id} Type={car.TypeCar} is added");
            BusyParkingSpace++;
            FreeParkingSpace--;
            numberOfCars++;
        }

        public void RemoveCar(ICar car)
        {
            if (car == null)
            {
                throw new NullReferenceException();
            }
            else if (!Exist(car))
            {
                throw new Exception();
            }
            else if (car.Balance < 0)
            {
                throw new Exception();
            }

            Cars.Remove(car);
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

            var index = Cars.IndexOf(car);

            Cars[index].Balance += balance;
            Refilled($"Car's balance: Id={car.Id} Type={car.TypeCar} is refilled. Balance is {car.Balance}");
        }

        private bool Exist(ICar car)
        {
            return Cars.Exists(c => c.Id == car.Id && c.TypeCar == car.TypeCar);
        }
    }
}
