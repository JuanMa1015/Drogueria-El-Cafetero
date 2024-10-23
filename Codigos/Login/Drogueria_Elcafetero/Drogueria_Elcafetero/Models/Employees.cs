using System.ComponentModel.DataAnnotations;
namespace Drogueria_Elcafetero.Models
{
    public class Employees
    {
        [Key] public int id_employee { get; set; }
        public string employee_name { get; set; }
        public double salary { get; set; }
        public DateTime hiring_date { get; set; }
        public string email { get; set; }
        public string password_hash { get; set; }

        public Rol id_rol { get; set; }
    }
}
