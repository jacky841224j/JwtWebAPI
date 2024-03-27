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

            // 取得 JWT 的 Secret Key
            var secret = "ba8347e6-3504-460d-979e-7a4555c890ee";

            // 將 Secret Key 轉換為 byte 陣列
            var key = Encoding.ASCII.GetBytes(secret);

            var credentials =
             new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            // 建立 JWT Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(
                issuer: "test", // 設定發行者
                audience: "test", // 設定接收者
                subject: new ClaimsIdentity(claims), // 設定 Claim
                expires: DateTime.UtcNow.AddMinutes(30), // 設定過期時間
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(securityToken);
        }
    }
}