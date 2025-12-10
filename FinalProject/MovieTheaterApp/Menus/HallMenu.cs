/*
 * File Name: HallMenu.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   This file provides the console menu for managing halls, including viewing,
 *   adding, updating, deleting, and searching hall records.
 */

using System;
using System.Collections.Generic;
using MovieTheaterApp.Models;
using MovieTheaterApp.Repositories;

namespace MovieTheaterApp.Menus
{
    public class HallMenu
    {
        private readonly HallRepository hallRepo = new();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== HALL MANAGEMENT ===");
                Console.WriteLine("1. View All Halls");
                Console.WriteLine("2. Add New Hall");
                Console.WriteLine("3. Update Hall");
                Console.WriteLine("4. Delete Hall");
                Console.WriteLine("5. Search Hall by ID");
                Console.WriteLine("6. Return to Main Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        ViewAllHalls(); 
                        break;
                    case "2": 
                        AddHall(); 
                        break;
                    case "3": 
                        UpdateHall(); 
                        break;
                    case "4": 
                        DeleteHall(); 
                        break;
                    case "5": 
                        SearchHall(); 
                        break;
                    case "6": 
                        return;
                    default: 
                        Console.WriteLine("Invalid option!"); 
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void ViewAllHalls()
        {
            Console.WriteLine("\n--- ALL HALLS ---");
            var halls = hallRepo.GetAllHalls();

            if (halls.Count == 0)
            {
                Console.WriteLine("No halls found.");
                return;
            }

            foreach (var hall in halls)
            {
                Console.WriteLine($"ID: {hall.HallID} | Name: {hall.HallName} | " +
                                $"Capacity: {hall.Capacity} | Description: {hall.Description}");
            }
            Console.WriteLine($"\nTotal: {halls.Count} halls");
        }

        private void AddHall()
        {
            Console.WriteLine("\n--- ADD NEW HALL ---");
            var hall = new Hall();

            Console.Write("Hall Name: ");
            hall.HallName = Console.ReadLine();

            Console.Write("Capacity: ");
            if (!int.TryParse(Console.ReadLine(), out int capacity))
            {
                Console.WriteLine("Invalid capacity!");
                return;
            }
            hall.Capacity = capacity;

            Console.Write("Description: ");
            hall.Description = Console.ReadLine();

            try
            {
                int newId = hallRepo.AddHall(hall);
                Console.WriteLine($"Hall added successfully! ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding hall: {ex.Message}");
            }
        }

        private void UpdateHall()
        {
            Console.Write("\nEnter Hall ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var hall = hallRepo.GetHallById(id);
            if (hall == null)
            {
                Console.WriteLine("Hall not found!");
                return;
            }

            Console.WriteLine($"Current: {hall.HallName} | Capacity: {hall.Capacity} | Description: {hall.Description}");

            Console.Write("New Hall Name (press Enter to keep current): ");
            var hallName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(hallName))
                hall.HallName = hallName;

            Console.Write("New Capacity (press Enter to keep current): ");
            var capacityInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(capacityInput) && int.TryParse(capacityInput, out int capacity))
                hall.Capacity = capacity;

            Console.Write("New Description (press Enter to keep current): ");
            var description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description))
                hall.Description = description;

            try
            {
                if (hallRepo.UpdateHall(hall))
                    Console.WriteLine("Hall updated successfully!");
                else
                    Console.WriteLine("Update failed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating hall: {ex.Message}");
            }
        }

        private void DeleteHall()
        {
            Console.Write("\nEnter Hall ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            Console.Write($"Are you sure you want to delete hall ID {id}? (y/n): ");
            if (Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
            {
                try
                {
                    if (hallRepo.DeleteHall(id))
                        Console.WriteLine("Hall deleted successfully!");
                    else
                        Console.WriteLine("Hall not found!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting hall: {ex.Message}");
                }
            }
        }

        private void SearchHall()
        {
            Console.Write("\nEnter Hall ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var hall = hallRepo.GetHallById(id);
            if (hall == null)
            {
                Console.WriteLine("Hall not found!");
                return;
            }

            Console.WriteLine($"\n--- HALL DETAILS ---");
            Console.WriteLine($"ID: {hall.HallID}");
            Console.WriteLine($"Name: {hall.HallName}");
            Console.WriteLine($"Capacity: {hall.Capacity}");
            Console.WriteLine($"Description: {hall.Description}");
        }
    }
}