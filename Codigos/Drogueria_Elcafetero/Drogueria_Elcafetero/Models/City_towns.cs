using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class city_towns 
    {
        [DisplayName("Cuidad")]
        [Key] public string city_name { get; set; }

        [DisplayName("Departamento")]
        public string department_name { get; set; }

    }
}
