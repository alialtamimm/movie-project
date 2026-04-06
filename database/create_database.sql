-- Cinema Film Booking and Membership System
-- Database creation script

CREATE DATABASE CinemaDB;
GO

USE CinemaDB;
GO

CREATE TABLE Films (
    FilmId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    Genre NVARCHAR(50),
    Duration INT,
    Rating NVARCHAR(10),
    ShowTime NVARCHAR(10),
    Price DECIMAL(5,2),
    AvailableSeats INT DEFAULT 50
);

CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    MembershipType NVARCHAR(20) DEFAULT 'Standard',
    JoinDate DATE DEFAULT GETDATE()
);

CREATE TABLE Tickets (
    TicketId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId),
    FilmId INT FOREIGN KEY REFERENCES Films(FilmId),
    FilmTitle NVARCHAR(100),
    SeatNumber INT,
    Price DECIMAL(6,2),
    PurchaseDate DATETIME DEFAULT GETDATE(),
    CardNumber NVARCHAR(16),
    ExpiryMonth INT,
    ExpiryYear INT,
    CVV NVARCHAR(3),
    AddressLine NVARCHAR(200),
    City NVARCHAR(100),
    Country NVARCHAR(100),
    Postcode NVARCHAR(20)
);

-- sample films
INSERT INTO Films (Title, Genre, Duration, Rating, ShowTime, Price, AvailableSeats) VALUES
('Inception', 'Sci-Fi', 148, '12A', '14:00', 12.99, 50),
('The Dark Knight', 'Action', 152, '12A', '17:00', 13.99, 50),
('Interstellar', 'Sci-Fi', 169, '12A', '20:00', 14.99, 50),
('The Godfather', 'Crime', 175, '18', '21:00', 11.99, 50),
('Pulp Fiction', 'Crime', 154, '18', '22:00', 11.99, 50),
('Toy Story', 'Animation', 81, 'PG', '11:00', 8.99, 50),
('Finding Nemo', 'Animation', 100, 'U', '13:00', 8.99, 50),
('Avengers Endgame', 'Action', 181, '12A', '18:30', 15.99, 50);