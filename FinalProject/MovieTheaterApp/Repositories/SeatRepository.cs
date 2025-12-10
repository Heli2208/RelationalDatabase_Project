/*
 * File Name: SeatRepository.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Provides MySQL data access operations for seat records, including methods
 *   to retrieve all seats, search by ID or hall, and perform add, update, and delete actions.
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MovieTheaterApp.Models;
using MovieTheaterApp.Database;

namespace MovieTheaterApp.Repositories
{
    public class SeatRepository
    {
        public List<Seat> GetAllSeats()
        {
            var seats = new List<Seat>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT SeatID, HallID, RowLabel, SeatNumber, SeatType, IsActive FROM Seat",
                connection);

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

        public Seat? GetSeatById(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT SeatID, HallID, RowLabel, SeatNumber, SeatType, IsActive FROM Seat WHERE SeatID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Seat
                {
                    SeatID = reader.GetInt32("SeatID"),
                    HallID = reader.GetInt32("HallID"),
                    RowLabel = reader.GetString("RowLabel"),
                    SeatNumber = reader.GetInt32("SeatNumber"),
                    SeatType = reader.IsDBNull(reader.GetOrdinal("SeatType")) ? string.Empty : reader.GetString("SeatType"),
                    IsActive = reader.GetBoolean("IsActive")
                };
            }

            return null;
        }

        public List<Seat> GetSeatsByHall(int hallId)
        {
            var seats = new List<Seat>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT SeatID, HallID, RowLabel, SeatNumber, SeatType, IsActive FROM Seat WHERE HallID = @hallId",
                connection);
            command.Parameters.AddWithValue("@hallId", hallId);

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

        public int AddSeat(Seat seat)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"INSERT INTO Seat (HallID, RowLabel, SeatNumber, SeatType, IsActive) 
                  VALUES (@hallId, @rowLabel, @seatNumber, @seatType, @isActive);
                  SELECT LAST_INSERT_ID();",
                connection);

            command.Parameters.AddWithValue("@hallId", seat.HallID);
            command.Parameters.AddWithValue("@rowLabel", seat.RowLabel ?? string.Empty);
            command.Parameters.AddWithValue("@seatNumber", seat.SeatNumber);
            command.Parameters.AddWithValue("@seatType", seat.SeatType ?? string.Empty);
            command.Parameters.AddWithValue("@isActive", seat.IsActive);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool UpdateSeat(Seat seat)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"UPDATE Seat SET HallID = @hallId, RowLabel = @rowLabel, SeatNumber = @seatNumber, 
                  SeatType = @seatType, IsActive = @isActive 
                  WHERE SeatID = @id",
                connection);

            command.Parameters.AddWithValue("@id", seat.SeatID);
            command.Parameters.AddWithValue("@hallId", seat.HallID);
            command.Parameters.AddWithValue("@rowLabel", seat.RowLabel ?? string.Empty);
            command.Parameters.AddWithValue("@seatNumber", seat.SeatNumber);
            command.Parameters.AddWithValue("@seatType", seat.SeatType ?? string.Empty);
            command.Parameters.AddWithValue("@isActive", seat.IsActive);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteSeat(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "DELETE FROM Seat WHERE SeatID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }
    }
}