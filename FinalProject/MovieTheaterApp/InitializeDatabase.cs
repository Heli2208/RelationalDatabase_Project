using System;
using MySql.Data.MySqlClient;
using MovieTheaterApp.Database;

namespace MovieTheaterApp
{
    public static class InitializeDatabase
    {
        public static void InsertTeamData()
        {
            try
            {
                using var connection = DbHelper.GetConnection();
                connection.Open();

                Console.WriteLine(" Inserting team data into database...");

                var checkCommand = new MySqlCommand(
                    "SELECT COUNT(*) FROM Customer WHERE Email = 'patelheli22@gmail.com'",
                    connection);

                int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (existingCount > 0)
                {
                    Console.WriteLine("Team data already exists in database.");
                    return;
                }

                string insertHalls = @"
                    INSERT IGNORE INTO Hall (HallID, HallName, Capacity, Description) VALUES 
                    (1, 'shaann', 100, 'i love this theater'),
                    (2, 'gold', 50, 'smaller theater for kids films'),
                    (3, 'solaris', 60, 'good for actions movies');
                ";

                string insertMovies = @"
                    INSERT IGNORE INTO Movie (MovieID, Title, Genre, DurationMinutes, Language, Rating, ReleaseDate, Status) VALUES
                    (1, 'ADHM', 'Emotional', 145, 'Hindi', '5', '2025-10-02', 'now showing'),
                    (2, 'Hidden Love', 'Romantic', 120, 'Korean', '4.9', '2025-11-04', 'showing'),
                    (3, 'Race', 'Action', 110, 'Hindi', '4.8', '2025-12-02', 'not showing');
                ";

                string insertCustomers = @"
                    INSERT IGNORE INTO Customer (CustomerID, FirstName, LastName, Email, Phone) VALUES
                    (1, 'Heli', 'patel', 'patelheli22@gmail.com', '6543345453'),
                    (2, 'Halvin', 'silva', 'halvins01@gmail.com', '4567457675'),
                    (3, 'Yash', 'patel', 'patelansh@gmail.com', '2345754543');
                ";

                string insertSeats = @"
                    INSERT IGNORE INTO Seat (SeatID, HallID, RowLabel, SeatNumber, SeatType, IsActive) VALUES
                    -- Hall 1 (shaann)
                    (1, 1, 'A', 1, 'Standard', 1),
                    (2, 1, 'A', 2, 'Standard', 1),
                    (3, 1, 'A', 3, 'Standard', 1),
                    (4, 1, 'A', 4, 'Standard', 1),
                    (5, 1, 'A', 5, 'Standard', 1),
                    (6, 1, 'A', 6, 'Premium', 1),
                    (7, 1, 'A', 7, 'Premium', 1),
                    (8, 1, 'A', 8, 'Premium', 1),
                    (9, 1, 'A', 9, 'Premium', 1),
                    (10, 1, 'A', 10, 'Premium', 1),
                    -- Hall 2 (gold)
                    (11, 2, 'B', 1, 'Standard', 1),
                    (12, 2, 'B', 2, 'Standard', 1),
                    (13, 2, 'B', 3, 'Standard', 1),
                    (14, 2, 'B', 4, 'Standard', 1),
                    (15, 2, 'B', 5, 'Premium', 1),
                    (16, 2, 'B', 6, 'Premium', 1),
                    (17, 2, 'B', 7, 'Premium', 1),
                    (18, 2, 'B', 8, 'Premium', 1),
                    -- Hall 3 (solaris)
                    (19, 3, 'C', 1, 'Standard', 1),
                    (20, 3, 'C', 2, 'Standard', 1),
                    (21, 3, 'C', 3, 'Standard', 1),
                    (22, 3, 'C', 4, 'Premium', 1),
                    (23, 3, 'C', 5, 'Premium', 1),
                    (24, 3, 'C', 6, 'Premium', 1);
                ";

                string insertShowtimes = @"
                    INSERT IGNORE INTO Showtime (ShowtimeID, MovieID, HallID, StartDateTime, BaseTicketPrice, Status) VALUES
                    (1, 1, 1, '2025-12-10 14:00:00', 12.99, 'Active'),
                    (2, 1, 1, '2025-12-10 18:00:00', 14.99, 'Active'),
                    (3, 2, 2, '2025-12-10 15:30:00', 11.99, 'Active'),
                    (4, 3, 3, '2025-12-10 17:00:00', 13.99, 'Active');
                ";

                string allCommands = insertHalls + insertMovies + insertCustomers + insertSeats + insertShowtimes;

                using var command = new MySqlCommand(allCommands, connection);
                int rowsAffected = command.ExecuteNonQuery();

                Console.WriteLine($"Team data inserted successfully! {rowsAffected} rows affected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error inserting team data: {ex.Message}");
                Console.WriteLine("Continuing with existing data...");
            }
        }

        public static void CheckAndInsertData()
        {
            try
            {
                using var connection = DbHelper.GetConnection();
                connection.Open();

                var checkCommand = new MySqlCommand(
                    "SELECT COUNT(*) FROM Customer WHERE Email = 'patelheli22@gmail.com'",
                    connection);

                int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (existingCount == 0) 
                {
                    Console.WriteLine("Setting up database with team data...");
                    InsertTeamData();
                }
                else
                {
                    Console.WriteLine("Team data already exists in database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking data: {ex.Message}");
            }
        }
    }
}