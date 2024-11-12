using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class purchase_orders 
    {
        [DisplayName("ID Orden")]
        [Key] public int id_purchase_order { get; set; }

        [DisplayName("ID Proveedor")]
        public int id_supplier { get; set; }

        [DisplayName("Fecha")]
        public DateTime order_date { get; set; }

        [DisplayName("Total")]
        public double total_order { get; set; }

        [DisplayName("Estado")]
        public string state { get; set; }
    }
}
