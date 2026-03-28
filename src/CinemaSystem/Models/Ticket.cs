using System;

namespace CinemaSystem.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int CustomerId { get; set; }
        public int FilmId { get; set; }
        public string FilmTitle { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }

        // payment info
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CVV { get; set; }

        // address
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }

        public Ticket()
        {
            FilmTitle = "";
            CardNumber = "";
            CVV = "";
            AddressLine = "";
            City = "";
            Country = "";
            Postcode = "";
            PurchaseDate = DateTime.Now;
        }

        public Ticket(int ticketId, int customerId, int filmId, string filmTitle, int seat, decimal price)
        {
            TicketId = ticketId;
            CustomerId = customerId;
            FilmId = filmId;
            FilmTitle = filmTitle;
            SeatNumber = seat;
            Price = price;
            PurchaseDate = DateTime.Now;
            CardNumber = "";
            CVV = "";
            AddressLine = "";
            City = "";
            Country = "";
            Postcode = "";
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"  Ticket ID: {TicketId}");
            Console.WriteLine($"  Film: {FilmTitle}");
            Console.WriteLine($"  Seat: {SeatNumber}");
            Console.WriteLine($"  Price: £{Price:F2}");
            Console.WriteLine($"  Purchased: {PurchaseDate.ToShortDateString()}");
        }

        public override string ToString()
        {
            return $"Ticket #{TicketId} - {FilmTitle} Seat {SeatNumber} - £{Price:F2}";
        }
    }
}