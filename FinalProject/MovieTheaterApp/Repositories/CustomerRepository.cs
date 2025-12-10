/*
 * File Name: CustomerRepository.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Provides MySQL data access for customer records, including methods to
 *   retrieve, add, update, and delete customer information.
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MovieTheaterApp.Models;
using MovieTheaterApp.Database;

namespace MovieTheaterApp.Repositories
{
    public class CustomerRepository
    {
        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT CustomerID, FirstName, LastName, Email, Phone, CreatedDate FROM Customer",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerID = reader.GetInt32("CustomerID"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Email = reader.GetString("Email"),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? string.Empty : reader.GetString("Phone"),
                    CreatedDate = reader.GetDateTime("CreatedDate")
                });
            }

            return customers;
        }

        public Customer? GetCustomerById(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT CustomerID, FirstName, LastName, Email, Phone, CreatedDate FROM Customer WHERE CustomerID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Customer
                {
                    CustomerID = reader.GetInt32("CustomerID"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Email = reader.GetString("Email"),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? string.Empty : reader.GetString("Phone"),
                    CreatedDate = reader.GetDateTime("CreatedDate")
                };
            }

            return null;
        }

        public int AddCustomer(Customer customer)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"INSERT INTO Customer (FirstName, LastName, Email, Phone, CreatedDate) 
                  VALUES (@firstName, @lastName, @email, @phone, @createdDate);
                  SELECT LAST_INSERT_ID();",
                connection);

            command.Parameters.AddWithValue("@firstName", customer.FirstName ?? string.Empty);
            command.Parameters.AddWithValue("@lastName", customer.LastName ?? string.Empty);
            command.Parameters.AddWithValue("@email", customer.Email ?? string.Empty);
            command.Parameters.AddWithValue("@phone", customer.Phone ?? string.Empty);
            command.Parameters.AddWithValue("@createdDate", DateTime.Now);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool UpdateCustomer(Customer customer)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"UPDATE Customer SET FirstName = @firstName, LastName = @lastName, 
                  Email = @email, Phone = @phone WHERE CustomerID = @id",
                connection);

            command.Parameters.AddWithValue("@id", customer.CustomerID);
            command.Parameters.AddWithValue("@firstName", customer.FirstName ?? string.Empty);
            command.Parameters.AddWithValue("@lastName", customer.LastName ?? string.Empty);
            command.Parameters.AddWithValue("@email", customer.Email ?? string.Empty);
            command.Parameters.AddWithValue("@phone", customer.Phone ?? string.Empty);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteCustomer(int id)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "DELETE FROM Customer WHERE CustomerID = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }
    }
}