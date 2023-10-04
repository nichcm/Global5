using Global5.Application.Interfaces;
using Global5.Application.Services.Base;
using Global5.Application.ViewModels.Responses.Token;
using Global5.Domain.Entities;
using Global5.Domain.Extensions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.Services
{
    public class TokenService : BaseService, ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService
        (
           IConfiguration configuration,
           ITranslateService translateService) : base(translateService)
        {
            _configuration = configuration;
        }

        public TokenCreateResponse CreateToken(string login, string pass, Users model)
        {
            TokenCreateResponse tokenCreateResponse = null;

            if (model != null)
            {
                if (model.Password == pass)
                {
                    var _secret = _configuration.GetSection("JwtSettings").GetSection("Secret").Value;
                    var _audience = _configuration.GetSection("JwtSettings").GetSection("Audience").Value;
                    var _issuer = _configuration.GetSection("JwtSettings").GetSection("Issuer").Value;
                    var _refreshToken = GenerateRefreshToken();
                    var _expDateToken = DateTime.UtcNow.AddMinutes(2000);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_secret);


                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("UserLoginId", model.Id.ToString()),
                            new Claim("Login", CryptoExtensions.Encrypt(login)),
                            new Claim("Name", CryptoExtensions.Encrypt(model.Name)),
                            new Claim("Email", CryptoExtensions.Encrypt(model.Email)),
                            new Claim("RefreshToken", _refreshToken),
                        }),
                        Issuer = _issuer,
                        Audience = _audience,
                        Expires = _expDateToken,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    tokenCreateResponse = new TokenCreateResponse(tokenHandler.WriteToken(token), _refreshToken, null, true);
                }
            }
            return tokenCreateResponse;
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
