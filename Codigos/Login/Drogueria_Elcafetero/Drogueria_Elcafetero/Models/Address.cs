using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class address 
    {   
        [Key] public int id_address { get; set; }
        public string city_name { get; set; }
        public string description { get; set; }
        public int id_supplier { get; set; }
    }
}
