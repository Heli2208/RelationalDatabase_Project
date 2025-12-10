/*
 * File Name: CustomerMenu.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   This file implements the customer management menu, allowing users to
 *   view, add, update, delete, and search customers in the application.
 */

using System;
using System.Collections.Generic;
using MovieTheaterApp.Models;
using MovieTheaterApp.Repositories;

namespace MovieTheaterApp.Menus
{
    public class CustomerMenu
    {
        private readonly CustomerRepository customerRepo = new();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CUSTOMER MANAGEMENT ===");
                Console.WriteLine("1. View All Customers");
                Console.WriteLine("2. Add New Customer");
                Console.WriteLine("3. Update Customer");
                Console.WriteLine("4. Delete Customer");
                Console.WriteLine("5. Search Customer by ID");
                Console.WriteLine("6. Return to Main Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1": 
                        ViewAllCustomers(); 
                        break;
                    case "2": 
                        AddCustomer(); 
                        break;
                    case "3": 
                        UpdateCustomer(); 
                        break;
                    case "4": 
                        DeleteCustomer(); 
                        break;
                    case "5": 
                        SearchCustomer(); 
                        break;
                    case "6": 
                        return;
                    default: 
                        Console.WriteLine("Invalid option!"); 
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void ViewAllCustomers()
        {
            Console.WriteLine("\n--- ALL CUSTOMERS ---");
            var customers = customerRepo.GetAllCustomers();

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.CustomerID} | Name: {customer.FullName} | " +
                                $"Email: {customer.Email} | Phone: {customer.Phone} | " +
                                $"Created: {customer.CreatedDate:yyyy-MM-dd}");
            }
            Console.WriteLine($"\nTotal: {customers.Count} customers");
        }

        private void AddCustomer()
        {
            Console.WriteLine("\n--- ADD NEW CUSTOMER ---");
            var customer = new Customer();

            Console.Write("First Name: ");
            customer.FirstName = Console.ReadLine();

            Console.Write("Last Name: ");
            customer.LastName = Console.ReadLine();

            Console.Write("Email: ");
            customer.Email = Console.ReadLine();

            Console.Write("Phone: ");
            customer.Phone = Console.ReadLine();

            try
            {
                int newId = customerRepo.AddCustomer(customer);
                Console.WriteLine($"Customer added successfully! ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
            }
        }

        private void UpdateCustomer()
        {
            Console.Write("\nEnter Customer ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var customer = customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                Console.WriteLine("Customer not found!");
                return;
            }

            Console.WriteLine($"Current: {customer.FullName} | Email: {customer.Email} | Phone: {customer.Phone}");

            Console.Write("New First Name (press Enter to keep current): ");
            var firstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firstName))
                customer.FirstName = firstName;

            Console.Write("New Last Name: ");
            var lastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastName))
                customer.LastName = lastName;

            Console.Write("New Email: ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                customer.Email = email;

            Console.Write("New Phone: ");
            customer.Phone = Console.ReadLine();

            try
            {
                if (customerRepo.UpdateCustomer(customer))
                    Console.WriteLine("Customer updated successfully!");
                else
                    Console.WriteLine("Update failed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating customer: {ex.Message}");
            }
        }

        private void DeleteCustomer()
        {
            Console.Write("\nEnter Customer ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            Console.Write($"Are you sure you want to delete customer ID {id}? (y/n): ");
            if (Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
            {
                try
                {
                    if (customerRepo.DeleteCustomer(id))
                        Console.WriteLine("Customer deleted successfully!");
                    else
                        Console.WriteLine("Customer not found!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting customer: {ex.Message}");
                }
            }
        }

        private void SearchCustomer()
        {
            Console.Write("\nEnter Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                return;
            }

            var customer = customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                Console.WriteLine("Customer not found!");
                return;
            }

            Console.WriteLine($"\n--- CUSTOMER DETAILS ---");
            Console.WriteLine($"ID: {customer.CustomerID}");
            Console.WriteLine($"Name: {customer.FullName}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine($"Phone: {customer.Phone}");
            Console.WriteLine($"Created Date: {customer.CreatedDate:yyyy-MM-dd HH:mm}");
        }
    }
}