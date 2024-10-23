using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class city_towns 
    {
        [Key] public string city_name { get; set; }
        public string department_name { get; set; }

    }
}
