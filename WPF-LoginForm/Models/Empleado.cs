using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public decimal Salario { get; set; }
        public string Cargo { get; set; }
        public int DepartamentoId { get; set; }
        public bool EstadoEliminar { get; set; }
    }
}
