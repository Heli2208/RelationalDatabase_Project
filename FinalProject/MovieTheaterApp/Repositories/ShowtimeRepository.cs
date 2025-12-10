/*
 * File Name: ShowtimeRepository.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Provides MySQL data access for showtime records, including operations to
 *   retrieve, add, update, and delete showtimes, as well as fetch available seats
 *   for a specific showtime.
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MovieTheaterApp.Models;
using MovieTheaterApp.Database;

namespace MovieTheaterApp.Repositories
{
    public class ShowtimeRepository
    {
        public List<Showtime> GetAllShowtimes()
        {
            var showtimes = new List<Showtime>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT s.ShowtimeID, s.MovieID, s.HallID, s.StartDateTime, s.BaseTicketPrice, s.Status,
                         m.Title AS MovieTitle, m.DurationMinutes, h.HallName
                  FROM Showtime s
                  JOIN Movie m ON s.MovieID = m.MovieID
                  JOIN Hall h ON s.HallID = h.HallID
                  ORDER BY s.StartDateTime",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                showtimes.Add(new Showtime
                {
                    ShowtimeID = reader.GetInt32("ShowtimeID"),
                    MovieID = reader.GetInt32("MovieID"),
                    HallID = reader.GetInt32("HallID"),
                    StartDateTime = reader.GetDateTime("StartDateTime"),
                    BaseTicketPrice = reader.GetDecimal("BaseTicketPrice"),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString("Status"),
                    Movie = new Movie
                    {
                        Title = reader.GetString("MovieTitle"),
                        DurationMinutes = reader.GetInt32("DurationMinutes")
                    },
                    Hall = new Hall
                    {
                        HallName = reader.GetString("HallName")
                    }
                });
            }

            return showtimes;
        }

        public Showtime? GetShowtimeById(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT s.ShowtimeID, s.MovieID, s.HallID, s.StartDateTime, s.BaseTicketPrice, s.Status,
                         m.Title AS MovieTitle, m.DurationMinutes, h.HallName
                  FROM Showtime s
                  JOIN Movie m ON s.MovieID = m.MovieID
                  JOIN Hall h ON s.HallID = h.HallID
                  WHERE s.ShowtimeID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Showtime
                {
                    ShowtimeID = reader.GetInt32("ShowtimeID"),
                    MovieID = reader.GetInt32("MovieID"),
                    HallID = reader.GetInt32("HallID"),
                    StartDateTime = reader.GetDateTime("StartDateTime"),
                    BaseTicketPrice = reader.GetDecimal("BaseTicketPrice"),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString("Status"),
                    Movie = new Movie
                    {
                        Title = reader.GetString("MovieTitle"),
                        DurationMinutes = reader.GetInt32("DurationMinutes")
                    },
                    Hall = new Hall
                    {
                        HallName = reader.GetString("HallName")
                    }
                };
            }

            return null;
        }

        public int AddShowtime(Showtime showtime)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"INSERT INTO Showtime (MovieID, HallID, StartDateTime, BaseTicketPrice, Status) 
                  VALUES (@movieId, @hallId, @startDateTime, @basePrice, @status);
                  SELECT LAST_INSERT_ID();",
                connection);

            command.Parameters.AddWithValue("@movieId", showtime.MovieID);
            command.Parameters.AddWithValue("@hallId", showtime.HallID);
            command.Parameters.AddWithValue("@startDateTime", showtime.StartDateTime);
            command.Parameters.AddWithValue("@basePrice", showtime.BaseTicketPrice);
            command.Parameters.AddWithValue("@status", showtime.Status ?? string.Empty);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool UpdateShowtime(Showtime showtime)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"UPDATE Showtime SET MovieID = @movieId, HallID = @hallId, 
                  StartDateTime = @startDateTime, BaseTicketPrice = @basePrice, Status = @status 
                  WHERE ShowtimeID = @id",
                connection);

            command.Parameters.AddWithValue("@id", showtime.ShowtimeID);
            command.Parameters.AddWithValue("@movieId", showtime.MovieID);
            command.Parameters.AddWithValue("@hallId", showtime.HallID);
            command.Parameters.AddWithValue("@startDateTime", showtime.StartDateTime);
            command.Parameters.AddWithValue("@basePrice", showtime.BaseTicketPrice);
            command.Parameters.AddWithValue("@status", showtime.Status ?? string.Empty);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteShowtime(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "DELETE FROM Showtime WHERE ShowtimeID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }

        public List<Seat> GetAvailableSeats(int showtimeId)
        {
            var seats = new List<Seat>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT s.SeatID, s.HallID, s.RowLabel, s.SeatNumber, s.SeatType, s.IsActive
                  FROM Seat s
                  WHERE s.HallID = (SELECT HallID FROM Showtime WHERE ShowtimeID = @showtimeId)
                    AND s.IsActive = 1
                    AND s.SeatID NOT IN (
                        SELECT bs.SeatID 
                        FROM BookingSeat bs
                        JOIN Booking b ON bs.BookingID = b.BookingID
                        WHERE b.ShowtimeID = @showtimeId AND b.Status = 'Confirmed'
                    )",
                connection);
            command.Parameters.AddWithValue("@showtimeId", showtimeId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                seats.Add(new Seat
                {
                    SeatID = reader.GetInt32("SeatID"),
                    HallID = reader.GetInt32("HallID"),
                    RowLabel = reader.GetString("RowLabel"),
                    SeatNumber = reader.GetInt32("SeatNumber"),
                    SeatType = reader.IsDBNull(reader.GetOrdinal("SeatType")) ? string.Empty : reader.GetString("SeatType"),
                    IsActive = reader.GetBoolean("IsActive")
                });
            }

            return seats;
        }
    }
}