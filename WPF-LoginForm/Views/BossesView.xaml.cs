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

        private string selectedJefeId = string.Empty;

        public BossesView()
        {       
            InitializeComponent();
            LoadGrid();

        }

        SqlConnection con = new SqlConnection("Data Source=34.134.82.88;Database=sistema_empleados;Persist Security Info=True; User ID = admin; Password=admin1235711$!");
        public void limpiar()
        {
            nombreBoss_txt.Clear();
            apellidoBoss_txt.Clear();
            descripBoss_txt.Clear();
            salarioBoss_txt.Clear();
            telBoss_txt.Clear();
            emailBoss_txt.Clear();
        }
        private void cleanBtn_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

            public void LoadGrid()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT jefe_id AS 'ID', jefe_nombre AS 'Nombre', jefe_apellido AS 'Apellido', jefe_celular AS 'Celular', jefe_email AS 'Correo', jefe_salario AS 'Salario', descripcion AS 'Cargo' FROM jefe_departamento  WHERE estado_eliminar = 1", con);
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            // Usar LINQ para contar las filas en el DataTable.
            int rowCount = dt.AsEnumerable().Count(); // Uso de LINQ aquí.

            // Actualizar el contenido del Label para mostrar el recuento de filas.
            lblCount.Content = $"Total de jefes: {rowCount}";

            // Convertir el DataTable en una DataView y asignarla al DataGrid.
            DataView view = dt.DefaultView;
            dataBoss_grid.ItemsSource = view;

        }

        long telBoss;
        public bool isValid()
        {
            if (nombreBoss_txt.Text == string.Empty)
            {
                MessageBox.Show("El nombre es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (apellidoBoss_txt.Text == string.Empty)
            {
                MessageBox.Show("El pellido es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (descripBoss_txt.Text == string.Empty)
            {
                MessageBox.Show("La descripción es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (salarioBoss_txt.Text == string.Empty)
            {
                MessageBox.Show("El salario es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (telBoss_txt.Text == string.Empty)
            {
                MessageBox.Show("El número celular es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!long.TryParse(telBoss_txt.Text, out telBoss))
            {
                MessageBox.Show("El Contacto debe ser un valor numérico válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (emailBoss_txt.Text == string.Empty)
            {
                MessageBox.Show("El correo es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO jefe_departamento (jefe_nombre, jefe_apellido, jefe_salario, jefe_celular, jefe_email, descripcion,estado_eliminar) VALUES (@jefe_nombre, @jefe_apellido, @jefe_salario, @jefe_celular, @jefe_email,@descripcion, @estadoEliminar)", con);
                    long telBoss = Convert.ToInt64(telBoss_txt.Text);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@jefe_nombre", nombreBoss_txt.Text);
                    cmd.Parameters.AddWithValue("@jefe_apellido", apellidoBoss_txt.Text);
                    cmd.Parameters.AddWithValue("@descripcion", descripBoss_txt.Text);
                    cmd.Parameters.AddWithValue("@jefe_salario", decimal.Parse(salarioBoss_txt.Text));
                    cmd.Parameters.AddWithValue("@jefe_celular", telBoss);
                    cmd.Parameters.AddWithValue("@jefe_email", emailBoss_txt.Text);
                    cmd.Parameters.AddWithValue("@estadoEliminar", 1);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();
                    MessageBox.Show("El jefe ha sido registrado ", "Agregado", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpiar();
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedJefeId))
            {
                con.Open();
                // Actualizar en lugar de eliminar
                SqlCommand cmd = new SqlCommand("update jefe_departamento set estado_eliminar = 0 where jefe_id = @jefe_id", con);
                cmd.Parameters.AddWithValue("@jefe_id", selectedJefeId);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Empleado ha sido marcado como eliminado", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    con.Close();
                    limpiar();
                    LoadGrid();
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


        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedJefeId) && isValid())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("update jefe_departamento set jefe_nombre = @jefe_nombre, jefe_apellido = @jefe_apellido, jefe_celular = @jefe_celular, jefe_email = @jefe_email, jefe_salario = @jefe_salario, descripcion = @descripcion where jefe_id = @jefe_id", con);
                
                try
                {
                    cmd.Parameters.AddWithValue("@jefe_id", selectedJefeId);
                    cmd.Parameters.AddWithValue("@jefe_nombre", nombreBoss_txt.Text);
                    cmd.Parameters.AddWithValue("@jefe_apellido", apellidoBoss_txt.Text);
                    cmd.Parameters.AddWithValue("@jefe_celular", telBoss_txt.Text);
                    cmd.Parameters.AddWithValue("@jefe_email", emailBoss_txt.Text);
                    cmd.Parameters.AddWithValue("@jefe_salario", decimal.Parse(salarioBoss_txt.Text));
                    cmd.Parameters.AddWithValue("@descripcion", descripBoss_txt.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Empleado ha sido actualizado", "Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                    con.Close();
                    limpiar();
                    LoadGrid();
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

    
        private void datagrid_bosses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataBoss_grid.SelectedItems.Count > 0)
                {
                    var selectedRow = dataBoss_grid.SelectedItem as DataRowView;
                    if (selectedRow != null)
                    {
                        selectedJefeId = selectedRow["ID"].ToString();
                        nombreBoss_txt.Text = selectedRow["Nombre"].ToString();
                        apellidoBoss_txt.Text = selectedRow["Apellido"].ToString();
                        descripBoss_txt.Text = selectedRow["Cargo"].ToString();
                        emailBoss_txt.Text = selectedRow["Correo"].ToString();
                        salarioBoss_txt.Text = selectedRow["Salario"].ToString();
                        telBoss_txt.Text = selectedRow["Celular"].ToString();
                    }
                }
                else
                {
                    limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }
        }


        public void BuscarJefes(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
            {
                LoadGrid(); // Carga todos los datos si el criterio de búsqueda está vacío
                return;
            }

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT jefe_id AS 'ID', jefe_nombre AS 'Nombre', jefe_apellido AS 'Apellido', jefe_celular AS 'Celular', jefe_email AS 'Correo', jefe_salario AS 'Salario', descripcion AS 'Cargo' FROM jefe_departamento  WHERE estado_eliminar = 1", con);
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();

            // Filtrar usando LINQ
            var query = dt.AsEnumerable().Where(row =>
                row.Field<string>("Nombre").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<string>("Apellido").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<string>("Celular").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<string>("Correo").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<decimal>("Salario").ToString().IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<string>("Cargo").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0);

            DataView view;
            if (query.Any())
            {
                var resultados = query.CopyToDataTable();
                view = resultados.DefaultView;
            }
            else
            {
                // Manejar el caso en que no hay coincidencias
                view = new DataView(); // Una DataView vacía o puedes mostrar un mensaje al usuario, según prefieras.
            }

            dataBoss_grid.ItemsSource = view;
        }

        // Ejemplo de cómo se podría llamar a BuscarEmpleados desde un evento de clic de botón
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string criterioBusqueda = txtCriterioBusqueda.Text; // Asume que existe un TextBox txtCriterioBusqueda para ingresar el criterio de búsqueda
            BuscarJefes(criterioBusqueda);
        }
    }
}
