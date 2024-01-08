using System;
using System.Collections.Generic;

namespace LeyDeHont.Domain
{
    internal class PartidosFactory
    {
        /*
         * Factoria que inicializa los partidos
         */
        public static List<DatosPartido>  inicialiteParties(PreviousData p, List<DatosPartido> parties)
        {
            double validVotes = p.valid_votes * 0.03;
            int minvotes = (int)Math.Round(validVotes);
            //Mete los votos con sus porcentajes
            parties[0].Votes = DatosPartido.calculateVotesPartie(0.3524, p);
            parties[1].Votes = DatosPartido.calculateVotesPartie(0.2475, p);
            parties[2].Votes = DatosPartido.calculateVotesPartie(0.1575, p);
            parties[3].Votes = DatosPartido.calculateVotesPartie(0.1425, p);
            parties[4].Votes = DatosPartido.calculateVotesPartie(0.0375, p);
            parties[5].Votes = DatosPartido.calculateVotesPartie(0.0325, p);
            parties[6].Votes = DatosPartido.calculateVotesPartie(0.015, p);
            parties[7].Votes = DatosPartido.calculateVotesPartie(0.005, p);
            parties[8].Votes = DatosPartido.calculateVotesPartie(0.0025, p);
            parties[9].Votes = DatosPartido.calculateVotesPartie(0.0025, p);         
            List < DatosPartido > updateParties=new List<DatosPartido>(); 
            //Añado a la nueva lista los votos que superan el 3 por ciento de los votos totales
            foreach (var part in parties)
            {
                if (part.Votes >= minvotes) 
                {
                    part.VotesAux = part.Votes;
                    updateParties.Add(part);
                }
            }           
           return updateParties;
        }
    }
}
