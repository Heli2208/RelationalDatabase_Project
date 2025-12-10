/*
 * File Name: Showtime.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Defines the Showtime model, representing scheduled movie showtimes
 *   including movie, hall, start time, ticket price, and status.
 */

using System;

namespace MovieTheaterApp.Models
{
    public class Showtime
    {
        public int ShowtimeID { get; set; }
        public int MovieID { get; set; }
        public int HallID { get; set; }
        public DateTime StartDateTime { get; set; }
        public decimal BaseTicketPrice { get; set; }
        public string? Status { get; set; }
        public Movie? Movie { get; set; }
        public Hall? Hall { get; set; }
    }
}