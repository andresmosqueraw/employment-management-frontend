using System.Windows.Input;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.ViewModels
{
    internal class DepartmentsViewModel : ViewModelBase
    {
        //Campos de la vista de departamentos
        private string _departmentName;
        private string _departmentDescription;
        private string _departmentLocation;
        private string _departmentPhone;
        private string _departmentEmail;

        private IDepartmentRepository departmentRepository;

        //Propiedades de la vista de departamentos
        public string DepartmentName
        {
            get
            {
                return _departmentName;
            }

            set
            {
                _departmentName = value;
                OnPropertyChanged(nameof(DepartmentName));
            }
        }

        public string DepartmentDescription
        {
            get
            {
                return _departmentDescription;
            }

            set
            {
                _departmentDescription = value;
                OnPropertyChanged(nameof(DepartmentDescription));
            }
        }

        public string DepartmentLocation
        {
            get
            {
                return _departmentLocation;
            }

            set
            {
                _departmentLocation = value;
                OnPropertyChanged(nameof(DepartmentLocation));
            }
        }

        public string DepartmentPhone
        {
            get
            {
                return _departmentPhone;
            }

            set
            {
                _departmentPhone = value;
                OnPropertyChanged(nameof(DepartmentPhone));
            }
        }

        public string DepartmentEmail
        {
            get
            {
                return _departmentEmail;
            }

            set
            {
                _departmentEmail = value;
                OnPropertyChanged(nameof(DepartmentEmail));
            }
        }

        public ICommand AgregarDepartamento { get; }
        public ICommand EditarDepartamento { get; }
        public ICommand EliminarDepartamento { get; }

        public DepartmentsViewModel(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
            AgregarDepartamento = new ViewModelCommand(AgregarDepartamentoExecute);
            EditarDepartamento = new ViewModelCommand(EditarDepartamentoExecute);
            EliminarDepartamento = new ViewModelCommand(EliminarDepartamentoExecute);

        }

        public DepartmentsViewModel()
        {
        }

        public void AgregarDepartamentoExecute(object parameter)
        {
            Departamento departamento = new Departamento
            {
                Nombre = DepartmentName,
                Descripcion = DepartmentDescription,
                Ubicacion = DepartmentLocation,
                Telefono = DepartmentPhone,
                Email = DepartmentEmail
            };
            departmentRepository.InsertarDepartamento(departamento);
        }
        public void EditarDepartamentoExecute(object parameter)
        {
            Departamento departamento = new Departamento
            {
                Nombre = DepartmentName,
                Descripcion = DepartmentDescription,
                Ubicacion = DepartmentLocation,
                Telefono = DepartmentPhone,
                Email = DepartmentEmail
            };
            departmentRepository.ActualizarDepartamento(departamento);
        }
        public void EliminarDepartamentoExecute(object parameter)
        {
            departmentRepository.EliminarDepartamento(int.Parse(parameter.ToString()));
        }

    }
}