using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using ProjetGSBWeb.Models.Dao;
using ProjetGSBWeb.Models.MesExceptions;
using ProjetGSBWeb.Models.Metier;


namespace ProjetGSBWeb.Controllers
{
    public class ActiviteComplController : Controller
    {

        public IActionResult RechercheActvite()
        {
            DataTable lieuxActivites = ServiceActiviteCompl.GetLieuxActivites();


            // Passer les DataTables à la vue via le ViewBag
            ViewBag.lieuxActivites = lieuxActivites;


            return View();
        }


        public IActionResult RechercherActiviteLieu()
        {
            ActiviteCompl uneActivite = null;
            try
            {
                // Récupère la valeur du champ de formulaire nom comme une chaîne de caractères
                string lieuxString = Request.Form["lieu"];

                // Convertit la chaîne de caractères en entier
                if (int.TryParse(lieuxString, out int lieu))
                {
                    // Appelle le service pour obtenir le praticien avec le nom spécifié
                    uneActivite = ServiceActiviteCompl.GetuneActivite(lieu);
                    return View(uneActivite);
                }
                else
                {
                    // La valeur du champ de formulaire n'est pas un entier valide
                    // Gérer l'erreur ici si nécessaire
                    return BadRequest("Le nom du praticien doit être un nombre entier.");
                }
            }
            catch (MonException e)
            {
                // Gérer l'erreur ici si nécessaire
                return NotFound();
            }


        }
        public IActionResult GetActiviteDetails(string lieu)
        {
            if (string.IsNullOrEmpty(lieu))
            {
                return BadRequest("Lieu de l'activité invalide.");
            }

            List<ActiviteCompl> activites = ServiceActiviteCompl.GetActivitesParLieu(lieu);

            if (activites == null || activites.Count == 0)
            {
                return NotFound("Aucune activité trouvée pour ce lieu.");
            }
            else
            {
                return PartialView("_ActiviteDetails", activites);
            }
        }


    }
}