using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;
using CinemaSystem.Data;

namespace CinemaSystem.Pages
{
    public class AccountModel : PageModel
    {
        private readonly CustomLinkedList<Customer> _customerList;
        private readonly CustomLinkedList<Ticket> _ticketList;
        private readonly DatabaseHelper _db;

        public Customer CurrentCustomer { get; set; }

        public AccountModel(CustomLinkedList<Customer> customerList, CustomLinkedList<Ticket> ticketList, DatabaseHelper db)
        {
            _customerList = customerList;
            _ticketList = ticketList;
            _db = db;
        }

        public IActionResult OnGet()
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            if (email == null)
            {
                return RedirectToPage("/Login");
            }

            // find the customer with that email
            Customer[] allCustomers = _customerList.ToArray();
            for (int i = 0; i < allCustomers.Length; i++)
            {
                if (allCustomers[i].Email == email)
                {
                    CurrentCustomer = allCustomers[i];
                    break;
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            if (email == null)
            {
                return RedirectToPage("/Login");
            }

            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;

            // delete from database
            try
            {
                _db.DeleteCustomer(customerId);
            }
            catch (Exception)
            {
            }

            // remove from in-memory linked lists too
            // (rebuild the customer list without this user)
            Customer[] allCustomers = _customerList.ToArray();
            CustomLinkedList<Customer> newList = new CustomLinkedList<Customer>();
            for (int i = 0; i < allCustomers.Length; i++)
            {
                if (allCustomers[i].CustomerId != customerId)
                {
                    newList.InsertAtTail(allCustomers[i]);
                }
            }
            // copy back into the original list
            // (since we cant replace the singleton, clear and re-add)
            ClearAndRefill(_customerList, newList.ToArray());

            // also remove their tickets from memory
            Ticket[] allTickets = _ticketList.ToArray();
            CustomLinkedList<Ticket> newTickets = new CustomLinkedList<Ticket>();
            for (int i = 0; i < allTickets.Length; i++)
            {
                if (allTickets[i].CustomerId != customerId)
                {
                    newTickets.InsertAtTail(allTickets[i]);
                }
            }
            ClearAndRefill(_ticketList, newTickets.ToArray());

            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

        private void ClearAndRefill<T>(CustomLinkedList<T> list, T[] items)
        {
            // hack to clear the list - delete head until empty
            while (list.Count > 0)
            {
                T[] all = list.ToArray();
                list.Delete(x => x.Equals(all[0]));
            }
            for (int i = 0; i < items.Length; i++)
            {
                list.InsertAtTail(items[i]);
            }
        }
    }
}