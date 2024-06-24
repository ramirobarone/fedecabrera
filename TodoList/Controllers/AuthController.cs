using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        // In memory users
        private static readonly List<User> _users = new List<User>
        {
            new User { Username = "User1", Password = "XXX" },
            new User { Username = "User2", Password = "YYY" }
        };

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userLogin)
        {

            var user = _users.FirstOrDefault(u => u.Username == userLogin.Username && u.Password == userLogin.Password);


            if (user == null)
            {
                return Unauthorized("Name or password wrong.");
            }

            // Generar un token JWT
            var token = GenerateJwtToken(user);

            // Devolver el token en la respuesta
            return Ok(new { Token = token });
        }


        private string GenerateJwtToken(User user)
        {

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("SecretKey").Value;
            var tokenValidityMinutes = int.Parse(jwtSettings.GetSection("TokenValidityMinutes").Value);


            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username)

            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: "myemisor",
                audience: "audience",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenValidityMinutes),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }




}



