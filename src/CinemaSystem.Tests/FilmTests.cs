using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaSystem.Models;

namespace CinemaSystem.Tests
{
    [TestClass]
    public class FilmTests
    {
        [TestMethod]
        public void TestFilmConstructor()
        {
            Film film = new Film(1, "Inception", "Sci-Fi", 148, "12A", "18:30", 12.99m, 50);

            Assert.AreEqual(1, film.FilmId);
            Assert.AreEqual("Inception", film.Title);
            Assert.AreEqual("Sci-Fi", film.Genre);
            Assert.AreEqual(148, film.Duration);
            Assert.AreEqual("12A", film.Rating);
            Assert.AreEqual("18:30", film.ShowTime);
            Assert.AreEqual(12.99m, film.Price);
            Assert.AreEqual(50, film.AvailableSeats);
        }

        [TestMethod]
        public void TestDefaultConstructor()
        {
            Film film = new Film();

            Assert.AreEqual("", film.Title);
            Assert.AreEqual("", film.Genre);
            Assert.AreEqual("", film.Rating);
            Assert.AreEqual(50, film.AvailableSeats);
        }

        [TestMethod]
        public void TestToString()
        {
            Film film = new Film(1, "Inception", "Sci-Fi", 148, "12A", "18:30", 12.99m, 50);
            string result = film.ToString();

            Assert.IsTrue(result.Contains("Inception"));
            Assert.IsTrue(result.Contains("Sci-Fi"));
        }

        [TestMethod]
        public void TestFilmEquals()
        {
            Film film1 = new Film(1, "Inception", "Sci-Fi", 148, "12A", "18:30", 12.99m, 50);
            Film film2 = new Film(1, "Inception", "Sci-Fi", 148, "12A", "18:30", 12.99m, 50);
            Film film3 = new Film(2, "Batman", "Action", 152, "12A", "20:00", 14.99m, 50);

            Assert.IsTrue(film1.Equals(film2));
            Assert.IsFalse(film1.Equals(film3));
        }

        [TestMethod]
        public void TestSetProperties()
        {
            Film film = new Film();
            film.FilmId = 5;
            film.Title = "Test Movie";
            film.Price = 9.99m;

            Assert.AreEqual(5, film.FilmId);
            Assert.AreEqual("Test Movie", film.Title);
            Assert.AreEqual(9.99m, film.Price);
        }
    }
}