using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class Sales_Details
    {
        [Key] public int id_detail { get; set; }
        public int id_sale { get; set; }
        public int id_product { get; set; }
        public int amount_products { get; set; }
        public double unit_price { get; set; }
        public double subtotal { get; set; }
    }
}
