using Microsoft.AspNetCore.Mvc;

namespace Dapper_BigData.Controllers
{
    public class UIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
