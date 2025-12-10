/*
 * File Name: Booking.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Defines the Booking model used to store booking details including
 *   customer, showtime, seats, booking date, status, and total price.
 */

using System;
using System.Collections.Generic;

namespace MovieTheaterApp.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int ShowtimeID { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string? Status { get; set; }
        public Customer? Customer { get; set; }
        public Showtime? Showtime { get; set; }
        public List<BookingSeat> BookingSeats { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }
}