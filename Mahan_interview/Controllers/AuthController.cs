using Domain.Commons.Contract;
using Domain.Commons.Entities;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;  
    public AuthController(IConfiguration configuration, IJwtService jwtService, IUserRepository userRepository)
    {
        _configuration = configuration;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel login)
    {
        if (login == null || string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
        {
            return BadRequest("Username and password are required");
        }

        var user = await AuthenticateUser(login.UserName, login.Password);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = _jwtService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private async Task<User> AuthenticateUser(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user != null && user.Password == password) 
        {
            return user;
        }

        return null;  
    }
}
