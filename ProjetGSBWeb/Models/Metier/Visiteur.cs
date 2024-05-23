namespace ProjetGSBWeb.Models.Metier
{
    public class Visiteur
    {

        private int id_visiteur;
        private string nom_visiteur;
        private string login_visiteur;
        private string pwd_visiteur;

        public int Id_visiteur { get => id_visiteur; set => id_visiteur = value; }
        public string Nom_visiteur { get => nom_visiteur; set => nom_visiteur = value; }
        public string Login_visiteur { get => login_visiteur; set => login_visiteur = value; }
        public string Pwd_visiteur { get => pwd_visiteur; set => pwd_visiteur = value; }


        public Visiteur(String login, String pwd)
        {
            this.login_visiteur = login;
            this.pwd_visiteur = pwd;
        }

        public Visiteur()
        { }
    }
}