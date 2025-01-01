using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUI.Controllers
{
    public class SignalRDefault : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Index2()
		{
			return View();
		}

	}
}
