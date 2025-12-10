/*
 * File Name: Customer.cs
 * Authors: Heli Patel, Halvin Silva Mayes, Yash Patel, Ayush Prajapati
 * Date: 09-12-2025
 * Assignment: Final Project
 * Description:
 *   Defines the Customer model, storing personal details such as name,
 *   email, phone, and created date along with a computed full name.
 */

using System;

namespace MovieTheaterApp.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedDate { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}