/*
 * File Name: Program.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Entry point for the Movie Theater Ticket Booking System, handling database
 *   connection checks, initial data setup, and navigation through the main menu.
 */

using System;
using MovieTheaterApp.Database;
using MovieTheaterApp.Menus;

namespace MovieTheaterApp
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Movie Theater Ticket Booking System";

            Console.WriteLine("Connecting to database...");
            if (!DbHelper.TestConnection())
            {
                Console.WriteLine("Failed to connect to database. Please check your connection string.");
                Console.WriteLine("\nCommon issues:");
                Console.WriteLine("1. MySQL service might not be running");
                Console.WriteLine("2. Wrong username/password in connection string");
                Console.WriteLine("3. Database 'movietheaterdb' doesn't exist");
                Console.WriteLine("\nTry running your Phase4_DDL.sql script in MySQL first.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Database connected successfully!\n");

            try
            {
                Console.WriteLine("Checking for existing data...");
                InitializeDatabase.CheckAndInsertData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not insert team data: {ex.Message}");
                Console.WriteLine("Continuing with existing data...");
            }

            Console.WriteLine("\nPress any key to continue to main menu...");
            Console.ReadKey();

            MainMenu();
        }

        static void MainMenu()
        {
            var customerMenu = new CustomerMenu();
            var movieMenu = new MovieMenu();
            var showtimeMenu = new ShowtimeMenu();
            var bookingMenu = new BookingMenu();
            var hallMenu = new HallMenu();
            var seatMenu = new SeatMenu();
            var bookingSeatMenu = new BookingSeatMenu();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("   MOVIE THEATER TICKET BOOKING SYSTEM     ");
                Console.WriteLine("         MAIN MENU        ");
                Console.WriteLine(" 1. Customer Management ");
                Console.WriteLine(" 2. Movie Management ");
                Console.WriteLine(" 3. Showtime Management ");
                Console.WriteLine(" 4. Booking Management ");
                Console.WriteLine(" 5. Hall Management ");
                Console.WriteLine(" 6. Seat Management ");
                Console.WriteLine(" 7. Booking Seat Management ");
                Console.WriteLine(" 8. View Team Data ");
                Console.WriteLine(" 9. Exit ");
                Console.Write("\nSelect option (1-9): ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        customerMenu.ShowMenu();
                        break;
                    case "2":
                        movieMenu.ShowMenu();
                        break;
                    case "3":
                        showtimeMenu.ShowMenu();
                        break;
                    case "4":
                        bookingMenu.ShowMenu();
                        break;
                    case "5":
                        hallMenu.ShowMenu();
                        break;
                    case "6":
                        seatMenu.ShowMenu();
                        break;
                    case "7":
                        bookingSeatMenu.ShowMenu();
                        break;
                    case "8":
                        ViewTeamData();
                        break;
                    case "9":
                        ExitProgram();
                        return;
                    default:
                        Console.WriteLine("\n Invalid option! Please enter a number from 1 to 9.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ViewTeamData()
        {
            Console.Clear();
            Console.WriteLine("           PROJECT TEAM DATA                ");

            Console.WriteLine(" TEAM MEMBERS:");
            Console.WriteLine("   1. Heli Patel");
            Console.WriteLine("   Email: patelheli22@gmail.com");
            Console.WriteLine("   Phone: 6543345453\n");

            Console.WriteLine("   2. Halvin Silva");
            Console.WriteLine("   Email: halvins01@gmail.com");
            Console.WriteLine("   Phone: 4567457675\n");

            Console.WriteLine("   3. Yash Patel");
            Console.WriteLine("   Email: patelansh@gmail.com");
            Console.WriteLine("   Phone: 2345754543\n");

            Console.WriteLine("  MOVIE THEATER HALLS:");
            Console.WriteLine("   1. shaann Theater");
            Console.WriteLine("   Capacity: 100 seats");
            Console.WriteLine("   Description: i love this theater\n");

            Console.WriteLine("   2. gold Theater");
            Console.WriteLine("   Capacity: 50 seats");
            Console.WriteLine("   Description: smaller theater for kids films\n");

            Console.WriteLine("   3. solaris Theater");
            Console.WriteLine("   Capacity: 60 seats");
            Console.WriteLine("   Description: good for actions movies\n");

            Console.WriteLine(" MOVIES IN THEATER:");
            Console.WriteLine(" 1. ADHM (Ae Dil Hai Mushkil)");
            Console.WriteLine("   Genre: Emotional");
            Console.WriteLine("   Duration: 145 minutes");
            Console.WriteLine("   Language: Hindi");
            Console.WriteLine("   Rating: 5/5");
            Console.WriteLine("   Release Date: October 2, 2025");
            Console.WriteLine("   Status: Now Showing\n");

            Console.WriteLine(" 2. Hidden Love");
            Console.WriteLine("   Genre: Romantic");
            Console.WriteLine("   Duration: 120 minutes");
            Console.WriteLine("   Language: Korean");
            Console.WriteLine("   Rating: 4.9/5");
            Console.WriteLine("   Release Date: November 4, 2025");
            Console.WriteLine("   Status: Showing\n");

            Console.WriteLine(" 3. Race");
            Console.WriteLine("   Genre: Action");
            Console.WriteLine("   Duration: 110 minutes");
            Console.WriteLine("   Language: Hindi");
            Console.WriteLine("   Rating: 4.8/5");
            Console.WriteLine("   Release Date: December 2, 2025");
            Console.WriteLine("   Status: Not Showing\n");

            Console.WriteLine("Project: Relational Databases - PROG2111");
            Console.WriteLine("Course: Fall 2025");

            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
        }

        static void ExitProgram()
        {
            Console.Clear();
            Console.WriteLine("       THANK YOU FOR USING OUR SYSTEM!      ");

            Console.WriteLine("Project Team Members:");
            Console.WriteLine("1. Heli Patel");
            Console.WriteLine("2. Halvin Silva");
            Console.WriteLine("3. Yash Patel\n");

            Console.WriteLine("Course: Relational Databases (PROG2111)");
            Console.WriteLine("Semester: Fall 2025\n");

            Console.WriteLine("Database: MySQL");
            Console.WriteLine("Programming Language: C# with ADO.NET\n");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}


