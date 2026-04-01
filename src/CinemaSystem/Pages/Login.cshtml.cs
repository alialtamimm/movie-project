using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;

namespace CinemaSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly CustomLinkedList<Customer> _customerList;

        public string ErrorMessage { get; set; } = "";

        public LoginModel(CustomLinkedList<Customer> customerList)
        {
            _customerList = customerList;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string Email, string Password)
        {
            Customer found = _customerList.Search(c => c.Email == Email && c.Password == Password);

            if (found != null)
            {
                // save login info in session
                HttpContext.Session.SetString("CustomerEmail", found.Email);
                HttpContext.Session.SetString("CustomerName", found.FirstName);
                HttpContext.Session.SetInt32("CustomerId", found.CustomerId);
                HttpContext.Session.SetString("MembershipType", found.MembershipType);

                return RedirectToPage("/Films");
            }

            ErrorMessage = "Invalid email or password.";
            return Page();
        }
    }
}