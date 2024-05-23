using System.Data;
using ProjetGSBWeb.Models.Persistance;
using ProjetGSBWeb.Models.MesExceptions;
using ProjetGSBWeb.Models.Metier;
using System;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace ProjetGSBWeb.Models.Dao
{
    public class ServiceInvitation
    {
        public static List<Invitation> GetInvitationsByID(int id)
        {
            DataTable dt;
            List<Invitation> invitations = new List<Invitation>();
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens", "ServiceInvitation.GetInvitations()");

            try
            {
                string mysql = @"SELECT inviter.*,inviter.id_praticien, inviter.id_activite_compl, activite_Compl.theme_activite, Activite_Compl.lieu_activite, Activite_Compl.motif_activite
                         FROM inviter
                         JOIN activite_Compl ON inviter.id_activite_compl = activite_Compl.id_activite_compl
                         WHERE inviter.id_praticien = @id ;
                     ";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", id);

                dt = DBInterface.Lecture(mysql, er, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        Invitation invitation = new Invitation();
                        invitation.Id_activite_compl = int.Parse(dataRow["id_activite_compl"].ToString());
                        invitation.Id_praticien = int.Parse(dataRow["id_praticien"].ToString());
                        invitation.Theme_activite = dataRow["theme_activite"].ToString();
                        invitation.Lieu_activite = dataRow["lieu_activite"].ToString();
                        invitation.Motif_activite = dataRow["motif_activite"].ToString();

                        // Ajoutez l'invitation à la liste
                        invitations.Add(invitation);
                    }
                }

                return invitations;
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static bool deleteInvitation(int idPraticien, int idActiviteCompl)
        {
            Serreurs er = new Serreurs("Erreur sur suppression de l'invitation", "ServiceInvitation.deleteInvitation()");

            try
            {
                string mysql = @"DELETE FROM inviter 
                         WHERE id_praticien = @idPraticien
                         AND id_activite_compl = @idActivite";

                // Créer un dictionnaire de paramètres
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@idPraticien"] = idPraticien;
                parameters["@idActivite"] = idActiviteCompl;

                // Exécuter la commande de suppression en utilisant la nouvelle méthode
                return DBInterface.ExecuteWithParameters(mysql, parameters);
            }
            catch (MonException e)
            {
                // Gérer les exceptions
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static Invitation getUneInvitation(int idPraticien, int idActiviteCompl)
        {
            DataTable dt;
            Invitation uneInvitation = null;
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens", "ServiceInvitation.getUneInvitation()");

            try
            {
                string mysql = @"SELECT inviter.*, inviter.specialiste, inviter.id_praticien, inviter.id_activite_compl, activite_compl.theme_activite, activite_compl.lieu_activite, activite_compl.date_activite, activite_compl.motif_activite
                                FROM inviter 
                                JOIN activite_compl ON inviter.id_activite_compl = activite_compl.id_activite_compl
                                WHERE inviter.id_praticien = @idPraticien
                                AND inviter.id_activite_compl = @idActiviteCompl";

                // Ajoutez les paramètres dans le dictionnaire
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idPraticien", idPraticien);
                parameters.Add("@idActiviteCompl", idActiviteCompl);

                dt = DBInterface.Lecture(mysql, er, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    uneInvitation = new Invitation();
                    DataRow dataRow = dt.Rows[0];
                    uneInvitation.Id_praticien = int.Parse(dataRow["id_praticien"].ToString());
                    uneInvitation.Id_activite_compl = int.Parse(dataRow["id_activite_compl"].ToString());
                    uneInvitation.Specialiste = dataRow["specialiste"].ToString();
                    uneInvitation.Theme_activite = dataRow["theme_activite"].ToString();
                    uneInvitation.Lieu_activite = dataRow["lieu_activite"].ToString();
                    uneInvitation.Motif_activite = dataRow["motif_activite"].ToString();

                    // Convertir la colonne date_activite en DateTime
                    if (DateTime.TryParse(dataRow["date_activite"].ToString(), out DateTime dateActivite))
                    {
                        uneInvitation.Date_activite = dateActivite;
                    }
                    else
                    {
                        // Gérer l'échec de la conversion
                        // Vous pouvez attribuer une valeur par défaut ou générer une exception, selon votre logique de gestion des erreurs
                    }

                    return uneInvitation;
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

        public static List<string> getAllActivitesComplThemes()
        {
            List<string> motifsActivites = new List<string>();
            Serreurs er = new Serreurs("Erreur sur la lecture des Praticiens.", "Invitation.getAllActivitesComplThemes()");
            try
            {
                String mysql = "SELECT motif_activite FROM activite_compl";

                DataTable dataTable = DBInterface.Lecture(mysql, er);

                // Parcourir les résultats de la requête et ajouter les thèmes à la liste
                foreach (DataRow row in dataTable.Rows)
                {
                    string theme = row["motif_activite"].ToString();
                    motifsActivites.Add(theme);
                }

                return motifsActivites;
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }




        public static List<int> getAllSpecialistes()
        {
            List<int> specialiste = new List<int>();
            Serreurs er = new Serreurs("Erreur sur la lecture des Praticiens.", "Invitation.getAllActivitesCompl()");
            try
            {
                String mysql = "Select specialiste From inviter";

                DataTable dataTable = DBInterface.Lecture(mysql, er);

                // Parcourir les résultats de la requête et ajouter les identifiants à la liste
                foreach (DataRow row in dataTable.Rows)
                {
                    string specialistes = row["specialiste"].ToString();
                }

                return specialiste;
            }
            catch (MonException e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }

        public static void UpdateInvitation(int idPraticien, int oldIdActiviteCompl, string motifActivites, string specialiste)
        {
            Serreurs er = new Serreurs("Erreur sur lecture des Praticiens", "ServiceInvitation.UpdateInvitation()");

            string mysql = @"UPDATE inviter
                       SET specialiste = @specialiste, 
                           id_activite_compl = (SELECT id_activite_compl FROM activite_compl WHERE motif_activite = @motifActivites)
                       WHERE id_praticien = @idPraticien 
                       AND id_activite_compl = @oldIdActiviteCompl;";

            try
            {
                DBInterface.Insertion_Donnees_Update(mysql, specialiste, motifActivites, idPraticien, oldIdActiviteCompl);
            }
            catch (MonException e)
            {
                // Si une exception est levée, repropage l'exception en la renvoyant sous forme de MonException
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }

        }


        public static void AddNewInvitation(int idPraticien, string motifActivite, string specialiste)
        {
            string mysql = @"INSERT INTO inviter (id_praticien, id_activite_compl, specialiste)
                     VALUES (@idPraticien, 
                             (SELECT id_activite_compl FROM activite_compl WHERE motif_activite = @motifActivite), 
                             @specialiste)";

            try
            {
                DBInterface.Insertion_Donnees_Add(mysql, specialiste, motifActivite, idPraticien);
            }
            catch (MonException e)
            {
                // Si une exception est levée, repropage l'exception en la renvoyant sous forme de MonException
                Serreurs er = new Serreurs("Erreur sur l'ajout de l'invitation", "ServiceInvitation.AddNewInvitation()");
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
        }


        public static List<Praticien> GetPraticiens()
        {
            List<Praticien> praticiens = new List<Praticien>();
            Serreurs er = new Serreurs("Erreur sur la lecture des Praticiens.", "ServiceInvitation.GetPraticiens()");
            try
            {
                string mysql = "SELECT id_praticien, nom_praticien FROM praticien";

                DataTable dt = DBInterface.Lecture(mysql, er);

                foreach (DataRow row in dt.Rows)
                {
                    Praticien praticien = new Praticien
                    {
                        Id_praticien = Convert.ToInt32(row["id_praticien"]),
                        Nom_praticien = row["nom_praticien"].ToString()
                    };

                    praticiens.Add(praticien);
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