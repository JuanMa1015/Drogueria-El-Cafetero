using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class purchase_orders_details
    {
        [DisplayName("ID Detalle Orden")]
        [Key] public int id_order_detail { get; set; }

        [DisplayName("ID Orden")]
        public int id_purchase_order { get; set; }

        [DisplayName("ID Producto")]
        public int id_product { get; set; }

        [DisplayName("Cantidad Producto")]
        public int amount_product { get; set; }

        [DisplayName("Precio Unitario")]
        public double unit_price { get; set; }

        [DisplayName("Subtotal")]
        public double subtotal { get; set; }
    }
}
