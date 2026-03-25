using System;

namespace CinemaSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int MemberId { get; set; }
        public int FilmId { get; set; }
        public DateTime BookingDate { get; set; }
        public int SeatNumber { get; set; }
        public decimal TotalPrice { get; set; }

        public Booking()
        {
            BookingDate = DateTime.Now;
        }

        public Booking(int bookingId, int memberId, int filmId, int seatNumber, decimal totalPrice)
        {
            BookingId = bookingId;
            MemberId = memberId;
            FilmId = filmId;
            SeatNumber = seatNumber;
            TotalPrice = totalPrice;
            BookingDate = DateTime.Now;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Booking ID: {BookingId}");
            Console.WriteLine($"Member ID: {MemberId}");
            Console.WriteLine($"Film ID: {FilmId}");
            Console.WriteLine($"Seat: {SeatNumber}");
            Console.WriteLine($"Price: £{TotalPrice:F2}");
            Console.WriteLine($"Date: {BookingDate.ToShortDateString()}");
        }

        public override string ToString()
        {
            return $"Booking #{BookingId} - Film {FilmId} Seat {SeatNumber} - £{TotalPrice:F2}";
        }
    }
}