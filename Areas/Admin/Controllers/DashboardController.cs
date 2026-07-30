using Microsoft.AspNetCore.Mvc;

namespace ModernPortfolio.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Admin")]
        // GET: DashboardController
        public ActionResult Index()
        {
            return View();
        }

    }
}
