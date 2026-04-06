using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;

namespace CinemaSystem.Pages
{
    public class MyTicketsModel : PageModel
    {
        private readonly CustomLinkedList<Ticket> _ticketList;

        public Ticket[] Tickets { get; set; } = new Ticket[0];

        public MyTicketsModel(CustomLinkedList<Ticket> ticketList)
        {
            _ticketList = ticketList;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("CustomerEmail") == null)
            {
                return RedirectToPage("/Login");
            }

            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            Tickets = _ticketList.SearchAll(t => t.CustomerId == customerId);

            return Page();
        }
    }
}