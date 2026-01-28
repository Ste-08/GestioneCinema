using CineManager_Web.Models; // Assicurati che il namespace sia corretto
using CineManagerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CineManager_Web.Controllers
{
    public class CinemaController : Controller
    {
        // La lista statica dei dati
        private static List<Sala> _sale = new List<Sala> {
            new Sala { Id = 1, Film = "Il Glossario", PostiLiberi = 20, TotalePosti = 20, NumeroSala = 1},
            new Sala { Id = 2, Film = "C# Revenge", PostiLiberi = 5, TotalePosti = 5, NumeroSala = 2}
        };

        public IActionResult Index()
        {
            return View(_sale); // Passa i dati alla Vista
        }

        public IActionResult Prenota(int id)
        {
            var sala = _sale.Find(s => s.Id == id);

            // Se la sala esiste e ci sono posti, occupa il posto
            if (sala != null && sala.PostiLiberi > 0)
            {
                sala.OccupaPosto(); // Assumo che questo metodo diminuisca i PostiLiberi
            }

            // IMPORTANTE: Dopo la prenotazione, torna all'Index per mostrare i dati aggiornati
            return RedirectToAction("Index");
        }
    }
}