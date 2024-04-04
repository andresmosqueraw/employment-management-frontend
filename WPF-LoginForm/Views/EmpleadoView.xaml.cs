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
using System.Runtime.InteropServices;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for EmpleadoView.xaml
    /// </summary>
    public partial class EmpleadoView : UserControl
    {

        private string selectedEmpleadoId = string.Empty;


        public EmpleadoView()
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
            // Especifica las columnas explícitamente y coloca 'empleado_id' primero.
            SqlCommand cmd = new SqlCommand("SELECT empleado_id AS 'ID', empleado_nombre AS 'Nombre', empleado_apellido AS 'Apellido', empleado_cedula AS 'Cédula', empleado_email AS 'Correo', empleado_salario AS 'Salario', empleado_cargo AS 'Cargo' FROM Empleados WHERE estado_eliminar = 1", con);
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();

            // Usar LINQ para contar las filas en el DataTable.
            int rowCount = dt.AsEnumerable().Count(); // Uso de LINQ aquí.

            // Actualizar el contenido del Label para mostrar el recuento de filas.
            lblCount.Content = $"Total de empleados: {rowCount}";

            // Convertir el DataTable en una DataView y asignarla al datagrid_empleados.
            DataView view = dt.DefaultView;
            datagrid_empleados.ItemsSource = view;


            datagrid_empleados.AutoGeneratingColumn += (s, e) =>
            {
                if (e.Column.Header.ToString() == "ID")
                {
                    // Ocultar la columna 'ID'
                    e.Column.Visibility = Visibility.Collapsed;
                }
                // Aquí puedes agregar más lógica si deseas personalizar otros encabezados o propiedades de columna
            };
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
                    SqlCommand cmd = new SqlCommand("insert into Empleados (Nombre, Apellido, Apellido, Correo, Salario, Cargo, departamento_id, estado_eliminar) values (@nombre, @apellido, @cedula, @correo, @salario, @cargo, @departamento, @estadoEliminar)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@nombre", nombre_txt.Text);
                    cmd.Parameters.AddWithValue("@apellido", apellido_txt.Text);
                    cmd.Parameters.AddWithValue("@cedula", cedula_txt.Text);
                    cmd.Parameters.AddWithValue("@correo", correo_txt.Text);
                    cmd.Parameters.AddWithValue("@salario", decimal.Parse(salario_txt.Text));
                    cmd.Parameters.AddWithValue("@cargo", cargo_txt.Text);
                    cmd.Parameters.AddWithValue("@departamento", 1);
                    cmd.Parameters.AddWithValue("@estadoEliminar", 1);
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


        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedEmpleadoId))
            {
                con.Open();
                // Actualizar en lugar de eliminar
                SqlCommand cmd = new SqlCommand("update Empleados set estado_eliminar = 0 where ID = @empleadoId", con);
                cmd.Parameters.AddWithValue("@empleadoId", selectedEmpleadoId);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Empleado ha sido marcado como eliminado", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    con.Close();
                    limpiar();
                    CargarDatos();
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

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedEmpleadoId) && isValid())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("update Empleados set Nombre = @nombre, Apellido = @apellido, Apellido = @cedula, Correo = @correo, Salario = @salario, Cargo = @cargo where ID = " + selectedEmpleadoId + " ", con);
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

        private void datagrid_empleados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (datagrid_empleados.SelectedItems.Count > 0)
                {
                    var selectedRow = datagrid_empleados.SelectedItem as DataRowView;
                    if (selectedRow != null)
                    {
                        selectedEmpleadoId = selectedRow["ID"].ToString();
                        nombre_txt.Text = selectedRow["Nombre"].ToString();
                        apellido_txt.Text = selectedRow["Apellido"].ToString();
                        cedula_txt.Text = selectedRow["Apellido"].ToString();
                        correo_txt.Text = selectedRow["Correo"].ToString();
                        salario_txt.Text = selectedRow["Salario"].ToString();
                        cargo_txt.Text = selectedRow["Cargo"].ToString();
                    }
                } else
                {
                    limpiar();
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }
        }
    }
}
