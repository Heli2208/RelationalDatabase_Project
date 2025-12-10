/*
 * File Name: DbHelper.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   This file provides helper methods for creating and testing MySQL database
 *   connections used throughout the Movie Theater application.
 */

using System;
using MySql.Data.MySqlClient;

namespace MovieTheaterApp.Database
{
    public static class DbHelper
    {
        private static readonly string connectionString = "Server=localhost;Database=movietheaterdb;Uid=root;Pwd=PinuVijay@2208;Port=3306;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                Console.WriteLine($"Connection string used: {connectionString.Replace("your_password_here", "*****")}");
                return false;
            }
        }
    }
}