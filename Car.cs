using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking
{
    public static class CarTypeConverter
    {
        public static CarType ToCarType(string carType)
        {
            switch (carType.ToLower())
            {
                case "passenger":
                    return CarType.Passenger;
                case "bus":
                    return CarType.Bus;
                case "truck":
                    return CarType.Truck;
                case "motorcycle":
                    return CarType.Motorcycle;
                default:
                    throw new ArgumentException("There are no CarType matching input string value");
            }
        }
    }
    /// <summary>
    /// Reprsents actual types of cars
    /// </summary>
    public enum CarType
    {
        Passenger,
        Truck,
        Bus,
        Motorcycle
    }
    /// <summary>
    /// Represents some car.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Car identifier
        /// </summary>
        public readonly string Id;
        /// <summary>
        /// Car balance
        /// </summary>
        public double Balance { get; set; }
        /// <summary>
        /// Type of car
        /// </summary>
        public CarType CarType { get; }
        /// <summary>
        /// Initialize new instance of <see cref="Car"/>
        /// </summary>
        /// <param name="balance">Initial amount of money.</param>
        /// <param name="type"><see cref="CarType"/> type of car.</param>
        public Car(string id, double balance, CarType type)
        {
            Id = id;
            Balance = balance;
            CarType = type;
        }

        public override string ToString()
        {
            return String.Format("Car identifier: '{1}'{0}Balance: {2}{0}Type:{3}{0}",
                Environment.NewLine,
                Id,
                Balance,
                CarType.ToString());
        }
    }


}
