using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;

namespace CinemaSystem.Pages
{
    public class BuyTicketModel : PageModel
    {
        private readonly CustomHashTable<int, Film> _filmTable;
        private readonly CustomLinkedList<Ticket> _ticketList;

        public Film SelectedFilm { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public BuyTicketModel(CustomHashTable<int, Film> filmTable, CustomLinkedList<Ticket> ticketList)
        {
            _filmTable = filmTable;
            _ticketList = ticketList;
        }

        public IActionResult OnGet(int filmId)
        {
            if (HttpContext.Session.GetString("CustomerEmail") == null)
            {
                return RedirectToPage("/Login");
            }

            SelectedFilm = _filmTable.Search(filmId);
            if (SelectedFilm == null)
            {
                return RedirectToPage("/Films");
            }

            CalculateDiscount();
            return Page();
        }

        public IActionResult OnPost(int FilmId, int SeatNumber, string CardNumber, int ExpiryMonth, int ExpiryYear, string CVV, string AddressLine, string City, string Country, string Postcode)
        {
            if (HttpContext.Session.GetString("CustomerEmail") == null)
            {
                return RedirectToPage("/Login");
            }

            SelectedFilm = _filmTable.Search(FilmId);
            if (SelectedFilm == null)
            {
                return RedirectToPage("/Films");
            }

            CalculateDiscount();

            // validate card number - must be 16 digits
            if (string.IsNullOrEmpty(CardNumber) || CardNumber.Length != 16 || !IsAllDigits(CardNumber))
            {
                ErrorMessage = "Card number must be exactly 16 digits.";
                return Page();
            }

            // validate cvv - must be 3 digits
            if (string.IsNullOrEmpty(CVV) || CVV.Length != 3 || !IsAllDigits(CVV))
            {
                ErrorMessage = "CVV must be exactly 3 digits.";
                return Page();
            }

            // validate expiry
            if (ExpiryMonth < 1 || ExpiryMonth > 12)
            {
                ErrorMessage = "Expiry month must be between 1 and 12.";
                return Page();
            }

            if (ExpiryYear < 2026)
            {
                ErrorMessage = "Card has expired.";
                return Page();
            }

            // validate seat
            if (SeatNumber < 1 || SeatNumber > 50)
            {
                ErrorMessage = "Seat number must be between 1 and 50.";
                return Page();
            }

            // validate address
            if (string.IsNullOrEmpty(AddressLine) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(Postcode))
            {
                ErrorMessage = "All address fields are required.";
                return Page();
            }

            if (SelectedFilm.AvailableSeats <= 0)
            {
                ErrorMessage = "Sorry, this showing is sold out.";
                return Page();
            }

            // create the ticket
            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;

            Ticket ticket = new Ticket();
            ticket.TicketId = _ticketList.Count + 1;
            ticket.CustomerId = customerId;
            ticket.FilmId = FilmId;
            ticket.FilmTitle = SelectedFilm.Title;
            ticket.PosterUrl = SelectedFilm.PosterUrl;
            ticket.SeatNumber = SeatNumber;
            ticket.Price = FinalPrice;
            ticket.CardNumber = CardNumber;
            ticket.ExpiryMonth = ExpiryMonth;
            ticket.ExpiryYear = ExpiryYear;
            ticket.CVV = CVV;
            ticket.AddressLine = AddressLine;
            ticket.City = City;
            ticket.Country = Country;
            ticket.Postcode = Postcode;

            _ticketList.InsertAtTail(ticket);
            SelectedFilm.AvailableSeats--;

            SuccessMessage = $"Ticket purchased! Your ticket ID is #{ticket.TicketId}. Enjoy {SelectedFilm.Title}!";
            return Page();
        }

        private void CalculateDiscount()
        {
            string membershipType = HttpContext.Session.GetString("MembershipType") ?? "Standard";
            switch (membershipType)
            {
                case "Premium":
                    Discount = 0.10m;
                    break;
                case "VIP":
                    Discount = 0.20m;
                    break;
                default:
                    Discount = 0.00m;
                    break;
            }
            FinalPrice = SelectedFilm.Price - (SelectedFilm.Price * Discount);
        }

        // checks if string is only numbers
        private bool IsAllDigits(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > '9')
                    return false;
            }
            return true;
        }
    }
}