using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaSystem.Models;

namespace CinemaSystem.Tests
{
    [TestClass]
    public class TicketTests
    {
        [TestMethod]
        public void TestTicketConstructor()
        {
            Ticket ticket = new Ticket(1, 10, 5, "Inception", 3, 12.99m);

            Assert.AreEqual(1, ticket.TicketId);
            Assert.AreEqual(10, ticket.CustomerId);
            Assert.AreEqual(5, ticket.FilmId);
            Assert.AreEqual("Inception", ticket.FilmTitle);
            Assert.AreEqual(3, ticket.SeatNumber);
            Assert.AreEqual(12.99m, ticket.Price);
        }

        [TestMethod]
        public void TestTicketDefaultDate()
        {
            Ticket ticket = new Ticket();
            Assert.AreEqual(DateTime.Now.Date, ticket.PurchaseDate.Date);
        }

        [TestMethod]
        public void TestTicketToString()
        {
            Ticket ticket = new Ticket(1, 10, 5, "Inception", 3, 12.99m);
            string result = ticket.ToString();

            Assert.IsTrue(result.Contains("Ticket #1"));
            Assert.IsTrue(result.Contains("Inception"));
        }

        [TestMethod]
        public void TestCustomerDiscount_Standard()
        {
            Customer customer = new Customer(1, "John", "Smith", "john@test.com", "pass123", "Standard");
            Assert.AreEqual(0.00m, customer.GetDiscount());
        }

        [TestMethod]
        public void TestCustomerDiscount_Premium()
        {
            Customer customer = new Customer(2, "Sarah", "Jones", "sarah@test.com", "pass123", "Premium");
            Assert.AreEqual(0.10m, customer.GetDiscount());
        }

        [TestMethod]
        public void TestCustomerDiscount_VIP()
        {
            Customer customer = new Customer(3, "Mike", "Wilson", "mike@test.com", "pass123", "VIP");
            Assert.AreEqual(0.20m, customer.GetDiscount());
        }

        [TestMethod]
        public void TestCustomerDefaultMembership()
        {
            Customer customer = new Customer();
            Assert.AreEqual("Standard", customer.MembershipType);
        }

        [TestMethod]
        public void TestCustomerToString()
        {
            Customer customer = new Customer(1, "John", "Smith", "john@test.com", "pass123", "Premium");
            string result = customer.ToString();

            Assert.IsTrue(result.Contains("John"));
            Assert.IsTrue(result.Contains("Premium"));
        }

        [TestMethod]
        public void TestPriceWithDiscount()
        {
            Film film = new Film(1, "Inception", "Sci-Fi", 148, "12A", "18:30", 10.00m, 50);
            Customer customer = new Customer(1, "Test", "User", "test@test.com", "pass", "VIP");

            decimal discount = customer.GetDiscount();
            decimal finalPrice = film.Price - (film.Price * discount);

            Assert.AreEqual(8.00m, finalPrice);
        }
    }
}
