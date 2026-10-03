using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;

namespace InsureYouAI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly InsureContext _context;
        public ArticleController(InsureContext context)
        {
            _context = context;
        }
        public IActionResult ArticleList()
        {
            ViewBag.ControllerName = "Makaleler";
            ViewBag.PageName = "Makale Listesi";
            var values = _context.Articles.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateArticle()
        {
            ViewBag.ControllerName = "Makaleler";
            ViewBag.PageName = "Yeni Makale Oluştur";

            return View();
        }

        [HttpPost]
        public IActionResult CreateArticle(Article article)
        {
            article.CreatedDate = DateTime.Now;
            _context.Articles.Add(article);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public IActionResult UpdateArticle(int id)
        {

            ViewBag.ControllerName = "Makaleler";
            ViewBag.PageName = "Makale Güncelleme Sayfası";

            var value = _context.Articles.Find(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }

        public IActionResult DeleteArticle(int id)
        {
            var value = _context.Articles.Find(id);
            _context.Articles.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public IActionResult CreateArticleWithOpenAI()
        {
            ViewBag.ControllerName = "Makaleler";
            ViewBag.PageName = "Yapay Zeka Makale Oluşturucu";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticleWithOpenAI(string prompt)
        {
            var apiKey = "";

            using var client = new HttpClient(); // istemci 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey); // istek başlığına API anahtarını ekle

            var requestData = new // istek verisi
            {
                model = "gpt-3.5-turbo", // kullanılacak model
                messages = new[]   // mesajlar
                {
                    new {role="system",content="Sen bir sigorta şirketi için çalışan, içerik yazarlığı yapan bir yapay zekasın. Kullanıcının verdiği özet ve anahtar kelimelere göre, sigortacılık sektörüyle ilgili makale üret. En az 3000 karakter olsun."},
                    new{role="user",content=prompt }
                },
                temperature = 0.7 // yaratıcılık seviyesi
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData); // istek gönder

            if (response.IsSuccessStatusCode) // başarılı yanıt alındıysa
            {
                var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                var content = result.choices[0].message.content;
                ViewBag.article = content;
            }
            else
            {
                ViewBag.article = "Bir hata oluştu: " + response.StatusCode;
            }
            return View();
        }

        public class OpenAIResponse
        {
            public List<Choice> choices { get; set; }
        }
        public class Choice
        {
            public Message message { get; set; }
        }
        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }
    }
}


