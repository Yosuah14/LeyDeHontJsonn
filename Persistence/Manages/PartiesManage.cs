using LeyDeHont.Domain;
using System;
using System.Collections.Generic;

namespace LeyDeHont.Persistence.Manages
{
    /*
     * Clase que administra los partidos creados
     */
  internal  class PartiesManage
    {       
        public List<DatosPartido> listParties { get; set; }
        public PartiesManage()
        {
            listParties = new List<DatosPartido>();
        }
        public void addPartdio(string acronimo, String nPartidos, String presidente)
        {
            listParties.Add(new DatosPartido(acronimo, nPartidos, presidente));
        }

    }
}

