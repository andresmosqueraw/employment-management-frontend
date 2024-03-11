using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            CargarDatos();
        }

        SqlConnection con = new SqlConnection("Server=34.134.82.88; Database=sistema_empleados; User ID=admin; Password=admin1235711$!;");

        public void limpiar()
        {
            nombre_txt.Clear();
            apellido_txt.Clear();
            cedula_txt.Clear();
            correo_txt.Clear();
            salario_txt.Clear();
            cargo_txt.Clear();
        }

        public void CargarDatos()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Empleados", con);
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }   

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
