using Microsoft.AspNetCore.Mvc;
using System.Data;
using ProjetGSBWeb.Models.Dao;
using ProjetGSBWeb.Models.MesExceptions;
using ProjetGSBWeb.Models.Metier;

namespace ProjetGSBWeb.Controllers
{
    public class PraticienController : Controller
    {
        public IActionResult Index()
        {
            System.Data.DataTable mesPraticiens = null;

            try
            {
                mesPraticiens = ServicePraticien.GetTousLesPraticiens();
            }
            catch (MonException e)
            {
                ModelState.AddModelError("Erreur", "Erreur lors de la recuperation des praticiens: " + e.Message);
            }
            return View(mesPraticiens);
        }


        public IActionResult RechercheNom()
        {
            DataTable nomPraticiens = ServicePraticien.GetNomPraticiens();
            DataTable typesPraticiens = ServicePraticien.GetTypesPraticiens();
            ViewBag.NomPraticiens = nomPraticiens;
            ViewBag.TypesPraticiens = typesPraticiens;

            return View();
        }

        public IActionResult RechercheType()
        {
            System.Data.DataTable TypesPraticiens = null;

            try
            {
                TypesPraticiens = ServicePraticien.GetTypesPraticiens();
            }
            catch (MonException e)
            {
                ModelState.AddModelError("Erreur", "Erreur lors de la recuperation des types: " + e.Message);
            }
            return View(TypesPraticiens);
        }

        public IActionResult RechercherPraticienNom()
        {
            Praticien unPraticien = null;
            try
            {
                string nomString = Request.Form["nom"];
                if (int.TryParse(nomString, out int nom))
                {
                    unPraticien = ServicePraticien.GetunPraticien(nom);
                    return View(unPraticien);
                }
                else
                {
                    return BadRequest("Le nom du praticien doit être un nombre entier.");
                }
            }
            catch (MonException e)
            {
                return NotFound();
            }
        }

        public IActionResult RechercherPraticienType()
        {
            Praticien unPraticien = null;
            try
            {
                string nomString = Request.Form["nom"]
                if (int.TryParse(nomString, out int nom))
                    unPraticien = ServicePraticien.GetunPraticien(nom);
                    return View(unPraticien);
                }
                else
                {
                    return BadRequest("Le nom du praticien doit être un nombre entier.");
                }
            }
            catch (MonException e)
            {
                return NotFound();
            }
        }

        public IActionResult GetPraticienDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID de praticien invalide.");
            }

            Praticien unPraticien = ServicePraticien.GetunPraticien(id);

            if (unPraticien == null)
            {
                return NotFound("Praticien non trouvé.");
            }
            else
            {
                return PartialView("_PraticienDetails", unPraticien);
            }
        }

        public IActionResult GetPraticiensByType(int idTypePraticien)
        {
            if (idTypePraticien <= 0)
            {
                return BadRequest("ID de type de praticien invalide.");
            }

            List<Praticien> praticiens = ServicePraticien.GetPraticiensByType(idTypePraticien);

            if (praticiens == null || praticiens.Count == 0)
            {
                return NotFound("Aucun praticien trouvé pour ce type.");
            }
            else
            {
                return PartialView("_PraticiensList", praticiens);
            }
        }
    }
}