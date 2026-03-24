using System;

namespace CinemaSystem.Models
{
    // TODO: Eren finish
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
    }
}