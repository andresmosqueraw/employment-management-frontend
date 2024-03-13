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
            search_txt.Clear();
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

        public bool isValid()
        {
            if (nombre_txt.Text == String.Empty)
            {
                MessageBox.Show("El campo nombre es obligatorio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (apellido_txt.Text == String.Empty)
            {
                MessageBox.Show("El campo apellido es obligatorio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (cedula_txt.Text == String.Empty)
            {
                MessageBox.Show("El campo cedula es obligatorio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (correo_txt.Text == String.Empty)
            {
                MessageBox.Show("El campo correo es obligatorio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (salario_txt.Text == String.Empty)
            {
                MessageBox.Show("El campo salario es obligatorio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (cargo_txt.Text == String.Empty)
            {
                MessageBox.Show("El campo cargo es obligatorio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }   

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into Empleados (empleado_nombre, empleado_apellido, empleado_cedula, empleado_email, empleado_salario, empleado_cargo, departamento_id) values (@nombre, @apellido, @cedula, @correo, @salario, @cargo, @departamento)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@nombre", nombre_txt.Text);
                    cmd.Parameters.AddWithValue("@apellido", apellido_txt.Text);
                    cmd.Parameters.AddWithValue("@cedula", cedula_txt.Text);
                    cmd.Parameters.AddWithValue("@correo", correo_txt.Text);
                    cmd.Parameters.AddWithValue("@salario", decimal.Parse(salario_txt.Text));
                    cmd.Parameters.AddWithValue("@cargo", cargo_txt.Text);
                    cmd.Parameters.AddWithValue("@departamento", 1);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    CargarDatos();
                    MessageBox.Show("Empleado agregado correctamente", "Agregado", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpiar();
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool existId()
        {
            if (search_txt.Text == string.Empty)
            {
                MessageBox.Show("Ingrese primero un ID para actualizar, eliminar u obtener los datos", "Actualizar o Eliminar ID", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (existId())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from Empleados where empleado_id = " + search_txt.Text + " ", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Empleado ha sido eliminado", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    con.Close();
                    limpiar();
                    CargarDatos();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("No fue eliminado" + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            
        }

        private void btnObtener_Click(object sender, RoutedEventArgs e)
        {
            if (existId())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Empleados where empleado_id = " + search_txt.Text + " ", con);
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        nombre_txt.Text = dr["empleado_nombre"].ToString();
                        apellido_txt.Text = dr["empleado_apellido"].ToString();
                        cedula_txt.Text = dr["empleado_cedula"].ToString();
                        correo_txt.Text = dr["empleado_email"].ToString();
                        salario_txt.Text = dr["empleado_salario"].ToString();
                        cargo_txt.Text = dr["empleado_cargo"].ToString();
                    }
                    con.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("No se pudo obtener los datos" + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
               
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (existId() && isValid())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("update Empleados set empleado_nombre = @nombre, empleado_apellido = @apellido, empleado_cedula = @cedula, empleado_email = @correo, empleado_salario = @salario, empleado_cargo = @cargo where empleado_id = " + search_txt.Text + " ", con);
                try
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre_txt.Text);
                    cmd.Parameters.AddWithValue("@apellido", apellido_txt.Text);
                    cmd.Parameters.AddWithValue("@cedula", cedula_txt.Text);
                    cmd.Parameters.AddWithValue("@correo", correo_txt.Text);
                    cmd.Parameters.AddWithValue("@salario", decimal.Parse(salario_txt.Text));
                    cmd.Parameters.AddWithValue("@cargo", cargo_txt.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Empleado ha sido actualizado", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                    con.Close();
                    limpiar();
                    CargarDatos();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("No fue actualizado" + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            
        }
    }
}
