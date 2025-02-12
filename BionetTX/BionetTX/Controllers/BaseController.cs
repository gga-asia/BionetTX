using Microsoft.AspNetCore.Mvc;

namespace BionetTX.Controllers
{
	public class BaseController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
