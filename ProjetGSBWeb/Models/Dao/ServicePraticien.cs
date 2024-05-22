using System.Data;
using ProjetGSBWeb.Models.Persistance;
using ProjetGSBWeb.Models.MesExceptions;

namespace ProjetGSBWeb.Models.Dao
{
    public class ServicePraticien
    {
        public static DataTable GetTousLesPraticiens()
        {
            DataTable mesPraticiens;
            Serreurs er = new Serreurs("Erreur sur la lecture des Praticiens.", "Praticien.getPraticiens()");
            try
            {
                String mysql = "Select id_praticien, nom_praticien, ville_praticien, coef_notoriete From praticien";

                mesPraticiens = DBInterface.Lecture(mysql, er);

                return mesPraticiens;
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static Praticien GetunPraticien(int id)
        {
            DataTable dt;
            Praticien unPraticien = null;
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens", "ServicePraticien.getunPraticien()");

            try
            {
                string mysql = @"SELECT p.id_praticien, p.nom_praticien, p.ville_praticien, p.coef_notoriete
                FROM praticien p
                WHERE p.id_praticien = @id ";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", id);



                dt = DBInterface.Lecture(mysql, er, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    unPraticien = new Praticien();
                    DataRow dataRow = dt.Rows[0];
                    unPraticien.Id_praticien = int.Parse(dataRow["id_praticien"].ToString());
                    unPraticien.Nom_praticien = dataRow["nom_praticien"].ToString();
                    unPraticien.Ville_praticien = dataRow["ville_praticien"].ToString();
                    unPraticien.Coef_notoriete = float.Parse(dataRow["coef_notoriete"].ToString());


                    return unPraticien;
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






        public static DataTable GetNomPraticiens()
        {
            DataTable dt;
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens", "ServicePraticien.GetNomPraticiens()");

            try
            {
                String mysql = "Select id_praticien, nom_praticien, id_type_praticien From praticien";
                dt = DBInterface.Lecture(mysql, er);
                return dt; // Retourne la DataTable contenant les noms des praticiens
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }



        public static DataTable GetTypesPraticiens()
        {
            DataTable dt;
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens", "ServicePraticien.GetTypesPraticiens()");

            try
            {
                String mysql = "SELECT DISTINCT id_type_praticien FROM praticien;";
                dt = DBInterface.Lecture(mysql, er);
                return dt; // Retourne la DataTable contenant les types
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static List<Praticien> GetPraticiensByType(int idTypePraticien)
        {
            DataTable dt;
            List<Praticien> praticiens = new List<Praticien>();
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens", "ServicePraticien.GetPraticiensByType()");

            try
            {
                string mysql = @"SELECT p.id_praticien, p.nom_praticien, p.ville_praticien, p.coef_notoriete
                         FROM praticien p
                         WHERE p.id_type_praticien = @idTypePraticien";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idTypePraticien", idTypePraticien);

                dt = DBInterface.Lecture(mysql, er, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        Praticien unPraticien = new Praticien();
                        unPraticien.Id_praticien = int.Parse(dataRow["id_praticien"].ToString());
                        unPraticien.Nom_praticien = dataRow["nom_praticien"].ToString();
                        unPraticien.Ville_praticien = dataRow["ville_praticien"].ToString();
                        unPraticien.Coef_notoriete = float.Parse(dataRow["coef_notoriete"].ToString());

                        praticiens.Add(unPraticien);
                    }
                }

                return praticiens;
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

    }
}