using CineManager_Web.Models; // Namespace per i modelli (es. Sala)
using CineManagerWeb.Models;  // Nota: controlla se hai due namespace simili, potrebbe crear confusione
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics;

namespace CineManager_Web.Controllers
{
    // Il Controller gestisce le interazioni tra l'utente (Vista) e i dati (Modello)
    public class CinemaController : Controller
    {
        private static int _totalePostiInizialeSala1 = 20;
        private static int _totalePostiInizialeSala2 = 5;

        private static List<Sala> _sale = new List<Sala> {
            new Sala { Id = 1, Film = "Il Glossario", PostiLiberi = _totalePostiInizialeSala1, TotalePosti = _totalePostiInizialeSala1, NumeroSala = 1, PrezzoBiglietto = 9},
            new Sala { Id = 2, Film = "C# Revenge", PostiLiberi = _totalePostiInizialeSala2, TotalePosti = _totalePostiInizialeSala2, NumeroSala = 2, PrezzoBiglietto = 12 }
        };
        
        

        // Logger per registrare eventi o errori (generato di default da VS)
        private readonly ILogger<CinemaController> _logger;
        
        public CinemaController(ILogger<CinemaController> logger)
        {
            _logger = logger;
        }

        // --- AZIONI DI SOLA LETTURA (VIEW) ---

        // Mostra la lista delle sale per gli utenti comuni
        public IActionResult Index()
        {
            return View(_sale); // Passa la lista delle sale alla View "Index.cshtml"
        }

        // Mostra la lista delle sale per l'amministratore (Pannello di controllo)
        public IActionResult ProgrammazioneIndex()
        {
            return View(_sale); // Passa la lista alla View "ProgrammazioneIndex.cshtml"
        }

        // --- AZIONI DI LOGICA (PRENOTAZIONI E DISDETTE) ---

        // Metodo per prenotare un posto (lato Utente)
        public IActionResult Prenota(int id)
        {
            // Cerca nella lista la sala che ha l'ID corrispondente a quello cliccato
            var sala = _sale.Find(s => s.Id == id);

            // Se la sala esiste e c'è almeno un posto libero
            if (sala != null && sala.PostiLiberi > 0)
            {
                sala.OccupaPosto(); // Esegue il metodo nel modello per scalare il posto
            }

            // Dopo l'operazione, reindirizza l'utente alla pagina principale per vedere l'aggiornamento
            return RedirectToAction("Index");
        }

        // Metodo per prenotare un posto (lato Admin)
        public IActionResult PrenotaAdmin(int id)
        {
            var sala = _sale.Find(s => s.Id == id);

            if (sala != null && sala.PostiLiberi > 0)
            {
                sala.OccupaPosto();
            }

            // A differenza di Prenota, questo torna alla pagina della programmazione admin
            return RedirectToAction("ProgrammazioneIndex");
        }

        // Metodo per liberare un posto occupato (Admin)
        public IActionResult Disdici(int id)
        {
            var sala = _sale.Find(s => s.Id == id);

            // Verifica che la sala esista e che ci sia effettivamente qualcuno da "sfrattare"
            if (sala != null && sala.PostiOccupati > 0)
            {
                sala.LiberaPosti(); // Metodo nel modello per incrementare i PostiLiberi
            }

            return RedirectToAction("ProgrammazioneIndex");
        }


        public IActionResult DiminuisciPostiTot(int id)
        {
            var sala = _sale.Find(s => s.Id == id);

            if(sala != null && sala.TotalePosti > 0)
            {
                sala.DiminuisciMax();
            }

            return RedirectToAction("ProgrammazioneIndex");
        }

        public IActionResult AumentaPostiTot(int id)
        {
            var sala = _sale.Find(s => s.Id == id);

            if (sala != null && sala.TotalePosti < 50)
            {
                sala.AumentaMax();
            }

            return RedirectToAction("ProgrammazioneIndex");
        }


        // --- GESTIONE LOGIN ---

        // [HttpGet] - Carica semplicemente la pagina con il form di login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // [HttpPost] - Riceve i dati (email e password) inviati dal form
        // Nota: Nel tuo codice originale il metodo sotto non aveva l'attributo esplicito sopra, 
        // ma gestisce la logica di autenticazione.
        public IActionResult Login(string email, string password)
        {
            // Simulazione di autenticazione (Hardcoded)
            if (email == "admin@cinema.it" && password == "1234")
            {
                // Se i dati sono corretti, manda l'utente all'area riservata
                return RedirectToAction("ProgrammazioneIndex", "Cinema");
            }
            else
            {
                // Se i dati sono errati, crea un messaggio di errore e ricarica la pagina di login
                ViewBag.Errore = "Email o Password non validi!";
                return View();
            }
        }

        public override string ToString() 
        {
             
            return $"Sala1 {""}";
        }

        public IActionResult Reset()
        {
            _sale[0].TotalePosti = _totalePostiInizialeSala1;
            _sale[1].TotalePosti = _totalePostiInizialeSala2;
            _sale[0].PostiLiberi = _totalePostiInizialeSala1;
            _sale[1].PostiLiberi = _totalePostiInizialeSala2;
            _sale[0].Incassi = 0;
            _sale[1].Incassi = 0;

            return RedirectToAction("ProgrammazioneIndex");
        }

        public IActionResult Salva()
        {
            using (StreamWriter sw = new StreamWriter("sale.txt", true))
            {
                foreach ( Sala s in _sale)
                {
                    sw.WriteLine($"Sala{s.Id}: {_sale[s.Id-1].Incassi}€ alle {DateAndTime.Now}");
                }
                
            }
            return RedirectToAction("ProgrammazioneIndex");
        }

        // --- GESTIONE ERRORI ---

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}