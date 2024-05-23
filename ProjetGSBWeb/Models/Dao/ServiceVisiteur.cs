using Org.BouncyCastle.Crypto.Digests;
using System.Data;
using ProjetGSBWeb.Models.Persistance;
using ProjetGSBWeb.Models.MesExceptions;
using ProjetGSBWeb.Models.Metier;

namespace ProjetGSBWeb.Models.Dao
{
    public class ServiceVisiteur
    {

        public Visiteur getVisiteur(String login)
        {
            DataTable dt;
            Visiteur unVisi = null;
            String mysql = "SELECT login_visiteur, pwd_visiteur FROM visiteur" + " where login_visiteur=" + "'" + login + "'";
            Serreurs er = new Serreurs("Erreur sur recherche d'un utilisateur.", "Service.getVisiteur");
            try
            {
                dt = DBInterface.Lecture(mysql, er);
                if (dt.IsInitialized && dt.Rows.Count > 0)
                {
                    unVisi = new Visiteur();
                    // il faut redecouper la liste pour retrouver les lignes
                    DataRow dataRow = dt.Rows[0];
                    unVisi.Login_visiteur = dataRow[0].ToString();
                    unVisi.Pwd_visiteur = dataRow[1].ToString();
                }
                return unVisi;
            }
            catch (MonException e)
            {
                throw e;
            }
            catch (Exception exc)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), exc.Message);

            }
        }
    }
}