using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking
{
    public class Transaction
    {
        public DateTime TransactionTime { get; }
        public string CarId { get; }
        public double SpentMoney { get; }

        public Transaction(string carId,double spentMoney)
        {
            TransactionTime = DateTime.Now;
            CarId = carId;
            SpentMoney = spentMoney;
        }

        public override string ToString()
        {
            return String.Format("Transaction record date:{1}{0}Record time:{2}{0}Earned money:{3}{0}",
                Environment.NewLine,
                DateTime.Now.ToShortDateString(),
                DateTime.Now.ToShortTimeString(),
                SpentMoney);
        }
    }
}
