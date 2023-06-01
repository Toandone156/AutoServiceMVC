using AutoServiceMVC.Utils;
using System.Net;
using System.Text;

namespace AutoServiceMVC.Models.System
{
	public class VnpayLib
	{
		private SortedList<string, string> _requestData = new SortedList<string, string>();
		private SortedList<string, string> _responseData = new SortedList<string, string>();

		public void AddRequestData(string key, string value) => _requestData.Add(key, value);

		public string CreateRequestUrl(string baseUrl, string hashSecret)
		{
			StringBuilder data = new StringBuilder();
			foreach (KeyValuePair<string, string> kv in _requestData)
			{
				if (!String.IsNullOrEmpty(kv.Value))
				{
					data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
				}
			}
			string queryString = data.ToString();

			baseUrl += "?" + queryString;
			String signData = queryString;
			if (signData.Length > 0)
			{

				signData = signData.Remove(data.Length - 1, 1);
			}
			string vnp_SecureHash = PaymentUtil.HmacSHA512(hashSecret, signData);
			baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

			return baseUrl;
		}

		public void AddResponseData(string key, string value) => _responseData.Add(key, value);
		public string GetResponseData(string key) => _responseData[key];
		public string GetResponseData()
		{
			StringBuilder data = new StringBuilder();
			if (_responseData.ContainsKey("vnp_SecureHashType"))
			{
				_responseData.Remove("vnp_SecureHashType");
			}
			if (_responseData.ContainsKey("vnp_SecureHash"))
			{
				_responseData.Remove("vnp_SecureHash");
			}
			foreach (KeyValuePair<string, string> kv in _responseData)
			{
				if (!String.IsNullOrEmpty(kv.Value))
				{
					data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
				}
			}
			//remove last '&'
			if (data.Length > 0)
			{
				data.Remove(data.Length - 1, 1);
			}
			return data.ToString();
		}

		public bool ValidateSignature(string inputHash, string secretKey)
		{
			string rspRaw = GetResponseData();
			string myChecksum = PaymentUtil.HmacSHA512(secretKey, rspRaw);
			return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
		}

	}
}
