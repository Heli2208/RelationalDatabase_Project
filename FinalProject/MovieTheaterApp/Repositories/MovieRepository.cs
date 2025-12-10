/*
 * File Name: MovieRepository.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Provides MySQL data access for movie records, including methods to retrieve,
 *   add, update, and delete movies and handle all related database operations.
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MovieTheaterApp.Models;
using MovieTheaterApp.Database;

namespace MovieTheaterApp.Repositories
{
    public class MovieRepository
    {
        public List<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT MovieID, Title, Genre, DurationMinutes, Language, Rating, ReleaseDate, Status FROM Movie",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                movies.Add(new Movie
                {
                    MovieID = reader.GetInt32("MovieID"),
                    Title = reader.GetString("Title"),
                    Genre = reader.IsDBNull(reader.GetOrdinal("Genre")) ? string.Empty : reader.GetString("Genre"),
                    DurationMinutes = reader.GetInt32("DurationMinutes"),
                    Language = reader.IsDBNull(reader.GetOrdinal("Language")) ? string.Empty : reader.GetString("Language"),
                    Rating = reader.IsDBNull(reader.GetOrdinal("Rating")) ? string.Empty : reader.GetString("Rating"),
                    ReleaseDate = reader.IsDBNull(reader.GetOrdinal("ReleaseDate")) ? (DateTime?)null : reader.GetDateTime("ReleaseDate"),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString("Status")
                });
            }

            return movies;
        }

        public Movie? GetMovieById(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT MovieID, Title, Genre, DurationMinutes, Language, Rating, ReleaseDate, Status FROM Movie WHERE MovieID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Movie
                {
                    MovieID = reader.GetInt32("MovieID"),
                    Title = reader.GetString("Title"),
                    Genre = reader.IsDBNull(reader.GetOrdinal("Genre")) ? string.Empty : reader.GetString("Genre"),
                    DurationMinutes = reader.GetInt32("DurationMinutes"),
                    Language = reader.IsDBNull(reader.GetOrdinal("Language")) ? string.Empty : reader.GetString("Language"),
                    Rating = reader.IsDBNull(reader.GetOrdinal("Rating")) ? string.Empty : reader.GetString("Rating"),
                    ReleaseDate = reader.IsDBNull(reader.GetOrdinal("ReleaseDate")) ? (DateTime?)null : reader.GetDateTime("ReleaseDate"),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString("Status")
                };
            }

            return null;
        }

        public int AddMovie(Movie movie)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"INSERT INTO Movie (Title, Genre, DurationMinutes, Language, Rating, ReleaseDate, Status) 
                  VALUES (@title, @genre, @duration, @language, @rating, @releaseDate, @status);
                  SELECT LAST_INSERT_ID();",
                connection);

            command.Parameters.AddWithValue("@title", movie.Title ?? string.Empty);
            command.Parameters.AddWithValue("@genre", movie.Genre ?? string.Empty);
            command.Parameters.AddWithValue("@duration", movie.DurationMinutes);
            command.Parameters.AddWithValue("@language", movie.Language ?? string.Empty);
            command.Parameters.AddWithValue("@rating", movie.Rating ?? string.Empty);
            command.Parameters.AddWithValue("@releaseDate", movie.ReleaseDate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@status", movie.Status ?? string.Empty);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool UpdateMovie(Movie movie)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"UPDATE Movie SET Title = @title, Genre = @genre, DurationMinutes = @duration, 
                  Language = @language, Rating = @rating, ReleaseDate = @releaseDate, Status = @status 
                  WHERE MovieID = @id",
                connection);

            command.Parameters.AddWithValue("@id", movie.MovieID);
            command.Parameters.AddWithValue("@title", movie.Title ?? string.Empty);
            command.Parameters.AddWithValue("@genre", movie.Genre ?? string.Empty);
            command.Parameters.AddWithValue("@duration", movie.DurationMinutes);
            command.Parameters.AddWithValue("@language", movie.Language ?? string.Empty);
            command.Parameters.AddWithValue("@rating", movie.Rating ?? string.Empty);
            command.Parameters.AddWithValue("@releaseDate", movie.ReleaseDate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@status", movie.Status ?? string.Empty);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteMovie(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "DELETE FROM Movie WHERE MovieID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }
    }
}