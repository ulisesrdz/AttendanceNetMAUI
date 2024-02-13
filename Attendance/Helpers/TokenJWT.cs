using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZXing.Aztec.Internal;

namespace Attendance.Helpers
{
    public static class TokenJWT
    {
        public static bool ValidateToken(string jwtToken)
        {
            //var tokenHandler = new JwtSecurityTokenHandler();

            string secretKey = Session._secretKey;
            //var key = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
            //var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuer = false, // Puedes configurar según tus necesidades
            //    ValidateAudience = false, // Puedes configurar según tus necesidades
            //    ValidateLifetime = true,
            //    IssuerSigningKey = key
            //};

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // Puedes configurar esto según tus necesidades
                ValidateAudience = false, // Puedes configurar esto según tus necesidades
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validar y deserializar el token
                //var handler = new JwtSecurityTokenHandler();
                //ClaimsPrincipal principal = handler.ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken validatedToken);

                //Console.WriteLine("Token válido");

                SecurityToken securityToken;
                var principals = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out securityToken);

                // Obtener información del token
                if (securityToken is JwtSecurityToken jwtSecurityToken)
                {
                    Console.WriteLine("Token válido");
                    Console.WriteLine($"Issuer: {jwtSecurityToken.Issuer}");
                    Console.WriteLine($"Audience: {jwtSecurityToken.Audiences.FirstOrDefault()}");
                    Console.WriteLine($"Subject: {jwtSecurityToken.Subject}");
                    Console.WriteLine($"Issued At: {jwtSecurityToken.ValidFrom}");
                    Console.WriteLine($"Expires At: {jwtSecurityToken.ValidTo}");
                }

                return true;
            }
            catch (SecurityTokenValidationException ex)
            {
                Console.WriteLine($"Error de validación del token: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return false;
            }
        }
    }
}
