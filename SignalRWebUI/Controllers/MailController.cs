using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp; // MailKit'in SmtpClient'ini kullanıyoruz
using SignalRWebUI.Dtos.MailDtos;

namespace SignalRWebUI.Controllers
{
	public class MailController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(CreateMailDto createMailDto)
		{
			// MimeMessage oluşturuluyor
			MimeMessage mimeMessage = new MimeMessage();

			// Gönderen adres
			MailboxAddress mailboxAddressFrom = new MailboxAddress("OG Restaurant Rezervasyon", "fenerli7070@gmail.com");
			mimeMessage.From.Add(mailboxAddressFrom);

			// Alıcı adres
			MailboxAddress mailboxAddressTo = new MailboxAddress("User", createMailDto.ReceiverMail);
			mimeMessage.To.Add(mailboxAddressTo);

			// Mail içeriği
			var bodyBuilder = new BodyBuilder
			{
				TextBody = createMailDto.Body // Mail'in gövde içeriği
			};
			mimeMessage.Body = bodyBuilder.ToMessageBody();

			// Konu
			mimeMessage.Subject = createMailDto.Subject;

			// SMTP istemcisiyle mail gönderimi
			using (var client = new SmtpClient())
			{
				client.Connect("smtp.gmail.com", 587, false);

				// Gmail hesap bilgileri (Kendi bilgilerinizi buraya ekleyin)
				client.Authenticate("fenerli7070@gmail.com", "dych deyp hhjo ajba");

				// Mail gönderimi
				client.Send(mimeMessage);

				// Bağlantıyı kapat
				client.Disconnect(true);
			}

			return RedirectToAction("Index", "Category");
		}
	}
}
