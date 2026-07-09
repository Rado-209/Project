using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    internal class Parking
    {
        public string ParkingID { get; set; }
        public string Location { get; set; }
        public int TotalSpaces { get; set; }
        public int AvailableSpaces { get; set; }
        public List<string> Vehicles { get; set; }

        public Parking(string id, string location, int totalSpaces, int availableSpaces, List<string> vehicles)
        {
            ParkingID = id;
            Location = location;
            TotalSpaces = totalSpaces;
            AvailableSpaces = availableSpaces;
            Vehicles = vehicles ?? new List<string>();
        }
        public string ToFileRow()
        {
            string vehiclesPart = string.Join(",", Vehicles);
            return $"{ParkingID};{Location};{TotalSpaces};{AvailableSpaces};{vehiclesPart}";
        }
        public override string ToString()
        {
            string vehiclesInfo = Vehicles.Count > 0 ? string.Join(", ", Vehicles) : "няма паркирани коли";
            return $" ID: {ParkingID} | Локация: {Location} | Общо паркоместа: {TotalSpaces} | " +
                   $"Свободни паркоместа: {AvailableSpaces} | Паркирани коли: {vehiclesInfo}";
        }
    }
}
