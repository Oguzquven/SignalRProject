using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.DiscountDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
	public class DiscountController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public DiscountController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7010/api/Discount");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultDiscountDto>>(jsonData);
				return View(values);
			}
			return View();
		}
		[HttpGet]
		public IActionResult CreateDiscount()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateDiscount(CreateDiscountDto createDiscountDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createDiscountDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7010/api/Discount", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}
		public async Task<IActionResult> DeleteDiscount(int id)
		{
			var client = _httpClientFactory.CreateClient();
			try
			{
				var responseMessage = await client.DeleteAsync($"https://localhost:7010/api/Discount/{id}");

				// Başarılı durum
				if (responseMessage.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}

				// Hata durumu
				var errorContent = await responseMessage.Content.ReadAsStringAsync();
				Console.WriteLine($"Error Status Code: {responseMessage.StatusCode}");
				Console.WriteLine($"Error Content: {errorContent}");

				// Hata mesajını ViewBag ile görünümde göster
				ViewBag.ErrorMessage = "Discount silinemedi. Lütfen tekrar deneyin.";
				ViewBag.ApiErrorDetails = errorContent;
				return View("Error"); // Hata görünümüne yönlendirme
			}
			catch (Exception ex)
			{
				// İstisna durumunda loglama ve hata mesajı
				Console.WriteLine($"Exception: {ex.Message}");
				ViewBag.ErrorMessage = "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
				return View("Error");
			}
		}

		[HttpGet]
		public async Task<IActionResult> UpdateDiscount(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7010/api/Discount/{id}");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateDiscountDto>(jsonData);
				return View(values);
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> UpdateDiscount(UpdateDiscountDto updateDiscountDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsondata = JsonConvert.SerializeObject(updateDiscountDto);
			StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7010/api/Discount/", stringContent);
			if ((responseMessage.IsSuccessStatusCode))
			{
				return RedirectToAction("Index");
			}
			return View();
		}

        public async Task<IActionResult> ChangeStatusToTrue(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.GetAsync($"https://localhost:7010/api/Discount/ChangeStatusToTrue/{id}");
			return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangeStatusToFalse(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.GetAsync($"https://localhost:7010/api/Discount/ChangeStatusToFalse/{id}");
            return RedirectToAction("Index");
        }

    }
}
