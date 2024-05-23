namespace ProjetGSBWeb.Models.Metier
{
    public class ActiviteCompl
    {
        private int id_activite_compl;
        private DateTime date_activite;
        private string lieu_activite;
        private string theme_activite;
        private string motif_activite;

        public int Id_activite_compl { get => id_activite_compl; set => id_activite_compl = value; }
        public DateTime Date_activite { get => date_activite; set => date_activite = value; }
        public string Lieu_activite { get => lieu_activite; set => lieu_activite = value; }
        public string Theme_activite { get => theme_activite; set => theme_activite = value; }
        public string Motif_activite { get => motif_activite; set => motif_activite = value; }
    }
}