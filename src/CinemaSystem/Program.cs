using System;
using CinemaSystem.Models;
using CinemaSystem.DataStructures;
using CinemaSystem.Data;

namespace CinemaSystem
{
    class Program
    {
        // these hold all our data in memory
        static CustomHashTable<int, Film> filmTable = new CustomHashTable<int, Film>(20);
        static CustomLinkedList<Member> memberList = new CustomLinkedList<Member>();
        static CustomLinkedList<Booking> bookingList = new CustomLinkedList<Booking>();
        static DatabaseHelper db;

        static void Main(string[] args)
        {
            // change this to your connection string
            string connString = @"Server=(localdb)\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";
            db = new DatabaseHelper(connString);

            // load data from database
            try
            {
                db.LoadFilms(filmTable);
                db.LoadMembers(memberList);
                db.LoadBookings(bookingList);
                Console.WriteLine("Data loaded from database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not connect to database: " + ex.Message);
                Console.WriteLine("Running without database.");
            }

            Console.WriteLine();
            Console.WriteLine("  Cinema Film Booking & Membership System");

            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Films");
                Console.WriteLine("2. Members");
                Console.WriteLine("3. Bookings");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        FilmMenu();
                        break;
                    case "2":
                        MemberMenu();
                        break;
                    case "3":
                        BookingMenu();
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }
        }

        // film menu
        static void FilmMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("--- Film Menu ---");
                Console.WriteLine("1. View all films");
                Console.WriteLine("2. Search film by ID");
                Console.WriteLine("3. Search film by title");
                Console.WriteLine("4. Add a film");
                Console.WriteLine("5. Back to main menu");
                Console.Write("Choose: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ViewAllFilms();
                        break;
                    case "2":
                        SearchFilmById();
                        break;
                    case "3":
                        SearchFilmByTitle();
                        break;
                    case "4":
                        AddFilm();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void ViewAllFilms()
        {
            Console.WriteLine();
            Film[] films = filmTable.GetAllValues();
            if (films.Length == 0)
            {
                Console.WriteLine("No films found.");
                return;
            }

            for (int i = 0; i < films.Length; i++)
            {
                Console.WriteLine("---");
                films[i].DisplayInfo();
            }
        }

        static void SearchFilmById()
        {
            Console.Write("Enter film ID: ");
            string input = Console.ReadLine();

            int id;
            if (!int.TryParse(input, out id))
            {
                Console.WriteLine("Please enter a valid number.");
                return;
            }

            Film found = filmTable.Search(id);
            if (found != null)
            {
                Console.WriteLine();
                found.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Film not found.");
            }
        }

        static void SearchFilmByTitle()
        {
            Console.Write("Enter film title (or part of it): ");
            string title = Console.ReadLine();

            if (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("Please enter a title.");
                return;
            }

            Film[] allFilms = filmTable.GetAllValues();
            bool found = false;

            for (int i = 0; i < allFilms.Length; i++)
            {
                if (allFilms[i].Title.ToLower().Contains(title.ToLower()))
                {
                    Console.WriteLine("---");
                    allFilms[i].DisplayInfo();
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No films found matching that title.");
            }
        }

        static void AddFilm()
        {
            Film film = new Film();

            Console.Write("Title: ");
            film.Title = Console.ReadLine();
            if (string.IsNullOrEmpty(film.Title))
            {
                Console.WriteLine("Title cannot be empty.");
                return;
            }

            Console.Write("Genre: ");
            film.Genre = Console.ReadLine();

            Console.Write("Duration (minutes): ");
            int dur;
            if (!int.TryParse(Console.ReadLine(), out dur))
            {
                Console.WriteLine("Invalid duration.");
                return;
            }
            film.Duration = dur;

            Console.Write("Rating (PG, 12A, 15, 18): ");
            film.Rating = Console.ReadLine();

            Console.Write("Showtime (e.g. 18:30): ");
            film.ShowTime = Console.ReadLine();

            Console.Write("Price: ");
            decimal price;
            if (!decimal.TryParse(Console.ReadLine(), out price))
            {
                Console.WriteLine("Invalid price.");
                return;
            }
            film.Price = price;

            try
            {
                int newId = db.AddFilm(film);
                film.FilmId = newId;
                filmTable.Insert(newId, film);
                Console.WriteLine($"Film added with ID {newId}.");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not save to database. Film added to memory only.");
                film.FilmId = filmTable.Count + 100;
                filmTable.Insert(film.FilmId, film);
            }
        }

        // member menu
        static void MemberMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("--- Member Menu ---");
                Console.WriteLine("1. View all members");
                Console.WriteLine("2. Search member by ID");
                Console.WriteLine("3. Register new member");
                Console.WriteLine("4. Back to main menu");
                Console.Write("Choose: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ViewAllMembers();
                        break;
                    case "2":
                        SearchMemberById();
                        break;
                    case "3":
                        RegisterMember();
                        break;
                    case "4":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void ViewAllMembers()
        {
            Console.WriteLine();
            Member[] members = memberList.ToArray();
            if (members.Length == 0)
            {
                Console.WriteLine("No members found.");
                return;
            }

            for (int i = 0; i < members.Length; i++)
            {
                Console.WriteLine("---");
                members[i].DisplayInfo();
            }
        }

        static void SearchMemberById()
        {
            Console.Write("Enter member ID: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Please enter a valid number.");
                return;
            }

            Member found = memberList.Search(m => m.MemberId == id);
            if (found != null)
            {
                Console.WriteLine();
                found.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Member not found.");
            }
        }

        static void RegisterMember()
        {
            Member member = new Member();

            Console.Write("First name: ");
            member.FirstName = Console.ReadLine();
            if (string.IsNullOrEmpty(member.FirstName))
            {
                Console.WriteLine("Name cannot be empty.");
                return;
            }

            Console.Write("Last name: ");
            member.LastName = Console.ReadLine();

            Console.Write("Email: ");
            member.Email = Console.ReadLine();

            Console.Write("Membership type (Standard/Premium/VIP): ");
            string type = Console.ReadLine();
            if (type != "Standard" && type != "Premium" && type != "VIP")
            {
                Console.WriteLine("Invalid membership type. Defaulting to Standard.");
                type = "Standard";
            }
            member.MembershipType = type;

            try
            {
                int newId = db.AddMember(member);
                member.MemberId = newId;
                memberList.InsertAtTail(member);
                Console.WriteLine($"Member registered with ID {newId}.");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not save to database. Member added to memory only.");
                member.MemberId = memberList.Count + 100;
                memberList.InsertAtTail(member);
            }
        }

        // booking menu
        static void BookingMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("--- Booking Menu ---");
                Console.WriteLine("1. View all bookings");
                Console.WriteLine("2. Book a film");
                Console.WriteLine("3. Cancel a booking");
                Console.WriteLine("4. Back to main menu");
                Console.Write("Choose: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ViewAllBookings();
                        break;
                    case "2":
                        BookFilm();
                        break;
                    case "3":
                        CancelBooking();
                        break;
                    case "4":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void ViewAllBookings()
        {
            Console.WriteLine();
            Booking[] bookings = bookingList.ToArray();
            if (bookings.Length == 0)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            for (int i = 0; i < bookings.Length; i++)
            {
                Console.WriteLine("---");
                bookings[i].DisplayInfo();
            }
        }

        static void BookFilm()
        {
            // show available films first
            Console.WriteLine();
            Console.WriteLine("Available films:");
            ViewAllFilms();

            Console.Write("\nEnter film ID to book: ");
            int filmId;
            if (!int.TryParse(Console.ReadLine(), out filmId))
            {
                Console.WriteLine("Invalid film ID.");
                return;
            }

            Film film = filmTable.Search(filmId);
            if (film == null)
            {
                Console.WriteLine("Film not found.");
                return;
            }

            Console.Write("Enter your member ID: ");
            int memberId;
            if (!int.TryParse(Console.ReadLine(), out memberId))
            {
                Console.WriteLine("Invalid member ID.");
                return;
            }

            Member member = memberList.Search(m => m.MemberId == memberId);
            if (member == null)
            {
                Console.WriteLine("Member not found. Please register first.");
                return;
            }

            Console.Write("Enter seat number: ");
            int seat;
            if (!int.TryParse(Console.ReadLine(), out seat))
            {
                Console.WriteLine("Invalid seat number.");
                return;
            }

            // calculate price with discount
            decimal discount = member.GetDiscount();
            decimal finalPrice = film.Price - (film.Price * discount);

            Console.WriteLine();
            Console.WriteLine($"Film: {film.Title}");
            Console.WriteLine($"Member: {member.FirstName} {member.LastName} ({member.MembershipType})");
            Console.WriteLine($"Seat: {seat}");
            Console.WriteLine($"Original price: £{film.Price:F2}");
            if (discount > 0)
            {
                Console.WriteLine($"Discount: {discount * 100}%");
            }
            Console.WriteLine($"Total: £{finalPrice:F2}");
            Console.Write("Confirm booking? (y/n): ");

            string confirm = Console.ReadLine();
            if (confirm.ToLower() != "y")
            {
                Console.WriteLine("Booking cancelled.");
                return;
            }

            Booking booking = new Booking();
            booking.MemberId = memberId;
            booking.FilmId = filmId;
            booking.SeatNumber = seat;
            booking.TotalPrice = finalPrice;

            try
            {
                int newId = db.AddBooking(booking);
                booking.BookingId = newId;
                bookingList.InsertAtTail(booking);
                Console.WriteLine($"Booking confirmed! Booking ID: {newId}");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not save to database. Booking added to memory only.");
                booking.BookingId = bookingList.Count + 100;
                bookingList.InsertAtTail(booking);
            }
        }

        static void CancelBooking()
        {
            Console.Write("Enter booking ID to cancel: ");
            int bookingId;
            if (!int.TryParse(Console.ReadLine(), out bookingId))
            {
                Console.WriteLine("Invalid booking ID.");
                return;
            }

            Booking found = bookingList.Search(b => b.BookingId == bookingId);
            if (found == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            Console.Write("Are you sure you want to cancel this booking? (y/n): ");
            string confirm = Console.ReadLine();
            if (confirm.ToLower() != "y")
            {
                Console.WriteLine("Cancellation aborted.");
                return;
            }

            bool deleted = bookingList.Delete(b => b.BookingId == bookingId);
            if (deleted)
            {
                try
                {
                    db.DeleteBooking(bookingId);
                }
                catch (Exception) { }

                Console.WriteLine("Booking cancelled.");
            }
            else
            {
                Console.WriteLine("Could not cancel booking.");
            }
        }
    }
}