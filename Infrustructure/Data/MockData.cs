using Domain.Commons.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MockData
{
    public static async Task SeedData(FlyDbContext context)
    {

    //    if (await context.Flight.AnyAsync() || await context.FlightBooks.AnyAsync() || await context.User.AnyAsync())
    //        return;

    //    var flights = new List<Flight>
    //    {
    //        new Flight(true)
    //        {
    //            AirlineName = "Airline A",
    //            Origin = "New York",
    //            Destination = "Los Angeles",
    //            DepartureTime = DateTime.UtcNow.AddHours(1),
    //            ArrivalTime = DateTime.UtcNow.AddHours(5),
    //            AvailableSeats = 100,
    //            Price = 250.00m
    //        },
    //        new Flight(true)
    //        {
    //            AirlineName = "Airline B",
    //            Origin = "Chicago",
    //            Destination = "San Francisco",
    //            DepartureTime = DateTime.UtcNow.AddHours(2),
    //            ArrivalTime = DateTime.UtcNow.AddHours(4),
    //            AvailableSeats = 150,
    //            Price = 200.00m
    //        }
    //    };

    //    await context.Flight.AddRangeAsync(flights);
    //    await context.SaveChangesAsync();

     
    //    var users = new List<User>
    //    {
    //        new User(true)
    //        {
    //            FirstName = "John",
    //            LastName = "Doe",
    //            MobileNumber = "09123456789",
    //            Email = "john.doe@example.com",
    //            BirthDate = DateTimeOffset.UtcNow.AddYears(-30),
    //            IsActive = true,
    //            RegisterDate = DateTimeOffset.UtcNow,
    //            CreateDateTime = DateTimeOffset.UtcNow
    //        },
    //        new User(true)
    //        {
    //            FirstName = "Jane",
    //            LastName = "Smith",
    //            MobileNumber = "09123456790",
    //            Email = "jane.smith@example.com",
    //            BirthDate = DateTimeOffset.UtcNow.AddYears(-25),
    //            IsActive = true,
    //            RegisterDate = DateTimeOffset.UtcNow,
    //            CreateDateTime = DateTimeOffset.UtcNow
    //        }
    //    };

    //    await context.User.AddRangeAsync(users);
    //    await context.SaveChangesAsync();

        
    //    var flightBooks = new List<FlightBook>
    //    {
    //        new FlightBook(true)
    //        {
    //            FlightId = flights[0].Id,
    //            UserId = users[0].Id,
    //            BookingDate = DateTimeOffset.UtcNow,
    //            TotalPrice = 250.00m,
    //            IsActive = true,
    //            CreateDateTime = DateTimeOffset.UtcNow
    //        },
    //        new FlightBook(true)
    //        {
    //            FlightId = flights[1].Id,
    //            UserId = users[1].Id,
    //            BookingDate = DateTimeOffset.UtcNow,
    //            TotalPrice = 200.00m,
    //            IsActive = true,
    //            CreateDateTime = DateTimeOffset.UtcNow
    //        }
    //    };

    //    await context.FlightBooks.AddRangeAsync(flightBooks);
    //    await context.SaveChangesAsync();
    }
}
