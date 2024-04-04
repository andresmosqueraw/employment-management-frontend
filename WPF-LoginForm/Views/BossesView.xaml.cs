using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Lógica de interacción para BossesView.xaml
    /// </summary>
    public partial class BossesView : UserControl
    {
        public BossesView()
        {       
            InitializeComponent();
            LoadGrid();

        }

        SqlConnection con = new SqlConnection("Data Source=34.134.82.88;Database=sistema_empleados;Persist Security Info=True; User ID = admin; Password=admin1235711$!");
        public void clearData()
        {
            nombreDep_txt.Clear();
            descripDep_txt.Clear();
            ubicDep_txt.Clear();
            telDep_txt.Clear();
            emailDep_txt.Clear();
            emailDep_txt.Clear();
        }

        private void cleanBtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        public void LoadGrid()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Departamentos", con);
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            // Usar LINQ para contar las filas en el DataTable.
            int rowCount = dt.AsEnumerable().Count(); // Uso de LINQ aquí.

            // Actualizar el contenido del Label para mostrar el recuento de filas.
            lblCount.Content = $"Total de departamentos: {rowCount}";

            // Convertir el DataTable en una DataView y asignarla al DataGrid.
            DataView view = dt.DefaultView;
            dataDep_grid.ItemsSource = dt.DefaultView;

        }

        long telDep;
        public bool isValid()
        {
            if (nombreDep_txt.Text == string.Empty)
            {
                MessageBox.Show("Nombre es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (descripDep_txt.Text == string.Empty)
            {
                MessageBox.Show("Nombre es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (ubicDep_txt.Text == string.Empty)
            {
                MessageBox.Show("Nombre es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            if (telDep_txt.Text == string.Empty)
            {
                MessageBox.Show("Nombre es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!long.TryParse(telDep_txt.Text, out telDep))
            {
                MessageBox.Show("El Contacto debe ser un valor numérico válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (emailDep_txt.Text == string.Empty)
            {
                MessageBox.Show("Nombre es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;

        }
        private void insertBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Departamentos (departamento_nombre, departamento_descripcion, departamento_ubicacion, departamento_telefono, departamento_email) VALUES (@departamento_nombre, @departamento_descripcion, @departamento_ubicacion, @departamento_telefono, @departamento_email)", con);
                    long telDep = Convert.ToInt64(telDep_txt.Text);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@departamento_nombre", nombreDep_txt.Text);
                    cmd.Parameters.AddWithValue("@departamento_descripcion", descripDep_txt.Text);
                    cmd.Parameters.AddWithValue("@departamento_ubicacion", ubicDep_txt.Text);
                    cmd.Parameters.AddWithValue("@departamento_telefono", telDep);
                    cmd.Parameters.AddWithValue("@departamento_email", emailDep_txt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully registered", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            

        }

    
    }
}
