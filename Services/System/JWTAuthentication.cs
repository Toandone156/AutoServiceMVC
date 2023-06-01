using AutoServiceMVC.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutoServiceMVC.Services.System
{
	public interface IJWTAuthentication
	{
		string GenerateToken(string data);
		string ValidateToken(string token);
	}

	public class JWTAuthentication : IJWTAuthentication
	{
		public string GenerateToken(string data)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes("ThisIsSecretForAutoServiceSE1707");
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim("Data", data)
				}),
				Expires = DateTime.UtcNow.AddMinutes(5),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public string? ValidateToken(string token)
		{
			if (token == null) return null;

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes("ThisIsSecretForAutoServiceSE1707");
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var data = jwtToken.Claims.First(x => x.Type == "Data").Value;

				return data;
			}
			catch
			{
				return null;
			}
		}
	}
}
