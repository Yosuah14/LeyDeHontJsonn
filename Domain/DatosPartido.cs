using LeyDeHont.Persistence.Manages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LeyDeHont.Domain
{
    /*
     * Clase que almacena los datos de un partido politico
     */
    class DatosPartido : INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string acronimo;

        public string Acronimo
        {
            get { return acronimo; }
            set
            {
                acronimo = value;
                OnPropertyChange("Acronimo");
            }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                OnPropertyChange("Nombre");
            }
        }

        private string presidente;
        public string Presidente
        {
            get { return presidente; }
            set
            {
                presidente = value;
                OnPropertyChange("Presidente");
            }
        }

        private int votes;
        public int Votes
        {
            get { return votes; }
            set
            {
                votes = value;
                OnPropertyChange("Votes");
            }
        }

        private int seats;
        public int Seats
        {
            get { return seats; }
            set
            {
                seats = value;
                OnPropertyChange("Seats");
            }
        }

        public PartiesManage Pm { get; set; }

        private int votesaux;

        public int VotesAux
        {
            get { return votesaux; }
            set
            {
                votesaux = value;
                OnPropertyChange(nameof(VotesAux));
            }
        }

        // Constructores
        public DatosPartido()
        {
            Pm = new PartiesManage();
        }

        public DatosPartido(string acronimo, string nombre, string presidente)
        {
            Acronimo = acronimo;
            Nombre = nombre;
            Presidente = presidente;
            Pm = new PartiesManage();
        }

        public DatosPartido(string acronimo, string nombre, string presidente, int votes, int votesaux)
        {
            Acronimo = acronimo;
            Nombre = nombre;
            Presidente = presidente;
            Pm = new PartiesManage();
            Votes = votes;
            VotesAux = votesaux;
        }

        public DatosPartido(string acronimo, string nombre, string presidente, int votes, int votesaux, int seats)
        {
            Acronimo = acronimo;
            Nombre = nombre;
            Presidente = presidente;
            Pm = new PartiesManage();
            Votes = votes;
            Seats = seats;
            VotesAux = votesaux;
        }
        public List<DatosPartido> getListParties()
        {
            return Pm.listParties;
        }
        //añadir partidos
        public void addParties(string name, string nPartido, string presidente)
        {
            Pm.addPartdio(name, nPartido, presidente);
        }
        //Borrar partidos de la lista
        public void removeParties(DatosPartido p)
        {
            Pm.listParties.Remove(p);
        }
        //Calcular el porcentaje 
        public static int calculateVotesPartie(double percent, PreviousData p)
        {
            double votesDouble = p.valid_votes * percent; // Cálculo como número de punto flotante
            int votes = (int)Math.Round(votesDouble); // Redondeo y conversión a int
            return votes;
        }
        //Calcular los escaños
        private static int seatsCount(DatosPartido p)
        {
            //Sumamos un escaño
            p.Seats++;
            int votes = p.Votes / (p.Seats + 1);
            return votes;
        }
        //Metodo para añadir los escaños a cada partido
        public static List<DatosPartido> CalculateSeats(List<DatosPartido> parties)
        {
            //Pongo una constante con 37 escaños
            const int SEATS = 37;
            for (int seat = 0; seat < SEATS; seat++)
            {
                // Encontrar el partido con más votos en la lista calculada
                int indexOfPartyWithMostVotes = parties.FindIndex(p => p.VotesAux == parties.Max(party => party.VotesAux));
                // Realizar el cálculo para asignar asientos al partido
                int seatsCount = DatosPartido.seatsCount(parties[indexOfPartyWithMostVotes]);
                parties[indexOfPartyWithMostVotes].VotesAux = seatsCount;
            }
            return parties;
        }
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


