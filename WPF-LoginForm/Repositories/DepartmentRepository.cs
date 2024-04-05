using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Repositories

{
    public class DepartmentRepository : RepositoryBase, IDepartmentRepository
    {

        void IDepartmentRepository.InsertarDepartamento(Departamento departmento)
        {
            var connection = GetConnection();
            var command = new SqlCommand();
            {
                connection.Open();
                command.CommandText = "INSERT INTO Departamentos (departamento_nombre, departamento_descripcion, departamento_ubicacion, departamento_telefono, departamento_email, estado_eliminar) VALUES (@departamento_nombre, @departamento_descripcion, @departamento_ubicacion, @departamento_telefono, @departamento_email, 1)";
                command.Parameters.Add("@departamento_nombre", System.Data.SqlDbType.NVarChar).Value = departmento.Nombre;
                command.Parameters.Add("@departamento_descripcion", System.Data.SqlDbType.NVarChar).Value = departmento.Descripcion;
                command.Parameters.Add("@departamento_ubicacion", System.Data.SqlDbType.NVarChar).Value = departmento.Ubicacion;
                command.Parameters.Add("@departamento_telefono", System.Data.SqlDbType.NVarChar).Value = departmento.Telefono;
                command.Parameters.Add("@departamento_email", System.Data.SqlDbType.NVarChar).Value = departmento.Email;
                command.Connection = connection;
                command.ExecuteNonQuery();

                MessageBox.Show("Departamento registrado correctamente", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        void IDepartmentRepository.ActualizarDepartamento(Departamento departmento)
        {
            throw new NotImplementedException();
        }

        List<Departamento> IDepartmentRepository.ObtenerDepartamento()
        {
            throw new NotImplementedException();
        }

        void IDepartmentRepository.EliminarDepartamento(int departmentoId)
        {
            throw new NotImplementedException();
        }
    }
}
