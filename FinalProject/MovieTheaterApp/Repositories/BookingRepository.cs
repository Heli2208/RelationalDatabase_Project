/*
 * File Name: BookingRepository.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Implements data access methods for bookings, including retrieving booking
 *   details, managing booking seats, creating bookings with seats, cancelling,
 *   and deleting bookings using MySQL.
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MovieTheaterApp.Models;
using MovieTheaterApp.Database;

namespace MovieTheaterApp.Repositories
{
    public class BookingRepository
    {
        public List<Booking> GetAllBookings()
        {
            var bookings = new List<Booking>();
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT b.BookingID, b.CustomerID, b.ShowtimeID, b.BookingDateTime, b.Status,
                         c.FirstName, c.LastName, c.Email,
                         s.StartDateTime, s.BaseTicketPrice,
                         m.Title AS MovieTitle
                  FROM Booking b
                  JOIN Customer c ON b.CustomerID = c.CustomerID
                  JOIN Showtime s ON b.ShowtimeID = s.ShowtimeID
                  JOIN Movie m ON s.MovieID = m.MovieID
                  ORDER BY b.BookingDateTime DESC",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                bookings.Add(new Booking
                {
                    BookingID = reader.GetInt32("BookingID"),
                    CustomerID = reader.GetInt32("CustomerID"),
                    ShowtimeID = reader.GetInt32("ShowtimeID"),
                    BookingDateTime = reader.GetDateTime("BookingDateTime"),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString("Status"),
                    Customer = new Customer
                    {
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email")
                    },
                    Showtime = new Showtime
                    {
                        StartDateTime = reader.GetDateTime("StartDateTime"),
                        BaseTicketPrice = reader.GetDecimal("BaseTicketPrice"),
                        Movie = new Movie
                        {
                            Title = reader.GetString("MovieTitle")
                        }
                    }
                });
            }

            return bookings;
        }

        public Booking? GetBookingWithDetails(int bookingId)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();

            var command = new MySqlCommand(
                @"SELECT b.BookingID, b.CustomerID, b.ShowtimeID, b.BookingDateTime, b.Status,
                         c.FirstName, c.LastName, c.Email,
                         s.StartDateTime, s.BaseTicketPrice,
                         m.Title AS MovieTitle, h.HallName
                  FROM Booking b
                  JOIN Customer c ON b.CustomerID = c.CustomerID
                  JOIN Showtime s ON b.ShowtimeID = s.ShowtimeID
                  JOIN Movie m ON s.MovieID = m.MovieID
                  JOIN Hall h ON s.HallID = h.HallID
                  WHERE b.BookingID = @bookingId",
                connection);
            command.Parameters.AddWithValue("@bookingId", bookingId);

            Booking? booking = null;

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    booking = new Booking
                    {
                        BookingID = reader.GetInt32("BookingID"),
                        CustomerID = reader.GetInt32("CustomerID"),
                        ShowtimeID = reader.GetInt32("ShowtimeID"),
                        BookingDateTime = reader.GetDateTime("BookingDateTime"),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader.GetString("Status"),
                        Customer = new Customer
                        {
                            FirstName = reader.GetString("FirstName"),
                            LastName = reader.GetString("LastName"),
                            Email = reader.GetString("Email")
                        },
                        Showtime = new Showtime
                        {
                            StartDateTime = reader.GetDateTime("StartDateTime"),
                            BaseTicketPrice = reader.GetDecimal("BaseTicketPrice"),
                            Movie = new Movie
                            {
                                Title = reader.GetString("MovieTitle")
                            },
                            Hall = new Hall
                            {
                                HallName = reader.GetString("HallName")
                            }
                        }
                    };
                }
            }

            if (booking == null) return null;

            booking.BookingSeats = GetBookingSeats(bookingId);
            booking.TotalPrice = CalculateTotalPrice(bookingId);

            return booking;
        }

        private List<BookingSeat> GetBookingSeats(int bookingId)
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

        private decimal CalculateTotalPrice(int bookingId)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "SELECT SUM(SeatPriceAtBooking) FROM BookingSeat WHERE BookingID = @bookingId",
                connection);
            command.Parameters.AddWithValue("@bookingId", bookingId);

            var result = command.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }

        public int CreateBookingWithSeats(Booking booking, List<int> seatIds)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var command = new MySqlCommand(
                    @"INSERT INTO Booking (CustomerID, ShowtimeID, BookingDateTime, Status) 
                      VALUES (@customerId, @showtimeId, @bookingDateTime, @status);
                      SELECT LAST_INSERT_ID();",
                    connection, transaction);

                command.Parameters.AddWithValue("@customerId", booking.CustomerID);
                command.Parameters.AddWithValue("@showtimeId", booking.ShowtimeID);
                command.Parameters.AddWithValue("@bookingDateTime", DateTime.Now);
                command.Parameters.AddWithValue("@status", "Confirmed");

                int bookingId = Convert.ToInt32(command.ExecuteScalar());

                foreach (var seatId in seatIds)
                {
                    var seatCommand = new MySqlCommand(
                        @"INSERT INTO BookingSeat (BookingID, SeatID, SeatPriceAtBooking) 
                          VALUES (@bookingId, @seatId, 
                          (SELECT BaseTicketPrice FROM Showtime WHERE ShowtimeID = @showtimeId))",
                        connection, transaction);

                    seatCommand.Parameters.AddWithValue("@bookingId", bookingId);
                    seatCommand.Parameters.AddWithValue("@seatId", seatId);
                    seatCommand.Parameters.AddWithValue("@showtimeId", booking.ShowtimeID);

                    seatCommand.ExecuteNonQuery();
                }

                transaction.Commit();
                return bookingId;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool CancelBooking(int bookingId)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "UPDATE Booking SET Status = 'Cancelled' WHERE BookingID = @bookingId",
                connection);
            command.Parameters.AddWithValue("@bookingId", bookingId);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteBooking(int bookingId)
        {
            using var connection = DbHelper.GetConnection();
            connection.Open();
            var command = new MySqlCommand(
                "DELETE FROM Booking WHERE BookingID = @bookingId",
                connection);
            command.Parameters.AddWithValue("@bookingId", bookingId);

            return command.ExecuteNonQuery() > 0;
        }
    }
}