

using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class VMDepartment_City_towns
    {
        [Key] public string department_name { get; set; }
        public string city_name { get; set; }

    }
}
