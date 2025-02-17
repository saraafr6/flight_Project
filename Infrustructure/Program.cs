// See https://aka.ms/new-console-template for more information
using Infrastructure.Data;
using Infrustructure.Data;
using Microsoft.EntityFrameworkCore;



Console.WriteLine("Hello, World!");
using var db = new MigrateDbContext();
db.Database.SetCommandTimeout(500);
db.Database.Migrate();
