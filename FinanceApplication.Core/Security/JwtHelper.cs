using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinanceApplication.Core.Extensions;
using FinanceApplication.Core.Result;
using FinanceApplication.Entities.Dto.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinanceApplication.Core.Security;

public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private int _accessTokenExpiration;
        private IHttpContextAccessor _context;

        public JwtHelper(IConfiguration configuration, IHttpContextAccessor context)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            _accessTokenExpiration = _tokenOptions.AccessTokenExpiration;
            _context = context;
        }
        
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, UserDto user, SigningCredentials signingCredentials, List<byte> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: DateTime.Now.AddDays(_accessTokenExpiration),
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
                );
            return jwt;
        }
        private IEnumerable<Claim> SetClaims(UserDto user, List<byte> roleList)
        {
            var claimList = new List<Claim>();
            claimList.AddNameIdentifier(user.Id.ToString());
            claimList.AddRoles(roleList.ToArray());
            claimList.AddTaxId(user.TaxId);
            return claimList;
        }

        public AccessToken CreateToken(UserDto user)
        {
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, new List<byte>{ user.RoleId });
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = jwt.ValidTo
            };
        }
        
        public IDataResult<TokenInfo> GetTokenInfo(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var decryptedToken = new JwtSecurityToken(jwtEncodedString: token);
                var userId = decryptedToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var roleId = decryptedToken.Claims.First(c => c.Type == ClaimTypes.Role).Value;
                var taxId = decryptedToken.Claims.First(c => c.Type == ClaimTypes.UserData).Value;

                TokenInfo tokenInfo = new()
                {
                    RoleId = roleId,
                    StampBegin = decryptedToken.ValidFrom,
                    StampExpiration = decryptedToken.ValidTo,
                    UserId = userId,
                };

                if (tokenInfo.StampExpiration < DateTime.UtcNow)
                {
                    return new ErrorDataResult<TokenInfo>(null, "Tokenın süresi geçmiş.", StatusCodes.Status401Unauthorized);
                }
                
                UserIdentityHelper.SetUserInfo(tokenInfo.UserId, tokenInfo.RoleId, taxId);
                
                return new SuccessDataResult<TokenInfo>(tokenInfo);
            }
            return new ErrorDataResult<TokenInfo>(null, "Geçersiz token.");
        }
    }