using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class purchase_orders 
    {
        [Key] public int id_purchase_order { get; set; }
        public int id_supplier { get; set; }
        public DateTime order_date { get; set; }
        public double total_order { get; set; }
        public string state { get; set; }
    }
}
