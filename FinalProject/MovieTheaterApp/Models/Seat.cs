/*
 * File Name: Seat.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Defines the Seat model, storing hall association, row, seat number,
 *   seat type, activation status, and a combined seat label.
 */

namespace MovieTheaterApp.Models
{
    public class Seat
    {
        public int SeatID { get; set; }
        public int HallID { get; set; }
        public string? RowLabel { get; set; }
        public int SeatNumber { get; set; }
        public string? SeatType { get; set; }
        public bool IsActive { get; set; }

        public string SeatLabel => $"{RowLabel}{SeatNumber}";
    }
}