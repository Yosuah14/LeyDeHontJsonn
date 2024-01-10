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
            //Comprobamos que la lista no sea nula
            if (p.Pm.listParties == null)
            {
                //Ponemos vista a true para que no haga el foco cuando insertes el primer partido
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
            //Contralos que se completen los 3 campos
            if (string.IsNullOrEmpty(acronimo.Text) || string.IsNullOrEmpty(nPartido.Text) || string.IsNullOrEmpty(txtPresidente.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de agregar una nueva entrada.");
            }
            else
            {
                if (model.Parties == null)
                {
                    p.addParties(acronimo.Text, nPartido.Text, txtPresidente.Text);
                    dgvPeople.ItemsSource = model.Parties;

                    dgvPeople.Items.Refresh();
                    if (model.Parties == null) model.Parties = new ObservableCollection<DatosPartido>();
                    //Si el registro no existe, procedemos a crearlo
                    if (model.Parties.Where(x => x.Nombre == model.Nombre).FirstOrDefault() == null)
                    {
                        model.Parties.Add(new DatosPartido
                        {
                            Nombre = model.Nombre,
                            Acronimo = model.Acronimo,
                            Presidente = model.Presidente,
                            Seats = model.Seats,
                            Votes = model.Votes
                        });
                        //una vez agregado el registro al modelo, lo agregamos a la BDD
                        model.NewUser();



                        dgvPeople.ItemsSource = model.Parties;
                        dgvPeople.Items.Refresh();
                    }
                }
                if (model.Parties.Count > 9)
                {
                    //Inicializamos el objeto de datos previos
                    PreviousData pd = new PreviousData();
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
                    //Anicializamos la lista
                    ObservableCollection<DatosPartido> listaNormal = model.Parties;
                    List<DatosPartido> listaDePartidosF = new List<DatosPartido>(listaNormal);
                    //Añadimos los votos validos
                    listaDePartidosF = PartidosFactory.inicialiteParties(pd, listaDePartidosF);
                    simulation.IsEnabled = true;
                    dgvParties.ItemsSource = listaDePartidosF;
                    dgvParties.Items.Refresh();
                    managment.IsEnabled = false;
                    previousdata.IsEnabled = false;
                    foreach (DatosPartido partido in model.Parties)
                    {
                        partido.Votes = 0;
                        partido.Seats = 0;
                    }
                    blanco.Content = "Los votos en blanco son " + Convert.ToInt32(pd.valid_votes * 0.05);
                    simulation.Focus();
                }
                else
                {
                    p.addParties(acronimo.Text, nPartido.Text, txtPresidente.Text);
                    model.Acronimo=acronimo.Text; 
                    model.Nombre=nPartido.Text;
                    model.Presidente= txtPresidente.Text;
                    dgvPeople.Items.Refresh();
                    if (model.Parties == null) model.Parties = new ObservableCollection<DatosPartido>();
                    //Si el registro no existe, procedemos a crearlo
                    if (model.Parties.Where(x => x.Nombre == model.Nombre).FirstOrDefault() == null)
                    {
                        model.Parties.Add(new DatosPartido
                        {
                            Nombre = model.Nombre,
                            Acronimo = model.Acronimo,
                            Presidente = model.Presidente,
                            Seats = model.Seats,
                            Votes = model.Votes
                        });
                        //una vez agregado el registro al modelo, lo agregamos a la BDD
                        model.NewUser();
                        dgvPeople.ItemsSource = model.Parties;
                        dgvPeople.Items.Refresh();
                    }
                    else
                    {
                        foreach (DatosPartido r in model.Parties)
                        {

                            r.Acronimo = model.Acronimo;
                            r.Presidente = model.Presidente;
                            r.Seats = model.Seats;
                            r.Votes = model.Votes;
                            break;
                        }
                        //Actualizamos
                        model.UpdateParty();
                        dgvPeople.ItemsSource = model.Parties;
                        dgvPeople.Items.Refresh();
                    }
                }
                // Todos los campos están llenos y no se ha alcanzado el límite de 10 partidos, puedes agregar la entrada              
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
                // Eliminamos el elemento seleccionado del modelo y de la base de datos
                model.DeleteParty(selectedParty);
                model.Parties.Remove(selectedParty);
                
            }
            dgvPeople.ItemsSource = model.Parties;
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
          
            List<DatosPartido> listaDePartidosIni = new List<DatosPartido>(listaNormal);
            listaDePartidosIni = PartidosFactory.inicialiteParties(pd, listaDePartidosIni);
            listaDePartidosIni = DatosPartido.CalculateSeats(listaDePartidosIni);


            // Llama al nuevo método para actualizar todos los partidos en la base de datos
            model.UpdatePartyAll(model.Parties);
            dgvPeople.ItemsSource = model.Parties;
            dgvPeople.Items.Refresh();

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


