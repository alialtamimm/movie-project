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
        public string MembershipType { get; set; } // Standard, Premium, VIP
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

        // returns discount based on membership
        public decimal GetDiscount()
        {
            switch (MembershipType)
            {
                case "Premium":
                    return 0.10m; // 10% off
                case "VIP":
                    return 0.20m; // 20% off
                default:
                    return 0.00m; // no discount
            }
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({MembershipType})";
        }
    }
}