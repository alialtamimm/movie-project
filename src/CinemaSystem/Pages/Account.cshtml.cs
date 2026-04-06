using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;

namespace CinemaSystem.Pages
{
    public class AccountModel : PageModel
    {
        private readonly CustomLinkedList<Customer> _customerList;

        public Customer CurrentCustomer { get; set; }

        public AccountModel(CustomLinkedList<Customer> customerList)
        {
            _customerList = customerList;
        }

        public IActionResult OnGet()
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            if (email == null)
            {
                return RedirectToPage("/Login");
            }

            CurrentCustomer = _customerList.Search(c => c.Email == email);
            return Page();
        }
    }
}
