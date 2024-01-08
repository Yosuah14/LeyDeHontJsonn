using LeyDeHont.Domain;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LeyDeHont
{
    /*
     * Clase donde se ejecuta la aplicación
     */
    public partial class MainWindow : Window
    {
        // Variables
        DatosPartido p;
        Boolean vista;
        private DatosPartidoVIEW model = new DatosPartidoVIEW();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = model;
            model.LoadUsers();
            p = new DatosPartido();
            dgvPeople.ItemsSource = model.Parties;
            TextBox2.TextChanged += ChangeNullVotes;
            TextBox3.TextChanged += ChangeNullVotes;
           
        }

        // Método para insertar partidos
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            // Comprobamos que la lista no sea nula
            if (model.Parties == null)
            {
                // Ponemos vista a true para que no haga el foco cuando insertes el primer partido
                vista = true;
                p = new DatosPartido();
                dgvPeople.ItemsSource = p.getListParties();
                previousdata.IsEnabled = true;
                if (vista == false)
                {
                    previousdata.Focus();
                    vista = true;
                }
            }

            // Controla que se completen los 3 campos
            if (string.IsNullOrEmpty(acronimo.Text) || string.IsNullOrEmpty(nPartido.Text) || string.IsNullOrEmpty(txtPresidente.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de agregar una nueva entrada.");
            }
            else
            {
                if (model.Parties.Count > 9)
                {
                    // Inicializamos el objeto de datos previos
                    PreviousData pd = null;
                    int text1, text2, text3;

                    if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox2.Text) && !string.IsNullOrEmpty(TextBox3.Text) &&
                        int.TryParse(TextBox1.Text, out text1) && int.TryParse(TextBox2.Text, out text2) && int.TryParse(TextBox3.Text, out text3))
                    {
                        int validVotes = PreviousData.calculatevalidvotes(text1, text2, text3);
                        pd = new PreviousData(text1, text2, text3, validVotes);
                    }
                    else
                    {
                        // Manejo de error si los valores no son válidos
                        return;
                    }

                    // Inicializamos la lista
                    ObservableCollection<DatosPartido> listaNormal = model.Parties;
                    List<DatosPartido> listaDePartidosF = new List<DatosPartido>(listaNormal);

                    // Añadimos los votos válidos
                    listaDePartidosF = PartidosFactory.inicialiteParties(pd, listaDePartidosF);

                    simulation.IsEnabled = true;
                    dgvParties.ItemsSource = listaDePartidosF;
                    dgvParties.Items.Refresh();
                    managment.IsEnabled = false;
                    previousdata.IsEnabled = false;
                    blanco.Content = "Los votos en blanco son " + Convert.ToInt32(pd.valid_votes * 0.05);
                    simulation.Focus();
                }
                else
                {
                    // Si el registro no existe, procedemos a crearlo
                    DatosPartido newParty = new DatosPartido
                    {
                        Nombre = nPartido.Text,
                        Acronimo = acronimo.Text,
                        Presidente = txtPresidente.Text,
                        Seats = model.Seats,  // Asegúrate de tener un valor adecuado para model.Seats
                        Votes = model.Votes   // Asegúrate de tener un valor adecuado para model.Votes
                    };

                    if (!model.Parties.Any(x => x.Nombre == newParty.Nombre))
                    {
                        model.Parties.Add(newParty);

                        // Una vez agregado el registro al modelo, lo agregamos al archivo JSON
                        model.NewUser();
                    }
                    else
                    {
                        // Si el registro ya existe, actualizamos los valores
                        DatosPartido existingParty = model.Parties.First(x => x.Nombre == newParty.Nombre);
                        existingParty.Acronimo = model.Acronimo;
                        existingParty.Presidente = model.Presidente;
                        existingParty.Seats = model.Seats;
                        existingParty.Votes = model.Votes;

                        // Una vez actualizado el registro en el modelo, lo actualizamos en el archivo JSON
                        model.UpdateParty();
                    }

                    dgvPeople.ItemsSource = model.Parties;
                    dgvPeople.Items.Refresh();
                }
            }
        }


        // Método para borrar partidos
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            // Realizamos un casting de los datos seleccionados
            List<DatosPartido> partidosABorrar = dgvPeople.SelectedItems.Cast<DatosPartido>().ToList();

            foreach (DatosPartido partido in partidosABorrar)
            {
                // Removemos los partidos de ambas listas
                p.removeParties(partido);
                p.Pm.listParties.Remove(partido);
            }

            // Actualizamos la interfaz gráfica
            dgvPeople.Items.Refresh();

            // Obtenemos el partido seleccionado actualmente
            var selectedParty = dgvPeople.SelectedItem as DatosPartido;

            if (selectedParty != null)
            {
                // Eliminamos el elemento seleccionado del modelo y del archivo JSON
                model.DeleteParty(selectedParty.Nombre);
            }

            // Actualizamos nuevamente la interfaz gráfica
            dgvPeople.Items.Refresh();
        }

        // Método para ocultar el botón de borrado
        private void dgvPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvPeople.SelectedItem != null)
            {
                // Seleccionaron una fila, muestra el botón
                btnBorrado.Visibility = Visibility.Visible;
            }
            else
            {
                // No hay fila seleccionada, oculta el botón
                btnBorrado.Visibility = Visibility.Collapsed;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int text1 = 0, text2 = 0, text3;
            // Verificar si todas las cajas de texto están llenas y si contienen números válidos
            if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox2.Text) && !string.IsNullOrEmpty(TextBox3.Text) &&
                int.TryParse(TextBox1.Text, out text1) && int.TryParse(TextBox2.Text, out text2) && int.TryParse(TextBox3.Text, out text3) && text1 > text2)
            {
                PreviousData p;
                // Calculamos los datos validos
                int validvotes = PreviousData.calculatevalidvotes(text1, text2, text3);
                p = new PreviousData(text1, text2, text3, validvotes);
                // Habilitar las pestañas "PARTIES MANAGEMENT" y "SIMULATION"
                managment.IsEnabled = true;
                managment.IsSelected = true;
                previousdata.IsEnabled = false;
            }
            else
            {
                if ((string.IsNullOrEmpty(TextBox2.Text) && string.IsNullOrEmpty(TextBox3.Text)) || text2 == 0)
                {
                    //Comprobamos que sea numeors
                    MessageBox.Show("Uno o más de los valores en las cajas de texto no son números enteros válidos o están vacíos.");
                    managment.IsEnabled = false;
                    simulation.IsEnabled = false;
                }
                if (text1 < text2)
                {
                    //Comprobamos que los Abstetions votes sean menores que population
                    MessageBox.Show("No puede ser el numero de  ABSTENTIONS VOTES mayor que POPULATION");
                    managment.IsEnabled = false;
                    simulation.IsEnabled = false;
                }
            }
        }

        // Calcular los votos nulos
        private void ChangeNullVotes(object sender, RoutedEventArgs e)
        {
            int text1, text2;
            double text3;

            if (!string.IsNullOrEmpty(TextBox2.Text))
            {
                int.TryParse(TextBox1.Text, out text1);
                int.TryParse(TextBox2.Text, out text2);
                double.TryParse(TextBox3.Text, out text3);
                text3 = PreviousData.CalculateNullVotes(text1, text2);
                TextBox3.Text = text3.ToString();
            }
            else
            {
                // Mantener las pestañas deshabilitadas si alguna caja de texto no está llena
                managment.IsEnabled = false;
                simulation.IsEnabled = false;
                MessageBox.Show("Los valores en las cajas de texto no son números enteros válidos.");
            }
        }

        // Boton de simulacion
        private void Simulate_Click(object sender, RoutedEventArgs e)
        {
            // Si es nulo, es decir, has pulsado dos veces seguidas el botón de simulate, te devuelve al principio para comprobar la simulación
            PreviousData pd = new PreviousData(); // Creo una instancia de PreviousData

            // Obtiene los valores necesarios para inicializar PreviousData y luego la utiliza
            int text1, text2, text3;

            if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox2.Text) && !string.IsNullOrEmpty(TextBox3.Text) &&
                int.TryParse(TextBox1.Text, out text1) && int.TryParse(TextBox2.Text, out text2) && int.TryParse(TextBox3.Text, out text3))
            {
                int validVotes = PreviousData.calculatevalidvotes(text1, text2, text3);
                pd = new PreviousData(text1, text2, text3, validVotes);
            }
            else
            {
                // Manejo de error si los valores no son válidos
                return;
            }

            ObservableCollection<DatosPartido> listaNormal = model.Parties;
            foreach (DatosPartido partido in model.Parties)
            {
                partido.Votes = 0;
                partido.Seats = 0;
            }

            List<DatosPartido> listaDePartidosIni = new List<DatosPartido>(listaNormal);
            listaDePartidosIni = PartidosFactory.inicialiteParties(pd, listaDePartidosIni);
            listaDePartidosIni = DatosPartido.CalculateSeats(listaDePartidosIni);

            // Asignar los valores calculados a cada partido individualmente
            foreach (DatosPartido partido in listaDePartidosIni)
            {
                partido.Nombre = partido.Nombre;
                partido.Seats = partido.Seats; // Reemplaza por la lógica que estás utilizando para calcular los escaños
                partido.Votes = partido.Votes; // Reemplaza por la lógica que estás utilizando para calcular los votos
            }

            listaNormal = new ObservableCollection<DatosPartido>(listaDePartidosIni);
            model.Parties = listaNormal;

             // Llama al nuevo método para actualizar todos los partidos en la base de datos

            MessageBox.Show("Se ha realizado la simulación");
            model.Parties = new ObservableCollection<DatosPartido>(p.Pm.listParties);

            backButton.Visibility = Visibility.Visible;
        }

        // Boton de back para volver atrás cuando haces la simulación
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            dgvParties.ItemsSource = null;
            managment.Focus();
            simulation.IsEnabled = false;
            previousdata.IsEnabled = true;
            previousdata.Focus();
            vista = false;
        }
    }
}


