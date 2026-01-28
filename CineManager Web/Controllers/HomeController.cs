using System.Diagnostics;
using CineManager_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CineManager_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // 1. Metodo GET: Mostra la pagina di Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Metodo POST: Riceve i dati dal form quando clicchi "Entra"
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // SIMULAZIONE DI CONTROLLO PASSWORD
            // In un'app vera qui controlleresti il database
            if (email == "admin@cinema.it" && password == "1234")
            {
                // Login corretto: Mandalo alla programmazione
                return RedirectToAction("Index", "Cinema");
            }
            else
            {
                // Login errato: Ricarica la pagina con un errore
                ViewBag.Errore = "Email o Password non validi!";
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}