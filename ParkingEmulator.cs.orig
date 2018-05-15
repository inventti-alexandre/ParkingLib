using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parking
{
    public class ParkingEmulator
    {
        private Timer stateTimer;
        private Timer logTimer;
        
        private List<Transaction> transactionsList;
        private static Lazy<ParkingEmulator> instance = new Lazy<ParkingEmulator>(() => new ParkingEmulator());
        private string fileName= "Transactions.log";

        public List<Car> CarsList { get; }
        public double EarnedMoney { get; set; }
        public int FreePlaces
        {
            get
            {
                return Settings.ParkingSpace - CarsList.Count;
            }
        }
        public int EngagedPlaces
        {
            get
            {
                return CarsList.Count;
            }
        }
        private string filePath
        {
            get
            {
                string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(directory, fileName);
            }
        }     
        private ParkingEmulator()
        {
            stateTimer = new Timer(changeParkingState, new object(), 0, Settings.Timeout);
            logTimer = new Timer(logTransactions, new object(), 0, 5000);
            CarsList = new List<Car>();
            transactionsList = new List<Transaction>();
        }

        public static ParkingEmulator GetInstanse()
        {
            return instance.Value;
        }

        public void AddCar(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(String.Format("Input '{0}' argumet was null!", nameof(car)));
            }
            if (!CarsList.Contains(car,new CarEqualityComparer()))
            {
                CarsList.Add(car);
            }
            else
            {
                throw new ArgumentException(String.Format("The parking already contains a car with '{0}' description! Choose another one and try again!",car.Id));
            }
        }

        public void RemoveCar(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(String.Format("Input '{0}' argument was null!", nameof(car)));
            }

            if(CarsList.Contains(car,new CarEqualityComparer()))
            {
                if (car.Balance > 0)
                    CarsList.Remove(car);
                else
                    throw new InvalidOperationException("This car's balance is less than 0. Please, top up an account and try again!");
            }
            else
            {
                throw new ArgumentException("There no such car on the parking!");
            }

        }
        public Task<string> GetTransactionsLog()
        {
            return Task.Run(() =>
            {
                StringBuilder sb = new StringBuilder();
                using (FileStream fs = File.Open(filePath, FileMode.Open,FileAccess.Read,FileShare.Write))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string allInfo = sr.ReadToEnd();
                        string[] transactions = allInfo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var transaction in transactions)
                        {
                            string[] parts = transaction.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            sb.AppendLine(String.Format("Transaction record date:{1}{0}Record time:{2}{0}Earned money:{3}{0}",
                                Environment.NewLine,
                                parts[1],
                                parts[2],
                                parts[3]));
                        }
                    }
                }
                return sb.ToString();
            });      
        }

        public Task<double> GetLastEarnedMoney()
        {
            return Task.Run(() =>
            {
                return Convert.ToDouble(File.ReadLines(filePath).Last());
            });
        }

        public string GetLastTranscations()
        {
            StringBuilder sb = new StringBuilder();
            transactionsList.ForEach(tr => sb.AppendLine(tr.ToString()).AppendLine());
            return sb.ToString();
        }

        private async void logTransactions(object obj)
        {
            
            await Task.Run(() =>
            {
                using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate | FileMode.Append,FileAccess.Write,FileShare.Read))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        double sum;
                        lock (transactionsList)
                        {
                            sum = transactionsList.Sum(tr => tr.SpentMoney);
                            this.EarnedMoney += sum;
                            transactionsList.Clear();
                        }
                        string strToLog = String.Format("#{0}{1}{0}{2}{0}{3}",
                            Environment.NewLine,
                            DateTime.Now.ToShortDateString(),
                            DateTime.Now.ToShortTimeString(),
                            sum);
                        sw.WriteLine(strToLog);
                    }
                }
            });
        }

        private async void changeParkingState(object obj)
        {
            await Task.Run(() => {
                lock (transactionsList)
                {
                    CarsList.ForEach(car =>
                    {
                        double requiredMoney;
                        if (Settings.PriceSet.TryGetValue(car.CarType, out requiredMoney))
                        {
                            double spentMoney = car.Balance >= requiredMoney ? requiredMoney : requiredMoney * Settings.Fine;
                            car.Balance -= spentMoney;
                            this.EarnedMoney += spentMoney;
                            Transaction tr = new Transaction(car.Id, spentMoney);
                            transactionsList.Add(tr);
                        }

                    });
                } 
            });
        }
    }
}
