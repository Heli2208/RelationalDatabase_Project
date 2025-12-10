/*
 * File Name: Movie.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Defines the Movie model, including title, genre, duration, language,
 *   rating, release date, and status, with a formatted duration property.
 */

using System;

namespace MovieTheaterApp.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int DurationMinutes { get; set; }
        public string? Language { get; set; }
        public string? Rating { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Status { get; set; }

        public string DurationFormatted => $"{DurationMinutes} min";
    }
}