/*
 * File Name: SeatMenu.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   This file implements the console menu for managing seats, including viewing,
 *   adding, updating, deleting, and searching seat records by hall or seat ID.
 */

using System;
using System.Collections.Generic;
using MovieTheaterApp.Models;
using MovieTheaterApp.Repositories;

namespace MovieTheaterApp.Menus
{
    public class SeatMenu
    {
        private readonly SeatRepository seatRepo = new();
        private readonly HallRepository hallRepo = new();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SEAT MANAGEMENT ===");
                Console.WriteLine("1. View All Seats");
                Console.WriteLine("2. View Seats by Hall");
                Console.WriteLine("3. Add New Seat");
                Console.WriteLine("4. Update Seat");
                Console.WriteLine("5. Delete Seat");
                Console.WriteLine("6. Search Seat by ID");
                Console.WriteLine("7. Return to Main Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        ViewAllSeats(); 
                        break;
                    case "2": 
                        ViewSeatsByHall(); 
                        break;
                    case "3": 
                        AddSeat(); 
                        break;
                    case "4": 
                        UpdateSeat(); 
                        break;
                    case "5": 
                        DeleteSeat(); 
                        break;
                    case "6": 
                        SearchSeat(); 
                        break;
                    case "7": 
                        return;
                    default: 
                        Console.WriteLine("Invalid option!"); 
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void ViewAllSeats()
        {
            Console.WriteLine("\n--- ALL SEATS ---");
            var seats = seatRepo.GetAllSeats();

            if (seats.Count == 0)
            {
                Console.WriteLine("No seats found.");
                return;
            }

            foreach (var seat in seats)
            {
                Console.WriteLine($"ID: {seat.SeatID} | Hall: {seat.HallID} | Seat: {seat.SeatLabel} | " +
                                $"Type: {seat.SeatType} | Active: {seat.IsActive}");
            }
            Console.WriteLine($"\nTotal: {seats.Count} seats");
        }

        private void ViewSeatsByHall()
        {
            Console.Write("\nEnter Hall ID: ");
            if (!int.TryParse(Console.ReadLine(), out int hallId))
            {
                Console.WriteLine("Invalid Hall ID!");
                return;
            }

            var seats = seatRepo.GetSeatsByHall(hallId);
            Console.WriteLine($"\n--- SEATS FOR HALL {hallId} ---");

            if (seats.Count == 0)
            {
                Console.WriteLine("No seats found for this hall.");
                return;
            }

            foreach (var seat in seats)
            {
                Console.WriteLine($"ID: {seat.SeatID} | Seat: {seat.SeatLabel} | Type: {seat.SeatType} | Active: {seat.IsActive}");
            }
            Console.WriteLine($"\nTotal: {seats.Count} seats");
        }

        private void AddSeat()
        {
            Console.WriteLine("\n--- ADD NEW SEAT ---");
            var seat = new Seat();

            Console.WriteLine("\nAvailable Halls:");
            var halls = hallRepo.GetAllHalls();
            foreach (var hall in halls)
            {
                Console.WriteLine($"ID: {hall.HallID} - {hall.HallName}");
            }

            Console.Write("\nSelect Hall ID: ");
            if (!int.TryParse(Console.ReadLine(), out int hallId))
            {
                Console.WriteLine("Invalid Hall ID!");
                return;
            }
            seat.HallID = hallId;

            Console.Write("Row Label (e.g., A, B, C): ");
            seat.RowLabel = Console.ReadLine();

            Console.Write("Seat Number: ");
            if (!int.TryParse(Console.ReadLine(), out int seatNumber))
            {
                Console.WriteLine("Invalid seat number!");
                return;
            }
            seat.SeatNumber = seatNumber;

            Console.Write("Seat Type (e.g., Standard, Premium): ");
            seat.SeatType = Console.ReadLine();

            Console.Write("Is Active (1 for yes, 0 for no): ");
            if (!int.TryParse(Console.ReadLine(), out int isActive))
            {
                Console.WriteLine("Invalid input! Setting to active by default.");
                seat.IsActive = true;
            }
            else
            {
                seat.IsActive = isActive == 1;
            }

            try
            {
                int newId = seatRepo.AddSeat(seat);
                Console.WriteLine($"Seat added successfully! ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding seat: {ex.Message}");
            }
        }

        private void UpdateSeat()
        {
            Console.Write("\nEnter Seat ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var seat = seatRepo.GetSeatById(id);
            if (seat == null)
            {
                Console.WriteLine("Seat not found!");
                return;
            }

            Console.WriteLine($"Current: Hall: {seat.HallID} | Seat: {seat.SeatLabel} | Type: {seat.SeatType} | Active: {seat.IsActive}");

            Console.Write("New Hall ID (press Enter to keep current): ");
            var hallIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(hallIdInput) && int.TryParse(hallIdInput, out int hallId))
                seat.HallID = hallId;

            Console.Write("New Row Label: ");
            var rowLabel = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(rowLabel))
                seat.RowLabel = rowLabel;

            Console.Write("New Seat Number: ");
            var seatNumberInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(seatNumberInput) && int.TryParse(seatNumberInput, out int seatNumber))
                seat.SeatNumber = seatNumber;

            Console.Write("New Seat Type: ");
            var seatType = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(seatType))
                seat.SeatType = seatType;

            Console.Write("New Active Status (1 for yes, 0 for no, press Enter to keep current): ");
            var activeInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(activeInput) && int.TryParse(activeInput, out int isActive))
                seat.IsActive = isActive == 1;

            try
            {
                if (seatRepo.UpdateSeat(seat))
                    Console.WriteLine("Seat updated successfully!");
                else
                    Console.WriteLine("Update failed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating seat: {ex.Message}");
            }
        }

        private void DeleteSeat()
        {
            Console.Write("\nEnter Seat ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            Console.Write($"Are you sure you want to delete seat ID {id}? (y/n): ");
            if (Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
            {
                try
                {
                    if (seatRepo.DeleteSeat(id))
                        Console.WriteLine("Seat deleted successfully!");
                    else
                        Console.WriteLine("Seat not found!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting seat: {ex.Message}");
                }
            }
        }

        private void SearchSeat()
        {
            Console.Write("\nEnter Seat ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var seat = seatRepo.GetSeatById(id);
            if (seat == null)
            {
                Console.WriteLine("Seat not found!");
                return;
            }

            Console.WriteLine($"\n--- SEAT DETAILS ---");
            Console.WriteLine($"ID: {seat.SeatID}");
            Console.WriteLine($"Hall ID: {seat.HallID}");
            Console.WriteLine($"Row Label: {seat.RowLabel}");
            Console.WriteLine($"Seat Number: {seat.SeatNumber}");
            Console.WriteLine($"Seat Type: {seat.SeatType}");
            Console.WriteLine($"Is Active: {seat.IsActive}");
            Console.WriteLine($"Seat Label: {seat.SeatLabel}");
        }
    }
}