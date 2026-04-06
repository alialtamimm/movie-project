using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;
using CinemaSystem.Data;

namespace CinemaSystem.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly CustomLinkedList<Customer> _customerList;
        private readonly DatabaseHelper _db;

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public RegisterModel(CustomLinkedList<Customer> customerList, DatabaseHelper db)
        {
            _customerList = customerList;
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string FirstName, string LastName, string Email, string Password, string MembershipType)
        {
            // check if email already taken
            Customer[] allCustomers = _customerList.ToArray();
            for (int i = 0; i < allCustomers.Length; i++)
            {
                if (allCustomers[i].Email == Email)
                {
                    ErrorMessage = "An account with that email already exists.";
                    return Page();
                }
            }

            if (string.IsNullOrEmpty(Password) || Password.Length < 4)
            {
                ErrorMessage = "Password must be at least 4 characters.";
                return Page();
            }

            Customer customer = new Customer();
            customer.FirstName = FirstName;
            customer.LastName = LastName;
            customer.Email = Email;
            customer.Password = Password;
            if (MembershipType == null)
            {
                customer.MembershipType = "Standard";
            }
            else
            {
                customer.MembershipType = MembershipType;
            }

            // save to database first to get the new id
            try
            {
                int newId = _db.AddCustomer(customer);
                customer.CustomerId = newId;
            }
            catch (Exception)
            {
                customer.CustomerId = _customerList.Count + 1;
            }

            _customerList.InsertAtTail(customer);

            SuccessMessage = "Account created! You can now login.";
            return Page();
        }
    }
}