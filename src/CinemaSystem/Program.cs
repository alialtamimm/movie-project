using CinemaSystem.Models;
using CinemaSystem.DataStructures;
using CinemaSystem.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<CustomHashTable<int, Film>>(new CustomHashTable<int, Film>(20));
builder.Services.AddSingleton<CustomLinkedList<Customer>>(new CustomLinkedList<Customer>());
builder.Services.AddSingleton<CustomLinkedList<Ticket>>(new CustomLinkedList<Ticket>());

// register the database helper as a service so pages can use it
string connString = @"Server=(localdb)\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;TrustServerCertificate=True;";
builder.Services.AddSingleton<DatabaseHelper>(new DatabaseHelper(connString));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

var filmTable = app.Services.GetRequiredService<CustomHashTable<int, Film>>();
var customerList = app.Services.GetRequiredService<CustomLinkedList<Customer>>();
var ticketList = app.Services.GetRequiredService<CustomLinkedList<Ticket>>();
var db = app.Services.GetRequiredService<DatabaseHelper>();

// try loading data from the sql database
try
{
    db.LoadFilms(filmTable);
    db.LoadCustomers(customerList);
    db.LoadTickets(ticketList);
    Console.WriteLine("Data loaded from database.");
}
catch (Exception ex)
{
    Console.WriteLine("Could not connect to database: " + ex.Message);
    Console.WriteLine("Loading sample films instead.");
    LoadSampleFilms(filmTable);
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapRazorPages();

app.Run();

// load sample films if database is not available
void LoadSampleFilms(CustomHashTable<int, Film> table)
{
    table.Insert(1, new Film(1, "Inception", "Sci-Fi", 148, "12A", "14:00", 12.99m, 50, "/images/inception.jpg"));
    table.Insert(2, new Film(2, "The Dark Knight", "Action", 152, "12A", "17:00", 13.99m, 50, "/images/darkknight.jpg"));
    table.Insert(3, new Film(3, "Interstellar", "Sci-Fi", 169, "12A", "20:00", 14.99m, 50, "/images/interstellar.jpg"));
    table.Insert(4, new Film(4, "The Godfather", "Crime", 175, "18", "21:00", 11.99m, 50, "/images/godfather.jpg"));
    table.Insert(5, new Film(5, "Pulp Fiction", "Crime", 154, "18", "22:00", 11.99m, 50, "/images/pulpfiction.jpg"));
    table.Insert(6, new Film(6, "Toy Story", "Animation", 81, "PG", "11:00", 8.99m, 50, "/images/toystory.jpg"));
    table.Insert(7, new Film(7, "Finding Nemo", "Animation", 100, "U", "13:00", 8.99m, 50, "/images/findingnemo.jpg"));
    table.Insert(8, new Film(8, "Avengers Endgame", "Action", 181, "12A", "18:30", 15.99m, 50, "/images/avengers.jpg"));
}