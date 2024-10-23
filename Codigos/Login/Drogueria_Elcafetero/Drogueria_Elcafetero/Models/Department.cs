using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class Department
    {
        [Key] public string department_name { get; set; }
    }
}
