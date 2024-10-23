using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class Purchase_Orders_Details
    {
        [Key] public int id_order_detail { get; set; }
        public int id_purchase_order { get; set; }
        public int id_product { get; set; }
        public int amount_product { get; set; }
        public double unit_price { get; set; }
        public double subtotal { get; set; }
    }
}
