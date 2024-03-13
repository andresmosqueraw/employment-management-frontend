﻿using System;
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
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
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

            SqlCommand cmd = new SqlCommand("SELECT * FROM Departamentos", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
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
            } else if (!long.TryParse(telDep_txt.Text, out telDep)){
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

            } catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int depId = Convert.ToInt32(search_txt.Text);
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Departamentos WHERE departamento_id =" +depId + " ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Departamento eliminado exitosamente", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                clearData();
                LoadGrid();
                con.Close();
            } catch (SqlException ex)
            {
                MessageBox.Show("No eliminado correctamente" + ex.Message);
            } finally { con.Close(); }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            int depId = Convert.ToInt32(search_txt.Text);
            long telDep = Convert.ToInt64(telDep_txt.Text);
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Departamentos SET departamento_nombre = '"+ nombreDep_txt.Text + "', departamento_descripcion = '" + descripDep_txt.Text + "', departamento_ubicacion = '" + ubicDep_txt.Text + "', departamento_telefono = '" + telDep + "', departamento_email = '"+ emailDep_txt.Text +"' WHERE departamento_id = '"+depId+"' ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Departamento actualizado correctamente", "Actualizar", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            } finally
            {
                con.Close();
                clearData();
                LoadGrid();
            }
        }

        private void obtenerBtn_Click(object sender, RoutedEventArgs e)
        {
            int depId = Convert.ToInt32(search_txt.Text);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Departamentos WHERE departamento_id = " + depId + " ", con);
            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    nombreDep_txt.Text = dr["departamento_nombre"].ToString();
                    descripDep_txt.Text = dr["departamento_descripcion"].ToString();
                    ubicDep_txt.Text = dr["departamento_ubicacion"].ToString();
                    telDep_txt.Text = dr["departamento_telefono"].ToString();
                    emailDep_txt.Text = dr["departamento_email"].ToString();
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
}
