using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetGSBWeb.Models.Dao;
using ProjetGSBWeb.Models.MesExceptions;
using ProjetGSBWeb.Models.Metier;


namespace ProjetGSBWeb.Controllers
{
    public class InvitationController : Controller
    {

        public IActionResult GetInvitationsDetails(int id)
        {
            // Vérifier si l'ID est null ou invalide
            if (id == null || id <= 0)
            {
                // Si l'ID n'est pas valide, retourner une vue BadRequest
                return BadRequest("ID de praticien invalide.");
            }

            try
            {
                // Récupérer toutes les invitations associées à l'ID du praticien
                List<Invitation> invitations = ServiceInvitation.GetInvitationsByID(id);

                // Vérifier si des invitations ont été trouvées
                if (invitations != null && invitations.Count > 0)
                {
                    // Si des invitations sont trouvées, retourner la vue partielle avec les détails des invitations
                    return PartialView("_InvitationsDetails", invitations);

                }
                else
                {
                    // Si aucune invitation n'est trouvée pour l'ID spécifié, retourner une vue NotFound
                    return PartialView("_InvitationsDetails", invitations);
                }
            }
            catch (MonException e)
            {
                // Gérer les exceptions
                return StatusCode(500, "Une erreur s'est produite lors de la récupération des invitations.");
            }
        }


        [HttpPost]
        public IActionResult SupprimerInvitation(int idPraticien, int idActivite)
        {

            // Appelez la méthode deleteInvitation de votre service
            bool suppressionReussie = ServiceInvitation.deleteInvitation(idPraticien, idActivite);

            // Vérifiez si la suppression a réussi
            if (suppressionReussie)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Gérez le cas où la suppression a échoué, par exemple en renvoyant une vue avec un message d'erreur
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult ModifierInvitation(int idPraticien, int idActivite)
        {
            if (idPraticien <= 0 || idActivite <= 0)
            {
                // Si l'ID du praticien ou de l'activité n'est pas valide, retourner une vue BadRequest
                return BadRequest("ID de praticien ou d'activité invalide.");
            }

            // Récupérer l'invitation correspondant aux ID spécifiés
            Invitation uneInvitation = ServiceInvitation.getUneInvitation(idPraticien, idActivite);

            // Vérifier si l'invitation a été trouvée
            if (uneInvitation == null)
            {
                // Si aucune invitation n'est trouvée, retourner une vue NotFound
                return NotFound("Invitation non trouvée.");
            }

            // Récupérer les thèmes des activités complémentaires
            List<string> motifsActivites = ServiceInvitation.getAllActivitesComplThemes();

            // Passer les données à la vue via ViewBag
            ViewBag.Invitation = uneInvitation;
            ViewBag.MotifActivites = motifsActivites;
            HttpContext.Session.SetString("loginVisiteur", "1");
            // Retourner la vue
            return PartialView("FormEditInvitation");

        }

        [HttpPost]
        public ActionResult confirmUpdate(int idPraticien, int oldIdActiviteCompl, string motifActivites, string specialiste)
        {
            try
            {
                if (motifActivites == null || specialiste == null)
                {

                    return StatusCode(500, "Une erreur s'est produite lors de l'update.");
                }

                // Appelle le service pour mettre à jour l'invitation
                ServiceInvitation.UpdateInvitation(idPraticien, oldIdActiviteCompl, motifActivites, specialiste);

                // Redirige vers la page d'accueil après la mise à jour réussie
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur s'est produite lors de l'update.");


            }
        }

        public ActionResult Ajout()
        {
            var praticiens = ServiceInvitation.GetPraticiens();
            ViewBag.Praticiens = praticiens;

            // Remplacer cette méthode par celle que tu utilises pour obtenir les motifs d'activités complémentaires
            var motifActivites = ServiceInvitation.getAllActivitesComplThemes(); // Cette méthode doit retourner une liste de motifs d'activités

            // Passer la liste des motifs d'activités à la vue
            ViewBag.MotifActivites = motifActivites;

            return View();
        }



        [HttpPost]
        public ActionResult AddInvitation(int idPraticien, string motifActivite, string specialiste)
        {
            try
            {
                // Appel du service pour ajouter la nouvelle invitation
                ServiceInvitation.AddNewInvitation(idPraticien, motifActivite, specialiste);

                // Redirection après succès
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Une erreur s'est produite lors de l'ajout.");
            }
        }

    }

}