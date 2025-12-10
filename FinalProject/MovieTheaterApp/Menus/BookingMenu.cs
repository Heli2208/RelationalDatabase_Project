/*
 * File Name: BookingMenu.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   This file implements the console menu for managing bookings, including
 *   viewing, creating, cancelling, and deleting bookings using repository classes.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MovieTheaterApp.Models;
using MovieTheaterApp.Repositories;

namespace MovieTheaterApp.Menus
{
    public class BookingMenu
    {
        private readonly BookingRepository bookingRepo = new();
        private readonly ShowtimeRepository showtimeRepo = new();
        private readonly CustomerRepository customerRepo = new();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== BOOKING MANAGEMENT ===");
                Console.WriteLine("1. View All Bookings");
                Console.WriteLine("2. Create New Booking");
                Console.WriteLine("3. View Booking Details");
                Console.WriteLine("4. Cancel Booking");
                Console.WriteLine("5. Delete Booking");
                Console.WriteLine("6. Return to Main Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        ViewAllBookings(); 
                        break;
                    case "2": 
                        CreateBooking(); 
                        break;
                    case "3": 
                        ViewBookingDetails(); 
                        break;
                    case "4": 
                        CancelBooking(); 
                        break;
                    case "5": 
                        DeleteBooking(); 
                        break;
                    case "6": 
                        return;
                    default: Console.WriteLine("Invalid option!"); 
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void ViewAllBookings()
        {
            Console.WriteLine("\n--- ALL BOOKINGS ---");
            var bookings = bookingRepo.GetAllBookings();

            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            foreach (var booking in bookings)
            {
                Console.WriteLine($"ID: {booking.BookingID} | Customer: {booking.Customer?.FullName} | " +
                                $"Movie: {booking.Showtime?.Movie?.Title} | Time: {booking.Showtime?.StartDateTime:yyyy-MM-dd HH:mm} | " +
                                $"Status: {booking.Status} | Booked: {booking.BookingDateTime:yyyy-MM-dd HH:mm}");
            }
            Console.WriteLine($"\nTotal: {bookings.Count} bookings");
        }

        private void CreateBooking()
        {
            Console.WriteLine("\n--- CREATE NEW BOOKING ---");

            Console.WriteLine("\nAvailable Showtimes:");
            var showtimes = showtimeRepo.GetAllShowtimes();
            foreach (var showtime in showtimes)
            {
                Console.WriteLine($"ID: {showtime.ShowtimeID} | Movie: {showtime.Movie?.Title} | " +
                                $"Time: {showtime.StartDateTime:yyyy-MM-dd HH:mm} | Price: ${showtime.BaseTicketPrice}");
            }

            Console.Write("\nSelect Showtime ID: ");
            if (!int.TryParse(Console.ReadLine(), out int showtimeId))
            {
                Console.WriteLine("Invalid Showtime ID!");
                return;
            }

            Console.WriteLine("\nAvailable Customers:");
            var customers = customerRepo.GetAllCustomers();
            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.CustomerID} | Name: {customer.FullName}");
            }

            Console.Write("\nSelect Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID!");
                return;
            }

            var availableSeats = showtimeRepo.GetAvailableSeats(showtimeId);
            Console.WriteLine($"\nAvailable Seats ({availableSeats.Count} seats):");
            foreach (var seat in availableSeats)
            {
                Console.WriteLine($"ID: {seat.SeatID} | Seat: {seat.SeatLabel} | Type: {seat.SeatType}");
            }

            if (availableSeats.Count == 0)
            {
                Console.WriteLine("No available seats for this showtime.");
                return;
            }

            Console.Write("\nEnter seat IDs separated by commas: ");
            var seatInput = Console.ReadLine();
            var seatIds = new List<int>();

            foreach (var seatStr in seatInput?.Split(',') ?? Array.Empty<string>())
            {
                if (int.TryParse(seatStr.Trim(), out int seatId))
                {
                    seatIds.Add(seatId);
                }
            }

            if (seatIds.Count == 0)
            {
                Console.WriteLine("No valid seats selected!");
                return;
            }

            Console.WriteLine($"\nBooking Summary:");
            Console.WriteLine($"- Showtime: {showtimeId}");
            Console.WriteLine($"- Customer: {customerId}");
            Console.WriteLine($"- Seats: {string.Join(", ", seatIds)}");
            Console.Write("\nConfirm booking? (y/n): ");

            if (Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
            {
                try
                {
                    var booking = new Booking
                    {
                        CustomerID = customerId,
                        ShowtimeID = showtimeId
                    };

                    int bookingId = bookingRepo.CreateBookingWithSeats(booking, seatIds);
                    Console.WriteLine($"Booking created successfully! Booking ID: {bookingId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating booking: {ex.Message}");
                }
            }
        }

        private void ViewBookingDetails()
        {
            Console.Write("\nEnter Booking ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var booking = bookingRepo.GetBookingWithDetails(bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found!");
                return;
            }

            Console.WriteLine($"\n--- BOOKING DETAILS ---");
            Console.WriteLine($"Booking ID: {booking.BookingID}");
            Console.WriteLine($"Status: {booking.Status}");
            Console.WriteLine($"Booking Date: {booking.BookingDateTime:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"\nCustomer: {booking.Customer?.FullName}");
            Console.WriteLine($"Email: {booking.Customer?.Email}");
            Console.WriteLine($"\nShowtime: {booking.Showtime?.Movie?.Title}");
            Console.WriteLine($"Hall: {booking.Showtime?.Hall?.HallName}");
            Console.WriteLine($"Time: {booking.Showtime?.StartDateTime:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Base Price: ${booking.Showtime?.BaseTicketPrice}");
            Console.WriteLine($"\nSeats Booked:");

            foreach (var bookingSeat in booking.BookingSeats)
            {
                Console.WriteLine($"- Seat: {bookingSeat.Seat?.SeatLabel} | Price: ${bookingSeat.SeatPriceAtBooking}");
            }

            Console.WriteLine($"\nTotal Price: ${booking.TotalPrice}");
        }

        private void CancelBooking()
        {
            Console.Write("\nEnter Booking ID to cancel: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            Console.Write($"Are you sure you want to cancel booking ID {bookingId}? (y/n): ");
            if (Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
            {
                try
                {
                    if (bookingRepo.CancelBooking(bookingId))
                        Console.WriteLine("Booking cancelled successfully!");
                    else
                        Console.WriteLine("Booking not found!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error cancelling booking: {ex.Message}");
                }
            }
        }

        private void DeleteBooking()
        {
            Console.Write("\nEnter Booking ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            Console.Write($"Are you sure you want to DELETE booking ID {bookingId}? This cannot be undone! (y/n): ");
            if (Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
            {
                try
                {
                    if (bookingRepo.DeleteBooking(bookingId))
                        Console.WriteLine("Booking deleted successfully!");
                    else
                        Console.WriteLine("Booking not found!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting booking: {ex.Message}");
                }
            }
        }
    }
}