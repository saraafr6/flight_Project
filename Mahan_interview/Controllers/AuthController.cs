using Domain.Commons.Contract;
using Domain.Commons.Entities;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService; 

    public AuthController(IConfiguration configuration, IJwtService jwtService)
    {
        _configuration = configuration;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginModel login)
    {
      
        var user = AuthenticateUser(login.UserName, login.Password);
        if (user == null)
            return Unauthorized("Invalid credentials");

    
        var token = _jwtService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private User AuthenticateUser(string username, string password)
    {
       
        return new User { UserName = username };  
    }
}
