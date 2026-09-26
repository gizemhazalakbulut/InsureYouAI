using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
