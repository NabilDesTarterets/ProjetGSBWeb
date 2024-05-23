using Microsoft.AspNetCore.Mvc;
using ProjetGSBWeb.Models.Dao;
using ProjetGSBWeb.Models.MesExceptions;
using ProjetGSBWeb.Models.Metier;
using BCrypt;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;


namespace ProjetGSBWeb.Controllers
{
    public class ConnexionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Deconnexion()
        {
            try
            {
                if (HttpContext.Session.GetString("loginVisiteur") == "1")
                {
                    HttpContext.Session.SetString("loginVisiteur", "0");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Erreur", "Erreur lors du contrôle : " + ex.Message);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Connexion");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Controle()
        {
            try
            {
                string login = Request.Form["login"];
                string mdp = Request.Form["pwd"];

                try
                {
                    ServiceVisiteur unService = new ServiceVisiteur();
                    Visiteur unVisiteur = unService.getVisiteur(login);
                    if (unVisiteur != null)
                    {
                        try
                        {

                            if (BCrypt.Net.BCrypt.Verify(mdp, unVisiteur.Pwd_visiteur))

                            {
                                HttpContext.Session.SetString("loginVisiteur", "1");

                            }
                            else
                            {
                                HttpContext.Session.SetString("loginVisiteur", "0");

                                ModelState.AddModelError("Erreur", "Erreur lors du contrôle du mot de passe :" + login);
                                return RedirectToAction("Index", "Connexion");
                            }
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("Erreur", "Erreur lors du contrôle : " + e.Message);
                            return RedirectToAction("Index", "Connexion");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Erreur", "Erreur login erroné " + login); return RedirectToAction("Index", "Connexion");
                    }
                }
                catch (MonException e)
                {
                    ModelState.AddModelError("Erreur", "Erreur lors du contrôle : " + e.Message);
                    return RedirectToAction("Index", "Connexion");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (MonException e)
            {
                ModelState.AddModelError("Erreur", "Erreur lors du contrôle : " + e.Message);
                return RedirectToAction("Index", "Connexion");

            }
        }

    }
}