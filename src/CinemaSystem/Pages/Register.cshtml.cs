using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;

namespace CinemaSystem.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly CustomLinkedList<Customer> _customerList;

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public RegisterModel(CustomLinkedList<Customer> customerList)
        {
            _customerList = customerList;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string FirstName, string LastName, string Email, string Password, string MembershipType)
        {
            // check if email already taken
            Customer existing = _customerList.Search(c => c.Email == Email);
            if (existing != null)
            {
                ErrorMessage = "An account with that email already exists.";
                return Page();
            }

            if (string.IsNullOrEmpty(Password) || Password.Length < 4)
            {
                ErrorMessage = "Password must be at least 4 characters.";
                return Page();
            }

            Customer customer = new Customer();
            customer.CustomerId = _customerList.Count + 1;
            customer.FirstName = FirstName;
            customer.LastName = LastName;
            customer.Email = Email;
            customer.Password = Password;
            customer.MembershipType = MembershipType ?? "Standard";

            _customerList.InsertAtTail(customer);

            SuccessMessage = "Account created! You can now login.";
            return Page();
        }
    }
}