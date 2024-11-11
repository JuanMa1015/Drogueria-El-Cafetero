using Drogueria_el_cafetero.Models;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class car
    {
        [Key] public int id_car { get; set; }
        public int id_user { get; set; }
        public int id_product { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }

        [Required]
        public DateTime date { get; set; } = DateTime.Now;


    }
}
