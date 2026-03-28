using System;

namespace CinemaSystem.Models
{
    public class Film
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; } // minutes
        public string Rating { get; set; } // pg, 12a, 15, 18
        public string ShowTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }

        public Film()
        {
            Title = "";
            Genre = "";
            Rating = "";
            ShowTime = "";
            AvailableSeats = 50;
        }

        public Film(int filmId, string title, string genre, int duration, string rating, string showTime, decimal price, int seats)
        {
            FilmId = filmId;
            Title = title;
            Genre = genre;
            Duration = duration;
            Rating = rating;
            ShowTime = showTime;
            Price = price;
            AvailableSeats = seats;
        }

        // prints film info to the console
        public void DisplayInfo()
        {
            Console.WriteLine($"  ID: {FilmId}");
            Console.WriteLine($"  Title: {Title}");
            Console.WriteLine($"  Genre: {Genre}");
            Console.WriteLine($"  Duration: {Duration} mins");
            Console.WriteLine($"  Rating: {Rating}");
            Console.WriteLine($"  Showtime: {ShowTime}");
            Console.WriteLine($"  Price: £{Price:F2}");
            Console.WriteLine($"  Seats left: {AvailableSeats}");
        }

        public override string ToString()
        {
            return $"{Title} ({Genre}) - {ShowTime} - £{Price:F2}";
        }

        // need for the hash table to compare films
        public override bool Equals(object obj)
        {
            if (obj is Film other)
            {
                return this.FilmId == other.FilmId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return FilmId.GetHashCode();
        }
    }
}