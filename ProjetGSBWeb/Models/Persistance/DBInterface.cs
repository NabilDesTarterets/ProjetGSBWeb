using MySql.Data.MySqlClient;
using System.Data;
using ProjetGSBWeb.Models.MesExceptions;

namespace ProjetGSBWeb.Models.Persistance
{
    public class DBInterface
    {
        /// <summary>
        /// Exécution de la requête demandée en paramètre,req,
        /// et retour du resultat : un DataTable
        /// Si tout se passe bien la connexion est prête à être fermée
        /// par le client qui utilisera cette connexion
        /// </summary>
        /// <param name="req">RequêteMySql à exécuter</param>
        /// <returns></returns>
        public static DataTable Lecture(string req, Serreurs er, Dictionary<string, object> parameters = null)
        {
            MySqlConnection cnx = null;
            try
            {
                cnx = Connexion.getInstance().getConnexion();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = req;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;

                // Construire le DataSet
                DataSet ds = new DataSet();
                da.Fill(ds, "resultat");
                cnx.Close();

                // Retourner la table
                return ds.Tables["resultat"];
            }
            catch (MonException me)
            {
                throw me;
            }
            catch (Exception e)
            {
                throw new MonException(er.MessageUtilisateur(), er.MessageApplication(), e.Message);
            }
            finally
            {
                // S'il y a eu un problème, la connexion
                // peut être encore ouverte, dans ce cas
                // il faut la fermer.
                if (cnx != null)
                    cnx.Close();
            }
        }

        /// <summary>
        /// /// Insertion d'une requête avec OleDb
        /// </summary>
        /// <param name="requete"></param>
        public static void Execute_Transaction(String requete)
        {
            MySqlConnection cnx = null;
            try
            {
                // On ouvre une transaction
                cnx = Connexion.getInstance().getConnexion();
                MySqlTransaction OleTrans =
                cnx.BeginTransaction();
                MySqlCommand OleCmd = new MySqlCommand();
                OleCmd = cnx.CreateCommand();
                OleCmd.Transaction = OleTrans;
                OleCmd.CommandText = requete;
                OleCmd.ExecuteNonQuery();
                OleTrans.Commit();
            }
            catch (MySqlException uneException)
            {
                throw new MonException(uneException.Message,
               "Insertion", "SQL");
            }
        }

        public static bool ExecuteWithParameters(string requete, Dictionary<string, object> parameters)
        {
            MySqlConnection cnx = null;
            try
            {
                // Ouvrir une connexion à la base de données
                cnx = Connexion.getInstance().getConnexion();

                // Création de la commande SQL avec la requête fournie
                MySqlCommand command = cnx.CreateCommand();
                command.CommandText = requete;

                // Ajout des paramètres à la commande
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                // Exécution de la commande
                int rowsAffected = command.ExecuteNonQuery();

                // Retourne true si au moins une ligne a été affectée
                return rowsAffected > 0;
            }
            catch (MySqlException uneException)
            {
                throw new MonException(uneException.Message, "Insertion", "SQL");
            }
            finally
            {
                // Fermeture de la connexion
                if (cnx != null)
                    cnx.Close();
            }
        }

        public static void Insertion_Donnees_Update(String requete, string specialiste, string motifActivites, int idPraticien, int oldIdActiviteCompl)
        {
            MySqlConnection cnx = null;
            try
            {
                // Ouvrir une connexion à la base de données
                cnx = Connexion.getInstance().getConnexion();

                // Création de la commande SQL avec la requête fournie
                MySqlCommand command = cnx.CreateCommand();
                command.CommandText = requete;

                // Ajout des paramètres à la commande
                command.Parameters.AddWithValue("@specialiste", specialiste);
                command.Parameters.AddWithValue("@motifActivites", motifActivites);
                command.Parameters.AddWithValue("@idPraticien", idPraticien);
                command.Parameters.AddWithValue("@oldIdActiviteCompl", oldIdActiviteCompl);

                // Exécution de la commande
                command.ExecuteNonQuery();
            }
            catch (MySqlException uneException)
            {
                throw new MonException(uneException.Message, "Insertion", "SQL");
            }
            finally
            {
                // Fermeture de la connexion
                if (cnx != null)
                    cnx.Close();
            }
        }

        public static void Insertion_Donnees_Add(string requete, object specialiste, object motifActivite, object idPraticien)
        {
            MySqlConnection cnx = null;
            try
            {
                // Ouvrir une connexion à la base de données
                cnx = Connexion.getInstance().getConnexion();

                // Création de la commande SQL avec la requête fournie
                MySqlCommand command = cnx.CreateCommand();
                command.CommandText = requete;

                // Ajout des paramètres à la commande
                command.Parameters.AddWithValue("@specialiste", specialiste);
                command.Parameters.AddWithValue("@motifActivite", motifActivite);
                command.Parameters.AddWithValue("@idPraticien", idPraticien);

                // Exécution de la commande
                command.ExecuteNonQuery();
            }
            catch (MySqlException uneException)
            {
                throw new MonException(uneException.Message, "Insertion", "SQL");
            }
            finally
            {
                // Fermeture de la connexion
                if (cnx != null)
                    cnx.Close();
            }
        }

    }
}