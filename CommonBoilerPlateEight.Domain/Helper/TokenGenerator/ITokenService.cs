using CommonBoilerPlateEight.Domain.Models;
using System.Security.Claims;

namespace CommonBoilerPlateEight.Domain.Helper
{
    public interface ITokenService
    {
        string GenerateAccessToken(TokenModel tokenModel);
    }
}
