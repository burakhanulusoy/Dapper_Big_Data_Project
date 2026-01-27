using Microsoft.AspNetCore.Mvc;

namespace Dapper_BigData.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
