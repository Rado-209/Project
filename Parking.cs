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
    }
}
