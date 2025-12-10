/*
 * File Name: HallRepository.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Provides MySQL-based data access for hall records, including methods to
 *   retrieve, add, update, and delete hall information.
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MovieTheaterApp.Models;
using MovieTheaterApp.Database;

namespace MovieTheaterApp.Repositories
{
    public class HallRepository
    {
        public List<Hall> GetAllHalls()
        {
            var halls = new List<Hall>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT HallID, HallName, Capacity, Description FROM Hall",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                halls.Add(new Hall
                {
                    HallID = reader.GetInt32("HallID"),
                    HallName = reader.GetString("HallName"),
                    Capacity = reader.GetInt32("Capacity"),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString("Description")
                });
            }

            return halls;
        }

        public Hall? GetHallById(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT HallID, HallName, Capacity, Description FROM Hall WHERE HallID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Hall
                {
                    HallID = reader.GetInt32("HallID"),
                    HallName = reader.GetString("HallName"),
                    Capacity = reader.GetInt32("Capacity"),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString("Description")
                };
            }

            return null;
        }

        public int AddHall(Hall hall)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"INSERT INTO Hall (HallName, Capacity, Description) 
                  VALUES (@hallName, @capacity, @description);
                  SELECT LAST_INSERT_ID();",
                connection);

            command.Parameters.AddWithValue("@hallName", hall.HallName ?? string.Empty);
            command.Parameters.AddWithValue("@capacity", hall.Capacity);
            command.Parameters.AddWithValue("@description", hall.Description ?? string.Empty);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool UpdateHall(Hall hall)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"UPDATE Hall SET HallName = @hallName, Capacity = @capacity, Description = @description 
                  WHERE HallID = @id",
                connection);

            command.Parameters.AddWithValue("@id", hall.HallID);
            command.Parameters.AddWithValue("@hallName", hall.HallName ?? string.Empty);
            command.Parameters.AddWithValue("@capacity", hall.Capacity);
            command.Parameters.AddWithValue("@description", hall.Description ?? string.Empty);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteHall(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "DELETE FROM Hall WHERE HallID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }
    }
}