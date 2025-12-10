/*
 * File Name: MovieMenu.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   This file provides the console menu for managing movies, including viewing,
 *   adding, updating, deleting, and searching movie records.
 */

using System;
using System.Collections.Generic;
using MovieTheaterApp.Models;
using MovieTheaterApp.Repositories;

namespace MovieTheaterApp.Menus
{
    public class MovieMenu
    {
        private readonly MovieRepository movieRepo = new();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MOVIE MANAGEMENT ===");
                Console.WriteLine("1. View All Movies");
                Console.WriteLine("2. Add New Movie");
                Console.WriteLine("3. Update Movie");
                Console.WriteLine("4. Delete Movie");
                Console.WriteLine("5. Search Movie by ID");
                Console.WriteLine("6. Return to Main Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        ViewAllMovies(); 
                        break;
                    case "2": 
                        AddMovie(); 
                        break;
                    case "3": 
                        UpdateMovie(); 
                        break;
                    case "4": 
                        DeleteMovie(); 
                        break;
                    case "5": 
                        SearchMovie(); 
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

        private void ViewAllMovies()
        {
            Console.WriteLine("\n--- ALL MOVIES ---");
            var movies = movieRepo.GetAllMovies();

            if (movies.Count == 0)
            {
                Console.WriteLine("No movies found.");
                return;
            }

            foreach (var movie in movies)
            {
                Console.WriteLine($"ID: {movie.MovieID} | Title: {movie.Title} | " +
                                $"Genre: {movie.Genre} | Duration: {movie.DurationFormatted} | " +
                                $"Rating: {movie.Rating} | Status: {movie.Status}");
            }
            Console.WriteLine($"\nTotal: {movies.Count} movies");
        }

        private void AddMovie()
        {
            Console.WriteLine("\n--- ADD NEW MOVIE ---");
            var movie = new Movie();

            Console.Write("Title: ");
            movie.Title = Console.ReadLine();

            Console.Write("Genre: ");
            movie.Genre = Console.ReadLine();

            Console.Write("Duration (minutes): ");
            if (!int.TryParse(Console.ReadLine(), out int duration))
            {
                Console.WriteLine("Invalid duration!");
                return;
            }
            movie.DurationMinutes = duration;

            Console.Write("Language: ");
            movie.Language = Console.ReadLine();

            Console.Write("Rating: ");
            movie.Rating = Console.ReadLine();

            Console.Write("Release Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime releaseDate))
            {
                Console.WriteLine("Invalid date!");
                return;
            }
            movie.ReleaseDate = releaseDate;

            Console.Write("Status: ");
            movie.Status = Console.ReadLine();

            try
            {
                int newId = movieRepo.AddMovie(movie);
                Console.WriteLine($"Movie added successfully! ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding movie: {ex.Message}");
            }
        }

        private void UpdateMovie()
        {
            Console.Write("\nEnter Movie ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var movie = movieRepo.GetMovieById(id);
            if (movie == null)
            {
                Console.WriteLine("Movie not found!");
                return;
            }

            Console.WriteLine($"Current: {movie.Title} | Genre: {movie.Genre} | Status: {movie.Status}");

            Console.Write("New Title (press Enter to keep current): ");
            var title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
                movie.Title = title;

            Console.Write("New Genre: ");
            var genre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(genre))
                movie.Genre = genre;

            Console.Write("New Duration (minutes, press Enter to keep current): ");
            var durationInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(durationInput) && int.TryParse(durationInput, out int duration))
                movie.DurationMinutes = duration;

            Console.Write("New Language: ");
            var language = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(language))
                movie.Language = language;

            Console.Write("New Rating: ");
            var rating = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(rating))
                movie.Rating = rating;

            Console.Write("New Release Date (yyyy-mm-dd, press Enter to keep current): ");
            var releaseDateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(releaseDateInput) && DateTime.TryParse(releaseDateInput, out DateTime releaseDate))
                movie.ReleaseDate = releaseDate;

            Console.Write("New Status: ");
            var status = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(status))
                movie.Status = status;

            try
            {
                if (movieRepo.UpdateMovie(movie))
                    Console.WriteLine("Movie updated successfully!");
                else
                    Console.WriteLine("Update failed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating movie: {ex.Message}");
            }
        }

        private void DeleteMovie()
        {
            Console.Write("\nEnter Movie ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            Console.Write($"Are you sure you want to delete movie ID {id}? (y/n): ");
            if (Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
            {
                try
                {
                    if (movieRepo.DeleteMovie(id))
                        Console.WriteLine("Movie deleted successfully!");
                    else
                        Console.WriteLine("Movie not found!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting movie: {ex.Message}");
                }
            }
        }

        private void SearchMovie()
        {
            Console.Write("\nEnter Movie ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var movie = movieRepo.GetMovieById(id);
            if (movie == null)
            {
                Console.WriteLine("Movie not found!");
                return;
            }

            Console.WriteLine($"\n--- MOVIE DETAILS ---");
            Console.WriteLine($"ID: {movie.MovieID}");
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Genre: {movie.Genre}");
            Console.WriteLine($"Duration: {movie.DurationFormatted}");
            Console.WriteLine($"Language: {movie.Language}");
            Console.WriteLine($"Rating: {movie.Rating}");
            Console.WriteLine($"Release Date: {movie.ReleaseDate:yyyy-MM-dd}");
            Console.WriteLine($"Status: {movie.Status}");
        }
    }
}