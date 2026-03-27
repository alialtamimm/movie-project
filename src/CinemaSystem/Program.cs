using System;

namespace CinemaSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("  Cinema Film Booking & Membership System");
            Console.WriteLine();

            bool running = true;

            while (running)
            {
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
                        Console.WriteLine("Film menu");
                        break;
                    case "2":
                        Console.WriteLine("Member menu");
                        break;
                    case "3":
                        Console.WriteLine("Booking menu");
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Goodbye");
                        break;
                    default:
                        Console.WriteLine("Invalid option please try again");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}