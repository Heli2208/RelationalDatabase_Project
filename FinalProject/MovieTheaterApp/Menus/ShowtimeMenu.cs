/*
 * File Name: ShowtimeMenu.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   This file implements the showtime management console menu, providing
 *   options to view, add, update, delete showtimes and display available seats.
 */

using System;
using System.Collections.Generic;
using MovieTheaterApp.Models;
using MovieTheaterApp.Repositories;

namespace MovieTheaterApp.Menus
{
    public class ShowtimeMenu
    {
        private readonly ShowtimeRepository showtimeRepo = new();
        private readonly MovieRepository movieRepo = new();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SHOWTIME MANAGEMENT ===");
                Console.WriteLine("1. View All Showtimes");
                Console.WriteLine("2. Add New Showtime");
                Console.WriteLine("3. Update Showtime");
                Console.WriteLine("4. Delete Showtime");
                Console.WriteLine("5. View Available Seats");
                Console.WriteLine("6. Return to Main Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        ViewAllShowtimes(); 
                        break;
                    case "2": 
                        AddShowtime(); 
                        break;
                    case "3": 
                        UpdateShowtime(); 
                        break;
                    case "4": 
                        DeleteShowtime(); 
                        break;
                    case "5": 
                        ViewAvailableSeats(); 
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

        private void ViewAllShowtimes()
        {
            Console.WriteLine("\n--- ALL SHOWTIMES ---");
            var showtimes = showtimeRepo.GetAllShowtimes();

            if (showtimes.Count == 0)
            {
                Console.WriteLine("No showtimes found.");
                return;
            }

            foreach (var showtime in showtimes)
            {
                Console.WriteLine($"ID: {showtime.ShowtimeID} | Movie: {showtime.Movie?.Title} | " +
                                $"Hall: {showtime.Hall?.HallName} | Time: {showtime.StartDateTime:yyyy-MM-dd HH:mm} | " +
                                $"Price: ${showtime.BaseTicketPrice} | Status: {showtime.Status}");
            }
            Console.WriteLine($"\nTotal: {showtimes.Count} showtimes");
        }

        private void AddShowtime()
        {
            Console.WriteLine("\n--- ADD NEW SHOWTIME ---");
            var showtime = new Showtime();

            Console.WriteLine("\nAvailable Movies:");
            var movies = movieRepo.GetAllMovies();
            foreach (var movie in movies)
            {
                Console.WriteLine($"ID: {movie.MovieID} - {movie.Title}");
            }

            Console.Write("\nSelect Movie ID: ");
            if (!int.TryParse(Console.ReadLine(), out int movieId))
            {
                Console.WriteLine("Invalid Movie ID!");
                return;
            }
            showtime.MovieID = movieId;

            Console.Write("Hall ID: ");
            if (!int.TryParse(Console.ReadLine(), out int hallId))
            {
                Console.WriteLine("Invalid Hall ID!");
                return;
            }
            showtime.HallID = hallId;

            Console.Write("Start Date & Time (yyyy-mm-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startTime))
            {
                Console.WriteLine("Invalid date/time!");
                return;
            }
            showtime.StartDateTime = startTime;

            Console.Write("Base Ticket Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid price!");
                return;
            }
            showtime.BaseTicketPrice = price;

            Console.Write("Status: ");
            showtime.Status = Console.ReadLine();

            try
            {
                int newId = showtimeRepo.AddShowtime(showtime);
                Console.WriteLine($"Showtime added successfully! ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding showtime: {ex.Message}");
            }
        }

        private void UpdateShowtime()
        {
            Console.Write("\nEnter Showtime ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var showtime = showtimeRepo.GetShowtimeById(id);
            if (showtime == null)
            {
                Console.WriteLine("Showtime not found!");
                return;
            }

            Console.WriteLine($"Current: Movie ID: {showtime.MovieID} | Time: {showtime.StartDateTime} | Status: {showtime.Status}");

            Console.WriteLine("\nAvailable Movies:");
            var movies = movieRepo.GetAllMovies();
            foreach (var movie in movies)
            {
                Console.WriteLine($"ID: {movie.MovieID} - {movie.Title}");
            }

            Console.Write("New Movie ID (press Enter to keep current): ");
            var movieIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(movieIdInput) && int.TryParse(movieIdInput, out int movieId))
                showtime.MovieID = movieId;

            Console.Write("New Hall ID (press Enter to keep current): ");
            var hallIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(hallIdInput) && int.TryParse(hallIdInput, out int hallId))
                showtime.HallID = hallId;

            Console.Write("New Start Date & Time (yyyy-mm-dd HH:mm, press Enter to keep current): ");
            var startTimeInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(startTimeInput) && DateTime.TryParse(startTimeInput, out DateTime startTime))
                showtime.StartDateTime = startTime;

            Console.Write("New Base Ticket Price (press Enter to keep current): ");
            var priceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal price))
                showtime.BaseTicketPrice = price;

            Console.Write("New Status (press Enter to keep current): ");
            var status = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(status))
                showtime.Status = status;

            try
            {
                if (showtimeRepo.UpdateShowtime(showtime))
                    Console.WriteLine("Showtime updated successfully!");
                else
                    Console.WriteLine("Update failed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating showtime: {ex.Message}");
            }
        }

        private void DeleteShowtime()
        {
            Console.Write("\nEnter Showtime ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            Console.Write($"Are you sure you want to delete showtime ID {id}? (y/n): ");
            if (Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
            {
                try
                {
                    if (showtimeRepo.DeleteShowtime(id))
                        Console.WriteLine("Showtime deleted successfully!");
                    else
                        Console.WriteLine("Showtime not found!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting showtime: {ex.Message}");
                }
            }
        }

        private void ViewAvailableSeats()
        {
            Console.Write("\nEnter Showtime ID: ");
            if (!int.TryParse(Console.ReadLine(), out int showtimeId))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var seats = showtimeRepo.GetAvailableSeats(showtimeId);
            Console.WriteLine($"\n--- AVAILABLE SEATS ({seats.Count} seats) ---");

            if (seats.Count == 0)
            {
                Console.WriteLine("No available seats for this showtime.");
                return;
            }

            var seatsByRow = new Dictionary<string, List<Seat>>();
            foreach (var seat in seats)
            {
                if (!seatsByRow.ContainsKey(seat.RowLabel ?? ""))
                {
                    seatsByRow[seat.RowLabel ?? ""] = new List<Seat>();
                }
                seatsByRow[seat.RowLabel ?? ""].Add(seat);
            }

            foreach (var row in seatsByRow)
            {
                Console.Write($"Row {row.Key}: ");
                foreach (var seat in row.Value)
                {
                    Console.Write($"{seat.SeatNumber} ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n--- Detailed Seat List ---");
            foreach (var seat in seats)
            {
                Console.WriteLine($"Seat ID: {seat.SeatID} | Seat: {seat.SeatLabel} | Type: {seat.SeatType}");
            }
        }
    }
}