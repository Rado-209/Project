using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Project;

namespace ParkingProject
{
    class Program
    {
        static string filePath = "parkings.txt";
        static List<Parking> allParkings = new List<Parking>();

        static void Main(string[] args)
        {
            LoadDataFromFile();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА ЗА УПРАВЛЕНИЕ НА ПАРКИНГИ ===");
                Console.WriteLine("1. Добавяне на нов паркинг");
                Console.WriteLine("2. Регистрация на превозно средство в паркинг");
                Console.WriteLine("3. Напускане на паркинг от превозно средство");
                Console.WriteLine("4. Проверка на наличността на паркоместа");
                Console.WriteLine("5. Справка за всички паркинги");
                Console.WriteLine("6. Изход");
                Console.Write("Изберете опция (1-6): ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddNewParking();
                        break;
                    case "2":
                        RegisterVehicle();
                        break;
                    case "3":
                        RemoveVehicle();
                        break;
                    case "4":
                        CheckAvailability();
                        break;
                    case "5":
                        ShowAllParkings();
                        break;
                    case "6":
                        exit = true;
                        Console.WriteLine("Благодарим ви, че използвахте системата! Довиждане.");
                        break;
                    default:
                        Console.WriteLine("Невалиден избор. Натиснете бутон за опит...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void LoadDataFromFile()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                return;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(';');
                if (parts.Length >= 4)
                {
                    string id = parts[0];
                    string location = parts[1];
                    int totalSpaces = int.Parse(parts[2]);
                    int availableSpaces = int.Parse(parts[3]);

                    List<string> vehicles = new List<string>();
                    if (parts.Length == 5 && !string.IsNullOrWhiteSpace(parts[4]))
                    {
                        vehicles = parts[4].Split(',').Select(v => v.Trim()).Where(v => v.Length > 0).ToList();
                    }

                    allParkings.Add(new Parking(id, location, totalSpaces, availableSpaces, vehicles));
                }
            }
        }

        static void SaveDataToFile()
        {
            List<string> linesToSave = allParkings.Select(p => p.ToFileRow()).ToList();
            File.WriteAllLines(filePath, linesToSave);
        }

        static void AddNewParking()
        {
            Console.WriteLine("--- Добавяне на нов паркинг ---");

            Console.Write("Въведете уникално ID на паркинга: ");
            string id = Console.ReadLine();


            if (allParkings.Any(p => p.ParkingID.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Грешка: Вече съществува паркинг с това ID!");
                Console.ReadKey();
                return;
            }

            Console.Write("Местоположение: ");
            string location = Console.ReadLine();

            Console.Write("Общ брой паркоместа: ");
            if (!int.TryParse(Console.ReadLine(), out int totalSpaces) || totalSpaces < 0)
            {
                Console.WriteLine("Грешка: Невалиден формат за общ брой паркоместа!");
                Console.ReadKey();
                return;
            }

            Console.Write("Налични паркоместа: ");
            if (!int.TryParse(Console.ReadLine(), out int availableSpaces) || availableSpaces < 0 || availableSpaces > totalSpaces)
            {
                Console.WriteLine("Грешка: Невалиден брой налични паркоместа (не може да е повече от общия брой)!");
                Console.ReadKey();
                return;
            }


            Parking newParking = new Parking(id, location, totalSpaces, availableSpaces, new List<string>());
            allParkings.Add(newParking);


            SaveDataToFile();

            Console.WriteLine("\nПаркингът е добавен успешно и файлът е актуализиран!");
            Console.ReadKey();
        }


        static Parking SelectParking()
        {
            Console.Write("Въведете ID на паркинга: ");
            string id = Console.ReadLine();

            Parking parking = allParkings.FirstOrDefault(p => p.ParkingID.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (parking == null)
            {
                Console.WriteLine("Паркинг с такова ID не е намерен.");
            }

            return parking;
        }

        static void RegisterVehicle()
        {
            Console.WriteLine("--- Регистрация на превозно средство в паркинг ---");

            Parking parking = SelectParking();
            if (parking == null)
            {
                Console.ReadKey();
                return;
            }

            Console.Write("Въведете регистрационния номер на превозното средство: ");
            string plate = Console.ReadLine();

            if (parking.Vehicles.Any(v => v.Equals(plate, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Грешка: Това превозно средство вече е паркирано в този паркинг!");
                Console.ReadKey();
                return;
            }

            if (parking.AvailableSpaces <= 0)
            {
                Console.WriteLine("Няма свободни паркоместа в този паркинг!");
                Console.ReadKey();
                return;
            }

            parking.Vehicles.Add(plate);
            parking.AvailableSpaces--;

            SaveDataToFile();

            Console.WriteLine("\nПревозното средство е регистрирано успешно и файлът е актуализиран!");
            Console.ReadKey();
        }
        static void RemoveVehicle()
        {
            Console.WriteLine("--- Напускане на паркинг от превозно средство ---");

            Parking parking = SelectParking();
            if (parking == null)
            {
                Console.ReadKey();
                return;
            }

            Console.Write("Въведете регистрационния номер на превозното средство: ");
            string plate = Console.ReadLine();

            string vehicle = parking.Vehicles.FirstOrDefault(v => v.Equals(plate, StringComparison.OrdinalIgnoreCase));

            if (vehicle == null)
            {
                Console.WriteLine("Това превозно средство не е намерено в избрания паркинг.");
                Console.ReadKey();
                return;
            }

            parking.Vehicles.Remove(vehicle);
            if (parking.AvailableSpaces < parking.TotalSpaces)
            {
                parking.AvailableSpaces++;
            }

            SaveDataToFile();

            Console.WriteLine("\nПревозното средство напусна паркинга успешно и файлът е актуализиран!");
            Console.ReadKey();
        }
        static void CheckAvailability()
        {
            Console.WriteLine("--- Проверка на наличността на паркоместа ---");
            Console.Write("Въведете ID или местоположение на паркинга: ");
            string input = Console.ReadLine();

            Parking parking = allParkings.FirstOrDefault(p =>
                p.ParkingID.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                p.Location.Equals(input, StringComparison.OrdinalIgnoreCase));

            if (parking != null)
            {
                Console.WriteLine($"\nПаркинг: {parking.Location} (ID: {parking.ParkingID})");
                Console.WriteLine($"Свободни паркоместа: {parking.AvailableSpaces} от общо {parking.TotalSpaces}");
            }
            else
            {
                Console.WriteLine("Паркинг с такова ID или местоположение не е намерен.");
            }
            Console.ReadKey();
        }

        static void ShowAllParkings()
        {
            Console.WriteLine("--- Списък на всички паркинги ---");

            if (allParkings.Count == 0)
            {
                Console.WriteLine("В момента няма регистрирани паркинги.");
            }
            else
            {
                foreach (var parking in allParkings)
                {
                    Console.WriteLine(parking.ToString());
                    Console.WriteLine(new string('-', 60)); // Визуален разделител
                }
            }
            Console.ReadKey();
        }
    }
}
