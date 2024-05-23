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
            // V�rifier si l'ID est null ou invalide
            if (id == null || id <= 0)
            {
                // Si l'ID n'est pas valide, retourner une vue BadRequest
                return BadRequest("ID de praticien invalide.");
            }

            try
            {
                // R�cup�rer toutes les invitations associ�es � l'ID du praticien
                List<Invitation> invitations = ServiceInvitation.GetInvitationsByID(id);

                // V�rifier si des invitations ont �t� trouv�es
                if (invitations != null && invitations.Count > 0)
                {
                    // Si des invitations sont trouv�es, retourner la vue partielle avec les d�tails des invitations
                    return PartialView("_InvitationsDetails", invitations);

                }
                else
                {
                    // Si aucune invitation n'est trouv�e pour l'ID sp�cifi�, retourner une vue NotFound
                    return PartialView("_InvitationsDetails", invitations);
                }
            }
            catch (MonException e)
            {
                // G�rer les exceptions
                return StatusCode(500, "Une erreur s'est produite lors de la r�cup�ration des invitations.");
            }
        }


        [HttpPost]
        public IActionResult SupprimerInvitation(int idPraticien, int idActivite)
        {

            // Appelez la m�thode deleteInvitation de votre service
            bool suppressionReussie = ServiceInvitation.deleteInvitation(idPraticien, idActivite);

            // V�rifiez si la suppression a r�ussi
            if (suppressionReussie)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // G�rez le cas o� la suppression a �chou�, par exemple en renvoyant une vue avec un message d'erreur
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult ModifierInvitation(int idPraticien, int idActivite)
        {
            if (idPraticien <= 0 || idActivite <= 0)
            {
                // Si l'ID du praticien ou de l'activit� n'est pas valide, retourner une vue BadRequest
                return BadRequest("ID de praticien ou d'activit� invalide.");
            }

            // R�cup�rer l'invitation correspondant aux ID sp�cifi�s
            Invitation uneInvitation = ServiceInvitation.getUneInvitation(idPraticien, idActivite);

            // V�rifier si l'invitation a �t� trouv�e
            if (uneInvitation == null)
            {
                // Si aucune invitation n'est trouv�e, retourner une vue NotFound
                return NotFound("Invitation non trouv�e.");
            }

            // R�cup�rer les th�mes des activit�s compl�mentaires
            List<string> motifsActivites = ServiceInvitation.getAllActivitesComplThemes();

            // Passer les donn�es � la vue via ViewBag
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

                // Appelle le service pour mettre � jour l'invitation
                ServiceInvitation.UpdateInvitation(idPraticien, oldIdActiviteCompl, motifActivites, specialiste);

                // Redirige vers la page d'accueil apr�s la mise � jour r�ussie
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

            // Remplacer cette m�thode par celle que tu utilises pour obtenir les motifs d'activit�s compl�mentaires
            var motifActivites = ServiceInvitation.getAllActivitesComplThemes(); // Cette m�thode doit retourner une liste de motifs d'activit�s

            // Passer la liste des motifs d'activit�s � la vue
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

                // Redirection apr�s succ�s
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Une erreur s'est produite lors de l'ajout.");
            }
        }

    }

}