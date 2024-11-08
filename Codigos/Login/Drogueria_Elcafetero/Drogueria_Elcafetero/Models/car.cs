using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class car
    {
        [Key] public int id_car { get; set; }
        public int id_user { get; set; }
        public int id_product { get; set; }
    }
}
