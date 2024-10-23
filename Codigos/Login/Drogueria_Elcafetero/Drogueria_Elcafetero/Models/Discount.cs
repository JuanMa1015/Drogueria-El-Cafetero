using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class Discount 
    {
        [Key] public int id_discount { get; set; }
        public int id_supplier_invoice { get; set; }
        public string description { get; set; }
        public string discount_type { get;  set; }
        public double discount_value { get;  set; }
        public DateTime star_date { get;  set; }
        public DateTime end_date { get;  set; }
        public string conditions { get;  set; }
    }
}
