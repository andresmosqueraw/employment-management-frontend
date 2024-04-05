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
    
    public partial class CustomerView : UserControl
    {

        private string selectedId;

   

        public CustomerView()
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
            SqlCommand cmd = new SqlCommand("SELECT departamento_id as 'ID', departamento_nombre as 'Departamento', departamento_descripcion as 'Descripcion', departamento_ubicacion as 'Ubicacion', departamento_telefono as 'Telefono', departamento_email as 'Email' FROM Departamentos WHERE estado_eliminar = 1", con);
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
            dataDep_grid.ItemsSource = view;

            dataDep_grid.AutoGeneratingColumn += (s, e) =>
            {
                if (e.Column.Header.ToString() == "ID")
                {
                    // Ocultar la columna 'ID'
                    e.Column.Visibility = Visibility.Collapsed;
                }
            };

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
                MessageBox.Show("La Descripción es requerida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (ubicDep_txt.Text == string.Empty)
            {
                MessageBox.Show("La Ubicación es requerida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

           
            if (telDep_txt.Text == string.Empty)
            {
                MessageBox.Show("Contacto es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else if (!long.TryParse(telDep_txt.Text, out telDep)){
                MessageBox.Show("El Contacto debe ser un valor numérico válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            } 

            if (emailDep_txt.Text == string.Empty)
            {
                MessageBox.Show("Email es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                SqlCommand cmd = new SqlCommand("INSERT INTO Departamentos (departamento_nombre, departamento_descripcion, departamento_ubicacion, departamento_telefono, departamento_email, estado_eliminar) VALUES (@departamento_nombre, @departamento_descripcion, @departamento_ubicacion, @departamento_telefono, @departamento_email, 1)", con);
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

            } catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

        }


        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedId))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Departamentos SET estado_eliminar = 0 WHERE departamento_id = @depId", con);
                cmd.Parameters.AddWithValue("@depId", selectedId);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Departamento eliminado exitosamente", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                    con.Close();
                    clearData();
                    LoadGrid();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("No se pudo eliminar correctamente: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedId) && isValid())
            {
                long telDep = Convert.ToInt64(telDep_txt.Text);
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Departamentos SET departamento_nombre = '" + nombreDep_txt.Text + "', departamento_descripcion = '" + descripDep_txt.Text + "', departamento_ubicacion = '" + ubicDep_txt.Text + "', departamento_telefono = '" + telDep + "', departamento_email = '" + emailDep_txt.Text + "' WHERE departamento_id = '" + selectedId + "' ", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Departamento actualizado correctamente", "Actualizar", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                    clearData();
                    LoadGrid();
                }
            }
            
        }

        private void dataDep_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataDep_grid.SelectedItems.Count > 0)
                {
                    var selectedRow = dataDep_grid.SelectedItems[0] as DataRowView;
                    if (selectedRow != null)
                    {
                        selectedId = selectedRow["ID"].ToString();
                        nombreDep_txt.Text = selectedRow["Departamento"].ToString();
                        descripDep_txt.Text = selectedRow["Descripcion"].ToString();
                        ubicDep_txt.Text = selectedRow["Ubicacion"].ToString();
                        telDep_txt.Text = selectedRow["Telefono"].ToString();
                        emailDep_txt.Text = selectedRow["Email"].ToString();
                    }
                } else
                {
                    clearData();
                }          
            
            } catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + " - " + ex.Source);
            }
        }



        public void BuscarDepartamentos(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
            {
                LoadGrid(); // Carga todos los datos si el criterio de búsqueda está vacío
                return;
            }

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT departamento_id as 'ID', departamento_nombre as 'Departamento', departamento_descripcion as 'Descripcion', departamento_ubicacion as 'Ubicacion', departamento_telefono as 'Telefono', departamento_email as 'Email' FROM Departamentos WHERE estado_eliminar = 1", con);
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();

            // Filtrar usando LINQ
            var query = dt.AsEnumerable().Where(row =>
                row.Field<string>("Departamento").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<string>("Descripcion").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<string>("Ubicacion").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<long>("Telefono").ToString().IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0
                || row.Field<string>("Email").IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0);


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

            dataDep_grid.ItemsSource = view;
        }

        // Ejemplo de cómo se podría llamar a BuscarEmpleados desde un evento de clic de botón
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string criterioBusqueda = txtCriterioBusqueda.Text; // Asume que existe un TextBox txtCriterioBusqueda para ingresar el criterio de búsqueda
            BuscarDepartamentos(criterioBusqueda);
        }

    }
}
