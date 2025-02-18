using Domain.Commons;
using Domain.Commons.Contract;
using Domain.Commons.Repository;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppConnectionString");

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<FlyDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IFlightBookRepository, FlightBookRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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
        var logger = provider.GetRequiredService<ILogger<Program>>(); 
        logger.LogError("Error");
        return new EmailService(
            configuration["Email:SmtpServer"],
            configuration["Email:SmtpUser"],
            configuration["Email:SmtpPassword"],
            587); 
    }
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FlyDbContext>();
    try
    {
        await dbContext.Database.MigrateAsync();
        await DatabaseSeeder.SeedDatabaseAsync(dbContext);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error in insert data.");
    }
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

