/*
 * File Name: BookingSeatRepository.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Provides data access methods for booking seats, including retrieving all
 *   booking seats and filtering seats by booking ID using MySQL.
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MovieTheaterApp.Models;
using MovieTheaterApp.Database;

namespace MovieTheaterApp.Repositories
{
    public class BookingSeatRepository
    {
        public List<BookingSeat> GetAllBookingSeats()
        {
            var bookingSeats = new List<BookingSeat>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT bs.BookingSeatID, bs.BookingID, bs.SeatID, bs.SeatPriceAtBooking,
                         s.RowLabel, s.SeatNumber, s.SeatType,
                         b.CustomerID, b.ShowtimeID, b.BookingDateTime, b.Status
                  FROM BookingSeat bs
                  JOIN Seat s ON bs.SeatID = s.SeatID
                  JOIN Booking b ON bs.BookingID = b.BookingID",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                bookingSeats.Add(new BookingSeat
                {
                    BookingSeatID = reader.GetInt32("BookingSeatID"),
                    BookingID = reader.GetInt32("BookingID"),
                    SeatID = reader.GetInt32("SeatID"),
                    SeatPriceAtBooking = reader.GetDecimal("SeatPriceAtBooking"),
                    Seat = new Seat
                    {
                        RowLabel = reader.GetString("RowLabel"),
                        SeatNumber = reader.GetInt32("SeatNumber"),
                        SeatType = reader.IsDBNull(reader.GetOrdinal("SeatType")) ? string.Empty : reader.GetString("SeatType")
                    }
                });
            }

            return bookingSeats;
        }

        public List<BookingSeat> GetBookingSeatsByBookingId(int bookingId)
        {
            var bookingSeats = new List<BookingSeat>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT bs.BookingSeatID, bs.BookingID, bs.SeatID, bs.SeatPriceAtBooking,
                         s.RowLabel, s.SeatNumber, s.SeatType
                  FROM BookingSeat bs
                  JOIN Seat s ON bs.SeatID = s.SeatID
                  WHERE bs.BookingID = @bookingId",
                connection);
            command.Parameters.AddWithValue("@bookingId", bookingId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                bookingSeats.Add(new BookingSeat
                {
                    BookingSeatID = reader.GetInt32("BookingSeatID"),
                    BookingID = reader.GetInt32("BookingID"),
                    SeatID = reader.GetInt32("SeatID"),
                    SeatPriceAtBooking = reader.GetDecimal("SeatPriceAtBooking"),
                    Seat = new Seat
                    {
                        RowLabel = reader.GetString("RowLabel"),
                        SeatNumber = reader.GetInt32("SeatNumber"),
                        SeatType = reader.IsDBNull(reader.GetOrdinal("SeatType")) ? string.Empty : reader.GetString("SeatType")
                    }
                });
            }

            return bookingSeats;
        }
    }
}