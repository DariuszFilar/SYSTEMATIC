using Microsoft.AspNetCore.Mvc;

namespace SYSTEMATIC.API.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
