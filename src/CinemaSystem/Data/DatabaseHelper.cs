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

                    filmTable.Insert(film.FilmId, film);
                }
            }
        }

        // load all members from db into list
        public void LoadMembers(CustomLinkedList<Member> memberList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Members";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Member member = new Member();
                    member.MemberId = (int)reader["MemberId"];
                    member.FirstName = reader["FirstName"].ToString();
                    member.LastName = reader["LastName"].ToString();
                    member.Email = reader["Email"].ToString();
                    member.MembershipType = reader["MembershipType"].ToString();
                    member.JoinDate = (DateTime)reader["JoinDate"];

                    memberList.InsertAtTail(member);
                }
            }
        }

        // load all bookings from db into linked list
        public void LoadBookings(CustomLinkedList<Booking> bookingList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Bookings";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Booking booking = new Booking();
                    booking.BookingId = (int)reader["BookingId"];
                    booking.MemberId = (int)reader["MemberId"];
                    booking.FilmId = (int)reader["FilmId"];
                    booking.BookingDate = (DateTime)reader["BookingDate"];
                    booking.SeatNumber = (int)reader["SeatNumber"];
                    booking.TotalPrice = (decimal)reader["TotalPrice"];

                    bookingList.InsertAtTail(booking);
                }
            }
        }

        // add the new film to the database
        public int AddFilm(Film film)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Films (Title, Genre, Duration, Rating, ShowTime, Price) " +
                               "VALUES (@title, @genre, @duration, @rating, @showtime, @price); " +
                               "SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", film.Title);
                cmd.Parameters.AddWithValue("@genre", film.Genre);
                cmd.Parameters.AddWithValue("@duration", film.Duration);
                cmd.Parameters.AddWithValue("@rating", film.Rating);
                cmd.Parameters.AddWithValue("@showtime", film.ShowTime);
                cmd.Parameters.AddWithValue("@price", film.Price);

                // return the new id
                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                return newId;
            }
        }

        // add a new member to the database
        public int AddMember(Member member)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Members (FirstName, LastName, Email, MembershipType) " +
                               "VALUES (@fname, @lname, @email, @type); " +
                               "SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fname", member.FirstName);
                cmd.Parameters.AddWithValue("@lname", member.LastName);
                cmd.Parameters.AddWithValue("@email", member.Email);
                cmd.Parameters.AddWithValue("@type", member.MembershipType);

                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                return newId;
            }
        }

        // add a new booking to the database
        public int AddBooking(Booking booking)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Bookings (MemberId, FilmId, SeatNumber, TotalPrice) " +
                               "VALUES (@memberId, @filmId, @seat, @price); " +
                               "SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@memberId", booking.MemberId);
                cmd.Parameters.AddWithValue("@filmId", booking.FilmId);
                cmd.Parameters.AddWithValue("@seat", booking.SeatNumber);
                cmd.Parameters.AddWithValue("@price", booking.TotalPrice);

                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                return newId;
            }
        }

        // delete a booking from database
        public bool DeleteBooking(int bookingId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Bookings WHERE BookingId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", bookingId);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
    }
}