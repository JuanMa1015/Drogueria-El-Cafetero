using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class Suppliers_products
    {
        [Key] public int id_supplier_product { get; set; }
        public int id_product { get; set; }
        public int id_supplier { get; set; }
        public double price { get; set; }
        public DateTime agreement_date { get; set; }
    }
}
