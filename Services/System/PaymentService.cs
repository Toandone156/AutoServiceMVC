using AutoServiceMVC.Models.System;
using AutoServiceMVC.Utils;
using Org.BouncyCastle.Asn1.Ocsp;
using static Humanizer.On;

namespace AutoServiceMVC.Services.System
{
	public interface IPaymentService
	{
		public string GetVnpayPaymentUrl(HttpContext context, int amount);
		public (bool status, string? responseCode) CheckResponseVnpayPayment(HttpContext context);
	}

	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _config;

		public PaymentService(IConfiguration config)
		{
			_config = config;
		}

		public (bool status, string? responseCode) CheckResponseVnpayPayment(HttpContext context)
		{
			(bool status, string? responeCode) value = (false, null);

			var vnpayData = context.Request.Query;

			if (vnpayData.Count < 1) return value;

			string hashSecret = _config.GetSection("Payment")["Vnpay:HashSecret"];

			VnpayLib vnpay = new VnpayLib();

			//lấy toàn bộ dữ liệu được trả về
			foreach (var s in vnpayData)
			{
				if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
				{
					vnpay.AddResponseData(s.Key, s.Value);
				}
			}

			long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
			long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
			value.responeCode = vnpay.GetResponseData("vnp_ResponseCode");
			string vnp_SecureHash = vnpayData["vnp_SecureHash"];

			//value.status = vnpay.ValidateSignature(vnp_SecureHash, hashSecret);
			value.status = true;

			return value;
		}

		public string GetVnpayPaymentUrl(HttpContext context,int amount)
		{
			var section = _config.GetSection("Payment");
			var url = section["Vnpay:Url"];
			var returnUrl = section["Vnpay:ReturnUrl"];
			var tmnCode = section["Vnpay:TmnCode"];
			var hashSecret = section["Vnpay:HashSecret"];

			VnpayLib vnpay = new VnpayLib();
			vnpay.AddRequestData("vnp_Version", "2.1.0");
			vnpay.AddRequestData("vnp_Command", "pay");
			vnpay.AddRequestData("vnp_TmnCode", tmnCode);
			vnpay.AddRequestData("vnp_Amount", Convert.ToString(amount * 100)); //Vnpay need mutil with 100
			vnpay.AddRequestData("vnp_BankCode", "");
			vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
			vnpay.AddRequestData("vnp_CurrCode", "VND");
			vnpay.AddRequestData("vnp_IpAddr", PaymentUtil.GetClientIPAddress(context));
			vnpay.AddRequestData("vnp_Locale", "vn");
			vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan hoa don tai shop"); //Thông tin mô tả nội dung thanh toán
			vnpay.AddRequestData("vnp_OrderType", "other");
			vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
			vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

			return vnpay.CreateRequestUrl(url, hashSecret);
		}
	}
}
