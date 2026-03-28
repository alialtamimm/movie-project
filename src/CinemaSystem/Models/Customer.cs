using System;

namespace CinemaSystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MembershipType { get; set; } // standart, premium, vip
        public DateTime JoinDate { get; set; }

        public Customer()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            Password = "";
            MembershipType = "Standard";
            JoinDate = DateTime.Now;
        }

        public Customer(int id, string firstName, string lastName, string email, string password, string membershipType)
        {
            CustomerId = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            MembershipType = membershipType;
            JoinDate = DateTime.Now;
        }

        // returns discount based on membership type
        public decimal GetDiscount()
        {
            switch (MembershipType)
            {
                case "Premium":
                    return 0.10m;
                case "VIP":
                    return 0.20m;
                default:
                    return 0.00m; // no discount for standart
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"  Name: {FirstName} {LastName}");
            Console.WriteLine($"  Email: {Email}");
            Console.WriteLine($"  Membership: {MembershipType}");
            Console.WriteLine($"  Discount: {GetDiscount() * 100}%");
            Console.WriteLine($"  Joined: {JoinDate.ToShortDateString()}");
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({MembershipType})";
        }
    }
}