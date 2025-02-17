using Domain.Commons;
using Domain.Commons.Contract;
using Domain.Commons.Repository;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<FlyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConectionString")));
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IFlightBookRepository, FlightBookRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Configuration.GetConnectionString("AppConnectionString");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], 
            ValidAudience = builder.Configuration["Jwt:Audience"], 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });


builder.Services.AddScoped<IEmailService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>(); 

    if (int.TryParse(configuration["Email:SmtpPort"], out int smtpPort))
    {
        return new EmailService(
            configuration["Email:SmtpServer"],
            configuration["Email:SmtpUser"],
            configuration["Email:SmtpPassword"],
            smtpPort);
    }
    else
    {
        var logger = provider.GetRequiredService<ILogger<Program>>(); // Get logger
        logger.LogError("Invalid SMTP port configuration value.");
        throw new Exception("Invalid SMTP port configuration."); // Or provide a default value
    }
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
foreach (var config in builder.Configuration.AsEnumerable())
{
    Console.WriteLine($"{config.Key} = {config.Value}");
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FlyDbContext>();
    await MockData.SeedData(dbContext);
}
if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

