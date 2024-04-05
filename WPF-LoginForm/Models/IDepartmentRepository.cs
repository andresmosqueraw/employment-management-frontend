using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    internal interface IDepartmentRepository
    {

         void InsertarDepartamento(Departamento departmento);
         void ActualizarDepartamento(Departamento departmento);
         List<Departamento> ObtenerDepartamento();
         void EliminarDepartamento(int departmentoId);
    }
}
