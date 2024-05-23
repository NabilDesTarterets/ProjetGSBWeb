using System.Data;
using ProjetGSBWeb.Models.Persistance;
using ProjetGSBWeb.Models.MesExceptions;
using ProjetGSBWeb.Models.Metier;
using System;


namespace ProjetGSBWeb.Models.Dao
{
    public class ServiceActiviteCompl
    {

        public static DataTable GetLieuxActivites()
        {
            DataTable dt;
            Serreurs er = new Serreurs("Erreur sur lecture des Activités", "ServiceActiviteCompl.GetLieuxActivites()");

            try
            {
                String mysql = "Select lieu_activite From activite_compl";
                dt = DBInterface.Lecture(mysql, er);
                return dt; // Retourne la DataTable contenant les noms des praticiens
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static ActiviteCompl GetuneActivite(int id)
        {
            DataTable dt;
            ActiviteCompl uneActivite = null;
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens", "ServicePraticien.getunPraticien()");

            try
            {
                string mysql = @"SELECT p.id_activite_compl, p.lieu_activite, p.theme_activite
                FROM activite_compl p
                WHERE p.lieu_activite = @id ";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", id);



                dt = DBInterface.Lecture(mysql, er, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    uneActivite = new ActiviteCompl();
                    DataRow dataRow = dt.Rows[0];
                    uneActivite.Id_activite_compl = int.Parse(dataRow["id_activite_compl"].ToString());
                    uneActivite.Lieu_activite = dataRow["lieu_activite"].ToString();
                    uneActivite.Theme_activite = dataRow["theme_activite"].ToString();



                    return uneActivite;
                }
                else
                {
                    return null;
                }
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static List<ActiviteCompl> GetActivitesParLieu(string lieu)
        {
            DataTable dt;
            List<ActiviteCompl> activites = new List<ActiviteCompl>();
            Serreurs er = new Serreurs("Erreur sur lecture des activités", "ServiceActiviteCompl.GetActivitesParLieu()");

            try
            {
                string mysql = @"SELECT id_activite_compl, lieu_Activite
                         FROM activite_compl
                         WHERE lieu_Activite = @lieu";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@lieu", lieu);

                dt = DBInterface.Lecture(mysql, er, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        ActiviteCompl activite = new ActiviteCompl
                        {
                            Id_activite_compl = int.Parse(dataRow["id_activite_compl"].ToString()),
                            Lieu_activite = dataRow["lieu_activite"].ToString(),
                        };
                        activites.Add(activite);
                    }
                }

                return activites;
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

    }
}