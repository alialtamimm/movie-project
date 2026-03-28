using System;
using Microsoft.Data.SqlClient;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;

namespace CinemaSystem.Data
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string connString)
        {
            connectionString = connString;
        }

        // load all films from db into the hash table
        public void LoadFilms(CustomHashTable<int, Film> filmTable)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Films";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Film film = new Film();
                    film.FilmId = (int)reader["FilmId"];
                    film.Title = reader["Title"].ToString();
                    film.Genre = reader["Genre"].ToString();
                    film.Duration = (int)reader["Duration"];
                    film.Rating = reader["Rating"].ToString();
                    film.ShowTime = reader["ShowTime"].ToString();
                    film.Price = (decimal)reader["Price"];
                    film.AvailableSeats = (int)reader["AvailableSeats"];

                    filmTable.Insert(film.FilmId, film);
                }
            }
        }

        // load all customers
        public void LoadCustomers(CustomLinkedList<Customer> customerList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Customers";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Customer c = new Customer();
                    c.CustomerId = (int)reader["CustomerId"];
                    c.FirstName = reader["FirstName"].ToString();
                    c.LastName = reader["LastName"].ToString();
                    c.Email = reader["Email"].ToString();
                    c.Password = reader["Password"].ToString();
                    c.MembershipType = reader["MembershipType"].ToString();
                    c.JoinDate = (DateTime)reader["JoinDate"];

                    customerList.InsertAtTail(c);
                }
            }
        }

        // load all tickets
        public void LoadTickets(CustomLinkedList<Ticket> ticketList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Tickets";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Ticket t = new Ticket();
                    t.TicketId = (int)reader["TicketId"];
                    t.CustomerId = (int)reader["CustomerId"];
                    t.FilmId = (int)reader["FilmId"];
                    t.FilmTitle = reader["FilmTitle"].ToString();
                    t.SeatNumber = (int)reader["SeatNumber"];
                    t.Price = (decimal)reader["Price"];
                    t.PurchaseDate = (DateTime)reader["PurchaseDate"];
                    t.CardNumber = reader["CardNumber"].ToString();
                    t.ExpiryMonth = (int)reader["ExpiryMonth"];
                    t.ExpiryYear = (int)reader["ExpiryYear"];
                    t.CVV = reader["CVV"].ToString();
                    t.AddressLine = reader["AddressLine"].ToString();
                    t.City = reader["City"].ToString();
                    t.Country = reader["Country"].ToString();
                    t.Postcode = reader["Postcode"].ToString();

                    ticketList.InsertAtTail(t);
                }
            }
        }

        // register a new customer
        public int AddCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Customers (FirstName, LastName, Email, Password, MembershipType) " +
                               "VALUES (@fname, @lname, @email, @password, @type); " +
                               "SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fname", customer.FirstName);
                cmd.Parameters.AddWithValue("@lname", customer.LastName);
                cmd.Parameters.AddWithValue("@email", customer.Email);
                cmd.Parameters.AddWithValue("@password", customer.Password);
                cmd.Parameters.AddWithValue("@type", customer.MembershipType);

                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                return newId;
            }
        }

        // save a new ticket
        public int AddTicket(Ticket ticket)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Tickets (CustomerId, FilmId, FilmTitle, SeatNumber, Price, " +
                               "CardNumber, ExpiryMonth, ExpiryYear, CVV, AddressLine, City, Country, Postcode) " +
                               "VALUES (@custId, @filmId, @filmTitle, @seat, @price, " +
                               "@card, @expMonth, @expYear, @cvv, @addr, @city, @country, @postcode); " +
                               "SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@custId", ticket.CustomerId);
                cmd.Parameters.AddWithValue("@filmId", ticket.FilmId);
                cmd.Parameters.AddWithValue("@filmTitle", ticket.FilmTitle);
                cmd.Parameters.AddWithValue("@seat", ticket.SeatNumber);
                cmd.Parameters.AddWithValue("@price", ticket.Price);
                cmd.Parameters.AddWithValue("@card", ticket.CardNumber);
                cmd.Parameters.AddWithValue("@expMonth", ticket.ExpiryMonth);
                cmd.Parameters.AddWithValue("@expYear", ticket.ExpiryYear);
                cmd.Parameters.AddWithValue("@cvv", ticket.CVV);
                cmd.Parameters.AddWithValue("@addr", ticket.AddressLine);
                cmd.Parameters.AddWithValue("@city", ticket.City);
                cmd.Parameters.AddWithValue("@country", ticket.Country);
                cmd.Parameters.AddWithValue("@postcode", ticket.Postcode);

                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                return newId;
            }
        }
    }
}