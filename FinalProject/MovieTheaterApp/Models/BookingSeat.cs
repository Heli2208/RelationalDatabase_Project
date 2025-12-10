/*
 * File Name: BookingSeat.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Defines the BookingSeat model used to represent each individual seat
 *   booked under a booking, including seat ID and price at time of booking.
 */

namespace MovieTheaterApp.Models
{
    public class BookingSeat
    {
        public int BookingSeatID { get; set; }
        public int BookingID { get; set; }
        public int SeatID { get; set; }
        public decimal SeatPriceAtBooking { get; set; }
        public Seat? Seat { get; set; }
    }
}