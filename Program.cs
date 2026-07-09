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
