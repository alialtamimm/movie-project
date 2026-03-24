using System;

namespace CinemaSystem.Models
{
    // TODO: Eren finish
    public class Member
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MembershipType { get; set; } // Standard, Premium, VIP
        public DateTime JoinDate { get; set; }

        public Member()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            MembershipType = "Standard";
            JoinDate = DateTime.Now;
        }
    }
}