using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class products 
    {
        [Key] public int id_product { get; set; }
        public string product_name { get; set; }
        public double price { get; set; }
        public int units_in_stock { get; set; }
        public int id_supplier { get; set; }
        public DateTime expiration_date { get; set; }
        public bool active { get; set; } = true;
        public string image { get; set; }
        public int id_category { get; set; }
    }
}
