using System;

namespace CinemaSystem.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MembershipType { get; set; } // standart, premium, vip
        public DateTime JoinDate { get; set; }

        public Member()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            MembershipType = "Standard";
            JoinDate = DateTime.Now;
        }

        public Member(int memberId, string firstName, string lastName, string email, string membershipType)
        {
            MemberId = memberId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            MembershipType = membershipType;
            JoinDate = DateTime.Now;
        }

        // returns discount based on user
        public decimal GetDiscount()
        {
            switch (MembershipType)
            {
                case "Premium":
                    return 0.10m; // 10% off
                case "VIP":
                    return 0.20m; // 20% off
                default:
                    return 0.00m; // nothing for standart
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {MemberId}");
            Console.WriteLine($"Name: {FirstName} {LastName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Membership: {MembershipType}");
            Console.WriteLine($"Joined: {JoinDate.ToShortDateString()}");
            Console.WriteLine($"Discount: {GetDiscount() * 100}%");
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({MembershipType})";
        }
    }
}