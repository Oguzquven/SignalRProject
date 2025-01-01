using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DataAccessLayer.Concrete;
using SignalR.EntityLayer.Entities;
using SignalRWebUI.Dtos.BasketDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
	public class BasketsController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public BasketsController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

        // Sepet Listesi
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.MenuTableId = id; // Masanın ID'sini Razor View'a gönderiyoruz

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7010/api/Baskets/BasketListByMenuTableWithProductName?id={id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultBasketDto>>(jsonData);
                return View(values); // Razor View'a sepet verilerini gönderiyoruz
            }

            TempData["ErrorMessage"] = "Sepet verileri yüklenemedi.";
            return View();
        }


        // Ürün Silme
        [HttpPost]
        public async Task<IActionResult> DeleteBasket(int productId, int menuTableId)
        {
            var client = _httpClientFactory.CreateClient();

			
			
            // API'ye silme isteği gönderiyoruz
            var responseMessage = await client.DeleteAsync($"https://localhost:7010/api/Baskets/DeleteByProductAndMenuTable?productId={productId}&menuTableId={menuTableId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Ürün başarıyla silindi.";
                return RedirectToAction("Index", new { id = menuTableId });
            }

            // Hata durumunda API'den dönen hata mesajını al
            var errorDetails = await responseMessage.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"API Hatası: {errorDetails}";
            return RedirectToAction("Index", new { id = menuTableId });
        }
        [HttpPost("ClearBasket")]
        public IActionResult ClearBasket(int menuTableId)
        {
            using var context = new SignalRContext();

            var baskets = context.Baskets.Where(x => x.MenuTableID == menuTableId).ToList();
            if (!baskets.Any())
            {
                return NotFound("Sepet zaten boş.");
            }

            context.Baskets.RemoveRange(baskets);
            context.SaveChanges();

            return Ok("Sepet başarıyla temizlendi.");
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int menuTableId)
        {
            if (menuTableId == 0)
            {
                TempData["ErrorMessage"] = "Geçersiz Masa ID'si.";
                return RedirectToAction("Index", new { id = menuTableId });
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync($"https://localhost:7010/api/Baskets/CompleteOrder?menuTableId={menuTableId}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sipariş tamamlandı ve masa boşaltıldı.";
                return RedirectToAction("CustomerTableList", "CustomerTable"); // Masalar sayfasına yönlendirme
            }

            TempData["ErrorMessage"] = "Sipariş tamamlanırken bir hata oluştu.";
            return RedirectToAction("Index", new { id = menuTableId });
        }



    }
}
