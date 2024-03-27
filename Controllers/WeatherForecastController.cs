using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public string GenerateToken(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim("UID", userId),
            };

            // ���o JWT �� Secret Key
            var secret = "ba8347e6-3504-460d-979e-7a4555c890ee";

            // �N Secret Key �ഫ�� byte �}�C
            var key = Encoding.ASCII.GetBytes(secret);

            var credentials =
             new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            // �إ� JWT Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(
                issuer: "test", // �]�w�o���
                audience: "test", // �]�w������
                subject: new ClaimsIdentity(claims), // �]�w Claim
                expires: DateTime.UtcNow.AddMinutes(30), // �]�w�L���ɶ�
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(securityToken);
        }
    }
}