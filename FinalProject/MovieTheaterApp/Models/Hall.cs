/*
 * File Name: Hall.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Defines the Hall model used to store hall details including name,
 *   seating capacity, and an optional description.
 */

namespace MovieTheaterApp.Models
{
    public class Hall
    {
        public int HallID { get; set; }
        public string? HallName { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
    }
}