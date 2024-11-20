using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class Department
    {
        [DisplayName("Departamento")]
        [Key] public string department_name { get; set; }
    }
}
