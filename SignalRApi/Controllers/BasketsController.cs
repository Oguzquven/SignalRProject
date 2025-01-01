using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DtoLayer.BasketDto;
using SignalR.EntityLayer.Entities;
using SignalRApi.Models;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly SignalRContext _context; // Veritabanı bağlantısı için ekleme

        public BasketsController(IBasketService basketService, SignalRContext context)
        {
            _basketService = basketService;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetBasketByMenuTableID(int id)
        {
            var values = _basketService.TGetBasketByMenuTableNumber(id);
            return Ok(values);
        }

        [HttpGet("BasketListByMenuTableWithProductName")]
        public IActionResult BasketListByMenuTableWithProductName(int id)
        {
            var values = _context.Baskets.Include(x => x.Product)
                .Where(y => y.MenuTableID == id)
                .Select(z => new ResultBasketListWithProducts
                {
                    BasketID = z.BasketID,
                    Count = z.Count,
                    MenuTableID = z.MenuTableID,
                    Price = z.Price,
                    TotalPrice = z.TotalPrice,
                    ProductName = z.Product.ProductName,
                    ProductID = z.Product.ProductID
                }).ToList();

            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateBasket(CreateBasketDto createBasketDto)
        {
            var price = _context.Products
                .Where(x => x.ProductID == createBasketDto.ProductID)
                .Select(y => y.Price)
                .FirstOrDefault();

            if (price == 0)
                return BadRequest("Ürün bulunamadı.");

            var basket = new Basket
            {
                ProductID = createBasketDto.ProductID,
                MenuTableID = createBasketDto.MenuTableID,
                Count = 1,
                Price = price,
                TotalPrice = createBasketDto.TotalPrice,
            };

            _basketService.TAdd(basket);
            return Ok("Sepete ürün eklendi.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBasket(int id)
        {
            var basket = _basketService.TGetByID(id);

            if (basket == null)
                return NotFound("Sepet ürünü bulunamadı.");

            _basketService.TDelete(basket);
            return Ok("Sepetdeki seçilen ürün silindi.");
        }

        [HttpDelete("DeleteByProductAndMenuTable")]
        public IActionResult DeleteByProductAndMenuTable(int productId, int menuTableId)
        {
            var basketItem = _context.Baskets.FirstOrDefault(x => x.ProductID == productId && x.MenuTableID == menuTableId);

            if (basketItem == null)
                return NotFound("Silinecek ürün bulunamadı.");

            _context.Baskets.Remove(basketItem);
            _context.SaveChanges();
            return Ok("Ürün başarıyla silindi.");
        }

        [HttpPost("CompleteOrder")]
        public IActionResult CompleteOrder(int menuTableId)
        {
            // 1. Sepeti temizle
            var baskets = _context.Baskets.Where(x => x.MenuTableID == menuTableId).ToList();

            if (!baskets.Any())
                return NotFound("Sepet zaten boş.");

            _context.Baskets.RemoveRange(baskets);

            // 2. Masa durumunu false yap
            var menuTable = _context.MenuTables.FirstOrDefault(x => x.MenuTableID == menuTableId);

            if (menuTable == null)
                return NotFound("Masa bulunamadı.");

            menuTable.Status = false;

            // Veritabanında değişiklikleri kaydet
            _context.SaveChanges();

            return Ok("Sipariş tamamlandı. Sepet temizlendi ve masa boşaltıldı.");
        }
    }
}
