
namespace LeyDeHont.Domain
{
    /*
     * 
     * Clase que guarda los datos previos para crear los partidos
     */
    class PreviousData
    {
        public int POPULATION { get; set; }
        public int ABSTENTIONS_VOTES { get; set; }
        public double NULL_VOTES { get; set; }       
        public double valid_votes {get; set;

        }
        // Constructor vacío
        public PreviousData()
        {
        // No se requiere inicialización aquí
        }
        // Constructor con todos los atributos
        public PreviousData(int population, int abstentionsVotes, int nullVotes, double valid_votes)
        {
            POPULATION = population;
            ABSTENTIONS_VOTES = abstentionsVotes;
            NULL_VOTES = nullVotes;
            this.valid_votes = valid_votes;
        }
        //Calculamos los votos nulos
        public static double CalculateNullVotes(int POPULATION, int ABSTENTIONS_VOTES)
        {
            double nullvotes = (POPULATION - ABSTENTIONS_VOTES) / 20;
            return nullvotes;
        }
        //Calculamos los votos validos
        public static int calculatevalidvotes(int POPULATION, int ABSTENTIONS_VOTES,int NULLVOTES)
        {
            int validvotes = (POPULATION - ABSTENTIONS_VOTES) - NULLVOTES;
            return validvotes;
        }


    }


}

