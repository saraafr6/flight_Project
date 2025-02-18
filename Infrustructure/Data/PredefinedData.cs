using Domain.Commons.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class DataConstants
    {
        public static class Airlines
        {
            public static readonly string[] Names = { "ماهان", "ایران ایر", "آسمان", "قشم ایر" };
        }

        public static class Cities
        {
            public static readonly string[] Names = { "تهران", "مشهد", "شیراز", "اصفهان", "تبریز", "کیش", "یزد" };
        }

        public static class Prices
        {
            public static readonly decimal MinPrice = 1800000M;
            public static readonly decimal MaxPrice = 3200000M;
        }
    }

    public static class DataGeneratorHelpers
    {
        private static Random _random = new Random();

        public static DateTimeOffset RandomDate(int minDays, int maxDays) =>
            DateTimeOffset.UtcNow.AddDays(_random.Next(minDays, maxDays));

        public static decimal RandomPrice() =>
            DataConstants.Prices.MinPrice +
            (_random.Next(0, 15) * 100000M);

        public static string RandomMobile() =>
            $"0912{_random.Next(1000000, 9999999)}";

        public static (string origin, string destination) RandomCities()
        {
            var cities = DataConstants.Cities.Names.ToList();
            var origin = cities[_random.Next(cities.Count)];
            cities.Remove(origin);
            var destination = cities[_random.Next(cities.Count)];
            return (origin, destination);
        }
    }

    public static class UserPredefinedData
    {
        private static readonly (string firstName, string lastName)[] _names = {
            ("Sara", "Rezaei"),
            ("Amir", "Hasani"),
            ("Ali", "Karimi"),
            ("Zahra", "Sadeghi"),
            ("Elnaz", "Moridi"),
            ("Zivar", "Kafi"),
            ("Ala", "Mohseni"),
            ("Hani", "Najafi"),
            ("Rima", "Safi"),
            ("Rosha", "Abbasi")
        };

        public static List<User> GetUsers() =>
            _names.Select((name, index) => new User(true)
            {
                FirstName = name.firstName,
                LastName = name.lastName,
                UserName = $"{name.firstName}.{name.lastName}".ToLower(),
                MobileNumber = DataGeneratorHelpers.RandomMobile(),
                Password = $"P@ssw0rd{index + 1}",
                Email = $"{name.firstName}{name.lastName}563@gmail.com".ToLower(),
                BirthDate = DataGeneratorHelpers.RandomDate(-12000, -8000),
                IsActive = true,
                RegisterDate = DataGeneratorHelpers.RandomDate(-100, -1),
                CreateDateTime = DateTimeOffset.UtcNow,
                CreatedBy = "System"
            }).ToList();
    }

    public static class FlightPredefinedData
    {
        public static List<Flight> GetFlights()
        {
            var flights = new List<Flight>();

            for (int i = 0; i < 10; i++)
            {
                var departureTime = DateTime.UtcNow.AddDays(i / 2 + 1).Date.AddHours(8 + i % 8);
                var (origin, destination) = DataGeneratorHelpers.RandomCities();

                flights.Add(new Flight(true)
                {
                    AirlineName = DataConstants.Airlines.Names[i % DataConstants.Airlines.Names.Length],
                    Origin = origin,
                    Destination = destination,
                    DepartureTime = departureTime,
                    ArrivalTime = departureTime.AddHours(1.5),
                    AvailableSeats = 100 + (i * 10),
                    Price = DataGeneratorHelpers.RandomPrice(),
                    CreatedBy = "System"
                });
            }

            return flights;
        }
    }

    public static class FlightBookPredefinedData
    {
        public static List<FlightBook> GetFlightBooks(List<User> users, List<Flight> flights)
        {
            return Enumerable.Range(0, 10).Select(i => new FlightBook(true)
            {
                FlightId = flights[i].Id,
                UserId = users[i].Id,
                BookingDate = DateTimeOffset.UtcNow.AddDays(-10 + i),
                TotalPrice = flights[i].Price,
                IsActive = true,
                CreateDateTime = DateTimeOffset.UtcNow.AddDays(-10 + i),
                CreatedBy = "System"
            }).ToList();
        }
    }

    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(FlyDbContext context)
        {
            try
            {
                if (!await context.User.AnyAsync())
                {
                    var users = UserPredefinedData.GetUsers();
                    await context.User.AddRangeAsync(users);
                    await context.SaveChangesAsync();

                    var flights = FlightPredefinedData.GetFlights();
                    await context.Flight.AddRangeAsync(flights);
                    await context.SaveChangesAsync();

                    var bookings = FlightBookPredefinedData.GetFlightBooks(users, flights);
                    await context.FlightBook.AddRangeAsync(bookings);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}