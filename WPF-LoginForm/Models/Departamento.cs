using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    internal class Departamento
    {
        public int DepartamentoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool EstadoEliminar { get; set; }

    }
}
