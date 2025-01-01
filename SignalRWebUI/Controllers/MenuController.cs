﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.BasketDtos;
using SignalRWebUI.Dtos.ProductDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MenuController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.v=id;
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7010/api/Product/ProductListWithCategory");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
            return View(values);
        }
		[HttpPost]
		public async Task<IActionResult> AddBasket(int id,int menuTableId)
		{
            if (menuTableId == 0)
            {
                return BadRequest("MenuTableId 0 geliyor.");
            }
            CreateBasketDto createBasketDto = new CreateBasketDto()
            {
                ProductID = id,
                MenuTableID = menuTableId
            };
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createBasketDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7010/api/Baskets", stringContent);

			var client2 = _httpClientFactory.CreateClient();
			//var jsondata = JsonConvert.SerializeObject(updateCategoryDto);
			//StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
			await client2.GetAsync("https://localhost:7010/api/MenuTables/ChangeMenuTableStatusToTrue?id="+menuTableId);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return Json(createBasketDto);
		}
	}
}