using System;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;
using CinemaSystem.Data;

namespace CinemaSystem
{
    class Program
    {
        static CustomHashTable<int, Film> filmTable = new CustomHashTable<int, Film>(20);
        static CustomLinkedList<Customer> customerList = new CustomLinkedList<Customer>();
        static CustomLinkedList<Ticket> ticketList = new CustomLinkedList<Ticket>();
        static DatabaseHelper db;

        // who is currently logged in
        static Customer loggedInCustomer = null;
        static int nextTicketId = 1;

        static void Main(string[] args)
        {
            string connString = @"Server=(localdb)\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";
            db = new DatabaseHelper(connString);

            // try loading from database
            try
            {
                db.LoadFilms(filmTable);
                db.LoadCustomers(customerList);
                db.LoadTickets(ticketList);
                Console.WriteLine("Data loaded from database.");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not connect to database. Loading sample data.");
                LoadSampleFilms();
            }

            Console.WriteLine();
            Console.WriteLine("Cinema Film Booking & Membership System");

            bool running = true;

            while (running)
            {
                Console.WriteLine();

                if (loggedInCustomer == null)
                {
                    // not logged in menu
                    Console.WriteLine("Welcome!");
                    Console.WriteLine("1. Browse films");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("3. Login");
                    Console.WriteLine("4. Exit");
                }
                else
                {
                    // logged in menu
                    Console.WriteLine($"Hey {loggedInCustomer.FirstName}!");
                    Console.WriteLine("1. Browse films");
                    Console.WriteLine("2. Buy a ticket");
                    Console.WriteLine("3. My tickets");
                    Console.WriteLine("4. My account");
                    Console.WriteLine("5. Logout");
                    Console.WriteLine("6. Exit");
                }

                Console.Write("Choose: ");
                string input = Console.ReadLine();

                if (loggedInCustomer == null)
                {
                    switch (input)
                    {
                        case "1":
                            BrowseFilms();
                            break;
                        case "2":
                            Register();
                            break;
                        case "3":
                            Login();
                            break;
                        case "4":
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                else
                {
                    switch (input)
                    {
                        case "1":
                            BrowseFilms();
                            break;
                        case "2":
                            BuyTicket();
                            break;
                        case "3":
                            ViewMyTickets();
                            break;
                        case "4":
                            ViewAccount();
                            break;
                        case "5":
                            Console.WriteLine($"Logged out. Bye {loggedInCustomer.FirstName}!");
                            loggedInCustomer = null;
                            break;
                        case "6":
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
            }
        }

        // load some films if database doesnt work
        static void LoadSampleFilms()
        {
            filmTable.Insert(1, new Film(1, "Inception", "Sci-Fi", 148, "12A", "14:00", 12.99m, 50));
            filmTable.Insert(2, new Film(2, "The Dark Knight", "Action", 152, "12A", "17:00", 13.99m, 50));
            filmTable.Insert(3, new Film(3, "Interstellar", "Sci-Fi", 169, "12A", "20:00", 14.99m, 50));
            filmTable.Insert(4, new Film(4, "The Godfather", "Crime", 175, "18", "21:00", 11.99m, 50));
            filmTable.Insert(5, new Film(5, "Pulp Fiction", "Crime", 154, "18", "22:00", 11.99m, 50));
            filmTable.Insert(6, new Film(6, "Toy Story", "Animation", 81, "PG", "11:00", 8.99m, 50));
            filmTable.Insert(7, new Film(7, "Finding Nemo", "Animation", 100, "U", "13:00", 8.99m, 50));
            filmTable.Insert(8, new Film(8, "Avengers Endgame", "Action", 181, "12A", "18:30", 15.99m, 50));
        }

        // browse films
        static void BrowseFilms()
        {
            Console.WriteLine();
            Console.WriteLine("Available Films");

            Film[] films = filmTable.GetAllValues();
            if (films.Length == 0)
            {
                Console.WriteLine("No films available");
                return;
            }

            for (int i = 0; i < films.Length; i++)
            {
                Console.WriteLine();
                films[i].DisplayInfo();
                Console.WriteLine("  -------------------------");
            }

            // search
            Console.WriteLine();
            Console.Write("Search by title? (enter title or press Enter to go back): ");
            string search = Console.ReadLine();

            if (!string.IsNullOrEmpty(search))
            {
                bool found = false;
                for (int i = 0; i < films.Length; i++)
                {
                    if (films[i].Title.ToLower().Contains(search.ToLower()))
                    {
                        Console.WriteLine();
                        films[i].DisplayInfo();
                        found = true;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("No films found matching that");
                }
            }
        }

        // register
        static void Register()
        {
            Console.WriteLine();
            Console.WriteLine("Register");

            Customer customer = new Customer();

            Console.Write("First name: ");
            customer.FirstName = Console.ReadLine();
            if (string.IsNullOrEmpty(customer.FirstName))
            {
                Console.WriteLine("Name cannot be empty");
                return;
            }

            Console.Write("Last name: ");
            customer.LastName = Console.ReadLine();
            if (string.IsNullOrEmpty(customer.LastName))
            {
                Console.WriteLine("Last name cannot be empty");
                return;
            }

            Console.Write("Email: ");
            customer.Email = Console.ReadLine();
            if (string.IsNullOrEmpty(customer.Email) || !customer.Email.Contains("@"))
            {
                Console.WriteLine("Please enter a valid email");
                return;
            }

            // check if email already registered
            Customer existing = customerList.Search(c => c.Email == customer.Email);
            if (existing != null)
            {
                Console.WriteLine("An account with that email already exists, try login instead");
                return;
            }

            Console.Write("Password: ");
            customer.Password = Console.ReadLine();
            if (string.IsNullOrEmpty(customer.Password) || customer.Password.Length < 4)
            {
                Console.WriteLine("Password must be at least 4 characters.");
                return;
            }

            Console.Write("Membership type (Standard/Premium/VIP): ");
            string type = Console.ReadLine();
            if (type != "Standard" && type != "Premium" && type != "VIP")
            {
                Console.WriteLine("Invalid type, setting to Standard.");
                type = "Standard";
            }
            customer.MembershipType = type;

            try
            {
                int newId = db.AddCustomer(customer);
                customer.CustomerId = newId;
            }
            catch (Exception)
            {
                customer.CustomerId = customerList.Count + 1;
            }

            customerList.InsertAtTail(customer);
            Console.WriteLine($"Account created! Your customer ID is {customer.CustomerId}, you can now login.");
        }

        // login
        static void Login()
        {
            Console.WriteLine();
            Console.WriteLine("Login");

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Customer found = customerList.Search(c => c.Email == email && c.Password == password);

            if (found != null)
            {
                loggedInCustomer = found;
                Console.WriteLine($"Welcome back {found.FirstName}!");
            }
            else
            {
                Console.WriteLine("Invalid email or password");
            }
        }

        // buy ticket
        static void BuyTicket()
        {
            Console.WriteLine();
            Console.WriteLine("Buy a Ticket");

            // show films
            Film[] films = filmTable.GetAllValues();
            for (int i = 0; i < films.Length; i++)
            {
                Console.WriteLine();
                films[i].DisplayInfo();
                Console.WriteLine("  -------------------------");
            }

            Console.Write("\nEnter film ID: ");
            int filmId;
            if (!int.TryParse(Console.ReadLine(), out filmId))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            Film film = filmTable.Search(filmId);
            if (film == null)
            {
                Console.WriteLine("Film not found");
                return;
            }

            if (film.AvailableSeats <= 0)
            {
                Console.WriteLine("Sorry, this showing is sold out");
                return;
            }

            Console.Write("Pick a seat number (1-50): ");
            int seat;
            if (!int.TryParse(Console.ReadLine(), out seat) || seat < 1 || seat > 50)
            {
                Console.WriteLine("Invalid seat number");
                return;
            }

            // calculate price with discount
            decimal discount = loggedInCustomer.GetDiscount();
            decimal finalPrice = film.Price - (film.Price * discount);

            Console.WriteLine();
            Console.WriteLine($"  Film: {film.Title}");
            Console.WriteLine($"  Showtime: {film.ShowTime}");
            Console.WriteLine($"  Seat: {seat}");
            Console.WriteLine($"  Price: £{film.Price:F2}");
            if (discount > 0)
            {
                Console.WriteLine($"  Discount ({loggedInCustomer.MembershipType}): {discount * 100}%");
                Console.WriteLine($"  Final price: £{finalPrice:F2}");
            }

            // payment details
            Console.WriteLine();
            Console.WriteLine("Payment Details");

            Console.Write("Card number (16 digits): ");
            string cardNum = Console.ReadLine();
            // validate its only numbers
            if (string.IsNullOrEmpty(cardNum) || cardNum.Length != 16 || !IsAllDigits(cardNum))
            {
                Console.WriteLine("Invalid card number, must be 16 digits");
                return;
            }

            Console.Write("Expiry month (1-12): ");
            int expiryMonth;
            if (!int.TryParse(Console.ReadLine(), out expiryMonth) || expiryMonth < 1 || expiryMonth > 12)
            {
                Console.WriteLine("Invalid expiry month");
                return;
            }

            Console.Write("Expiry year (e.g. 2027): ");
            int expiryYear;
            if (!int.TryParse(Console.ReadLine(), out expiryYear) || expiryYear < 2026)
            {
                Console.WriteLine("Invalid expiry year");
                return;
            }

            Console.Write("CVV (3 digits): ");
            string cvv = Console.ReadLine();
            if (string.IsNullOrEmpty(cvv) || cvv.Length != 3 || !IsAllDigits(cvv))
            {
                Console.WriteLine("Invalid CVV, must be 3 digits");
                return;
            }

            // address
            Console.WriteLine();
            Console.WriteLine("Billing Address");

            Console.Write("Address line: ");
            string addressLine = Console.ReadLine();
            if (string.IsNullOrEmpty(addressLine))
            {
                Console.WriteLine("Address cannot be empty");
                return;
            }

            Console.Write("City: ");
            string city = Console.ReadLine();
            if (string.IsNullOrEmpty(city))
            {
                Console.WriteLine("City cannot be empty");
                return;
            }

            Console.Write("Country: ");
            string country = Console.ReadLine();
            if (string.IsNullOrEmpty(country))
            {
                Console.WriteLine("Country cannot be empty");
                return;
            }

            Console.Write("Postcode: ");
            string postcode = Console.ReadLine();
            if (string.IsNullOrEmpty(postcode))
            {
                Console.WriteLine("Postcode cannot be empty");
                return;
            }

            // confirm
            Console.WriteLine();
            Console.Write("Confirm purchase? (y/n): ");
            string confirm = Console.ReadLine();
            if (confirm.ToLower() != "y")
            {
                Console.WriteLine("Purchase cancelled");
                return;
            }

            // create ticket
            Ticket ticket = new Ticket();
            ticket.TicketId = nextTicketId;
            ticket.CustomerId = loggedInCustomer.CustomerId;
            ticket.FilmId = filmId;
            ticket.FilmTitle = film.Title;
            ticket.SeatNumber = seat;
            ticket.Price = finalPrice;
            ticket.CardNumber = cardNum;
            ticket.ExpiryMonth = expiryMonth;
            ticket.ExpiryYear = expiryYear;
            ticket.CVV = cvv;
            ticket.AddressLine = addressLine;
            ticket.City = city;
            ticket.Country = country;
            ticket.Postcode = postcode;

            try
            {
                int newId = db.AddTicket(ticket);
                ticket.TicketId = newId;
            }
            catch (Exception)
            {
                ticket.TicketId = nextTicketId;
            }

            ticketList.InsertAtTail(ticket);
            film.AvailableSeats--;
            nextTicketId++;

            Console.WriteLine();
            Console.WriteLine("Payment successful!");
            Console.WriteLine($"Your ticket ID is: {ticket.TicketId}");
            Console.WriteLine($"Enjoy {film.Title}!");
        }

        // view ur tickets
        static void ViewMyTickets()
        {
            Console.WriteLine();
            Console.WriteLine("My Tickets");

            Ticket[] myTickets = ticketList.SearchAll(t => t.CustomerId == loggedInCustomer.CustomerId);

            if (myTickets.Length == 0)
            {
                Console.WriteLine("You have no tickets");
                return;
            }

            for (int i = 0; i < myTickets.Length; i++)
            {
                Console.WriteLine();
                myTickets[i].DisplayInfo();
                Console.WriteLine("  -------------------------");
            }
        }

        // view account
        static void ViewAccount()
        {
            Console.WriteLine();
            Console.WriteLine("My Account");
            loggedInCustomer.DisplayInfo();
        }

        // checks if the string is numbers only
        static bool IsAllDigits(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > '9')
                {
                    return false;
                }
            }
            return true;
        }
    }
}