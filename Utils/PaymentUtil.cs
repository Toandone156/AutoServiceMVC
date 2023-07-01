using Microsoft.AspNetCore.Http.Features;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AutoServiceMVC.Utils
{
	public static class PaymentUtil
	{
		public static string HmacSHA512(string key, string inputData)
		{
			var hash = new StringBuilder();
			byte[] keyBytes = Encoding.UTF8.GetBytes(key);
			byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);

			using(var hmac = new HMACSHA512(keyBytes))
			{
				byte[] hashValue = hmac.ComputeHash(inputBytes);
				foreach(byte b in hashValue)
				{
					hash.Append(b.ToString("x2"));
				}
			}

			return hash.ToString();
		}

		public static string GetClientIPAddress(this HttpContext context)
		{
			string ip = string.Empty;
			if (!string.IsNullOrWhiteSpace(context.Request.Headers["X-Forwarded-For"]))
			{
				ip = context.Request.Headers["X-Forwarded-For"];
			}
			else
			{
				ip = context.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
			}
			return ip;
		}
	}
}

