using restful_api_joaodias.Business.Interfaces;
using restful_api_joaodias.Configurations;
using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Repository.UserRepo;
using restful_api_joaodias.Services.Token.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace restful_api_joaodias.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd";

        private TokenConfiguration _configuration;
        private IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public LoginBusinessImplementation(
            TokenConfiguration configuration,
            IUserRepository repository,
            ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {
            var user = _repository.ValidateCredentials(userCredentials);
            if (user == null)
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, userCredentials.UserName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenUntilExpirationTime = DateTime.Now.AddDays(_configuration.DaysUntilExpiration);

            _repository.RefreshUserInfo(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.MinutesUntilExpiration);

            return new TokenVO(
                accessToken,
                refreshToken,
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT));
        }

        public TokenVO ValidateCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var userName = principal?.Identity?.Name;

            var user = _repository.ValidateCredentials(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenUntilExpirationTime <= DateTime.Now)
            {
                return null;
            }

            accessToken = _tokenService.GenerateAccessToken(principal?.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _repository.RefreshUserInfo(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.MinutesUntilExpiration);

            return new TokenVO(
                accessToken,
                refreshToken,
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT));
        }

        public bool RevokeToken(string userName)
        {
            return _repository.RevokeToken(userName);
        }
    }
}
