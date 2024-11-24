using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CommonBoilerPlateEight.Domain.Constants;
using CommonBoilerPlateEight.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CommonBoilerPlateEight.Domain.Helper
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private string _secretKey;
        private string _expiryTimeInMinutes;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = configuration.GetSection("JwtConfig").GetSection("Key").Value;
        }
        public string GenerateAccessToken(TokenModel tokenModel)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                {
                new Claim(ClaimTypeConstant.ClaimPhoneNumber, tokenModel.PhoneNumber),
                new Claim(ClaimTypeConstant.ClaimCountryId, tokenModel.CountryId.ToString()),
                new Claim(ClaimTypeConstant.ClaimEmail, tokenModel.Email),
                new Claim(ClaimTypeConstant.ClaimUserType,tokenModel.UserType),
                new Claim(JwtRegisteredClaimNames.Sub, tokenModel.Id.ToString())
                };
            var tokeOptions = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

    }
}
