using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUI.ViewComponents.LayoutComponents
{
	public class _LayoutSidebarCompenentPartial:ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
