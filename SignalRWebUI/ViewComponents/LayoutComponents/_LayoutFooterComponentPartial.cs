using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SignalRWebUI.ViewComponents.LayoutComponents
{
	public class _LayoutFooterComponentPartial:ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
