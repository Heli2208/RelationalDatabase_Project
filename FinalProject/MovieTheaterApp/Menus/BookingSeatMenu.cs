/*
 * File Name: BookingSeatMenu.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   This file provides a console menu for viewing all booking seats or
 *   filtering booking seats by a specific booking ID.
 */

using System;
using System.Collections.Generic;
using MovieTheaterApp.Models;
using MovieTheaterApp.Repositories;

namespace MovieTheaterApp.Menus
{
    public class BookingSeatMenu
    {
        private readonly BookingSeatRepository bookingSeatRepo = new();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== BOOKING SEAT MANAGEMENT ===");
                Console.WriteLine("1. View All Booking Seats");
                Console.WriteLine("2. View Booking Seats by Booking ID");
                Console.WriteLine("3. Return to Main Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        ViewAllBookingSeats(); 
                        break;
                    case "2": 
                        ViewBookingSeatsByBookingId(); 
                        break;
                    case "3": 
                        return;
                    default: 
                        Console.WriteLine("Invalid option!"); 
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void ViewAllBookingSeats()
        {
            Console.WriteLine("\n--- ALL BOOKING SEATS ---");
            var bookingSeats = bookingSeatRepo.GetAllBookingSeats();

            if (bookingSeats.Count == 0)
            {
                Console.WriteLine("No booking seats found.");
                return;
            }

            foreach (var bookingSeat in bookingSeats)
            {
                Console.WriteLine($"BookingSeatID: {bookingSeat.BookingSeatID} | BookingID: {bookingSeat.BookingID} | " +
                                $"Seat: {bookingSeat.Seat?.SeatLabel} | Price: ${bookingSeat.SeatPriceAtBooking}");
            }
            Console.WriteLine($"\nTotal: {bookingSeats.Count} booking seats");
        }

        private void ViewBookingSeatsByBookingId()
        {
            Console.Write("\nEnter Booking ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid Booking ID!");
                return;
            }

            var bookingSeats = bookingSeatRepo.GetBookingSeatsByBookingId(bookingId);
            Console.WriteLine($"\n--- BOOKING SEATS FOR BOOKING {bookingId} ---");

            if (bookingSeats.Count == 0)
            {
                Console.WriteLine("No booking seats found for this booking.");
                return;
            }

            foreach (var bookingSeat in bookingSeats)
            {
                Console.WriteLine($"BookingSeatID: {bookingSeat.BookingSeatID} | Seat: {bookingSeat.Seat?.SeatLabel} | Price: ${bookingSeat.SeatPriceAtBooking}");
            }
            Console.WriteLine($"\nTotal: {bookingSeats.Count} booking seats");
        }
    }
}