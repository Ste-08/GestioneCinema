namespace CineManagerWeb.Models
{
    public class Sala
    {
        public int Id { get; set; }
       
        public string Film { get; set; }
        public int PostiLiberi { get; set; }

        public int TotalePosti { get; set; }

        public int PostiOccupati
        {
            get { return TotalePosti - PostiLiberi; }
        }
        public int NumeroSala { get; set; }


        public void OccupaPosto()
        {
            if (PostiLiberi > 0) PostiLiberi--;
        }
        public void LiberaPosti()
        {
            PostiLiberi++;
        }

        public void DiminuisciMax()
        {
            if(TotalePosti > PostiOccupati) TotalePosti --;
            PostiLiberi--;
        }
        public void AumentaMax()
        {
            TotalePosti++;
            PostiLiberi++;
        }
    }
}