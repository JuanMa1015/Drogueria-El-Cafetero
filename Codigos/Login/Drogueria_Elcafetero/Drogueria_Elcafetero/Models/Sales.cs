using Drogueria_el_cafetero.Models;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class Sales 
    {
        [Key] public int id_Sale { get; set; }
        public int id_customer { get; set; }
        public int id_employee { get; set; }
        public DateTime sale_date { get; set; }
        public double total_sale { get; set; }
    }
}
