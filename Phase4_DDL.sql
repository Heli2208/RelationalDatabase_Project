CREATE DATABASE IF NOT EXISTS movietheaterdb;
USE movietheaterdb;

SET default_storage_engine = INNODB;

DROP TABLE IF EXISTS BookingSeat;
DROP TABLE IF EXISTS Booking;
DROP TABLE IF EXISTS Showtime;
DROP TABLE IF EXISTS Seat;
DROP TABLE IF EXISTS Customer;
DROP TABLE IF EXISTS Movie;
DROP TABLE IF EXISTS Hall;

CREATE TABLE Hall (
    HallID INT AUTO_INCREMENT PRIMARY KEY,
    HallName VARCHAR(100) NOT NULL,
    Capacity INT NOT NULL,
    Description VARCHAR(255)
) ENGINE=InnoDB;

CREATE TABLE Movie (
    MovieID INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Genre VARCHAR(100),
    DurationMinutes INT NOT NULL,
    Language VARCHAR(50),
    Rating VARCHAR(10),
    ReleaseDate DATE,
    Status VARCHAR(50)
) ENGINE=InnoDB;

CREATE TABLE Customer (
    CustomerID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Phone VARCHAR(30),
    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT UQ_Customer_Email UNIQUE (Email)
) ENGINE=InnoDB;

CREATE TABLE Seat (
    SeatID INT AUTO_INCREMENT PRIMARY KEY,
    HallID INT NOT NULL,
    RowLabel VARCHAR(10) NOT NULL,
    SeatNumber INT NOT NULL,
    SeatType VARCHAR(50),
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT FK_Seat_Hall
        FOREIGN KEY (HallID)
        REFERENCES Hall (HallID)
        ON DELETE CASCADE,
    CONSTRAINT UQ_Seat_Hall_Row_Seat
        UNIQUE (HallID, RowLabel, SeatNumber)
) ENGINE=InnoDB;

CREATE TABLE Showtime (
    ShowtimeID INT AUTO_INCREMENT PRIMARY KEY,
    MovieID INT NOT NULL,
    HallID INT NOT NULL,
    StartDateTime DATETIME NOT NULL,
    BaseTicketPrice DECIMAL(10,2) NOT NULL,
    Status VARCHAR(50),
    CONSTRAINT FK_Showtime_Movie
        FOREIGN KEY (MovieID)
        REFERENCES Movie (MovieID)
        ON DELETE CASCADE,
    CONSTRAINT FK_Showtime_Hall
        FOREIGN KEY (HallID)
        REFERENCES Hall (HallID)
        ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE Booking (
    BookingID INT AUTO_INCREMENT PRIMARY KEY,
    CustomerID INT NOT NULL,
    ShowtimeID INT NOT NULL,
    BookingDateTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(50),
    CONSTRAINT FK_Booking_Customer
        FOREIGN KEY (CustomerID)
        REFERENCES Customer (CustomerID)
        ON DELETE CASCADE,
    CONSTRAINT FK_Booking_Showtime
        FOREIGN KEY (ShowtimeID)
        REFERENCES Showtime (ShowtimeID)
        ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE BookingSeat (
    BookingSeatID INT AUTO_INCREMENT PRIMARY KEY,
    BookingID INT NOT NULL,
    SeatID INT NOT NULL,
    SeatPriceAtBooking DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_BookingSeat_Booking
        FOREIGN KEY (BookingID)
        REFERENCES Booking (BookingID)
        ON DELETE CASCADE,
    CONSTRAINT FK_BookingSeat_Seat
        FOREIGN KEY (SeatID)
        REFERENCES Seat (SeatID)
        ON DELETE CASCADE,
    CONSTRAINT UQ_BookingSeat_Booking_Seat
        UNIQUE (BookingID, SeatID)
)ENGINE=InnoDB;