using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Drogueria_Elcafetero.Models
{
    public class Employees
    {
        [DisplayName("ID Empleado")]
        [Key] public int id_employee { get; set; }

        [DisplayName("Nombre")]
        public string employee_name { get; set; }

        [DisplayName("Salario")]
        public double salary { get; set; }

        [DisplayName("Fecha De Contrato")]
        public DateTime hiring_date { get; set; }

        [DisplayName("Correo")]
        public string email { get; set; }

        [DisplayName("Contraseña")]
        public string password_hash { get; set; }

        [DisplayName("Rol")]
        public string rol {  get; set; }

    }
}
