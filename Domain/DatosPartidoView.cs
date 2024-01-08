using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LeyDeHont.Domain
{
    internal class DatosPartidoVIEW : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<DatosPartido> parties;
        private const string cnstr = "server=localhost;uid=Jose;pwd=josepablo;database=partidospoliticos";

        public ObservableCollection<DatosPartido> Parties
        {
            get { return parties; }
            set
            {
                parties = value;
                OnPropertyChange("Parties");
            }
        }

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

        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NewUser()
        {
            PartiesDataComponent.InsertParty(new DatosPartido
            {
                Acronimo = Acronimo,
                Nombre = Nombre,
                Presidente = Presidente,
                Votes = Votes,
                Seats = Seats
            });
        }

        public void LoadUsers()
        {
            Parties = new ObservableCollection<DatosPartido>(PartiesDataComponent.ReadParties());
        }

        public void DeleteParty(string partyName)
        {
            // Implementa la lógica para borrar un partido utilizando el componente de datos.
            PartiesDataComponent.DeleteParty(partyName);
        }

        public void UpdateParty()
        {
            // Implementa la lógica para actualizar un partido utilizando el componente de datos.
            PartiesDataComponent.UpdateOrInsertParty(new DatosPartido
            {
                Acronimo = Acronimo,
                Nombre = Nombre,
                Presidente = Presidente,
                Votes = Votes,
                Seats = Seats
            });


        }
    }
}

