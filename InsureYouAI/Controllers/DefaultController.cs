using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.Json.Nodes;
using System.Text;

namespace InsureYouAI.Controllers
{
    public class DefaultController : Controller
    {

        private readonly InsureContext _context;
        public DefaultController(InsureContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult SendMessage()
        {
            return PartialView();
        }

   
    }
}
