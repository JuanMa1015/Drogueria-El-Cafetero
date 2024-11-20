using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drogueria_Elcafetero.Models
{
    public class sales_details
    {
        [DisplayName("ID Detalle Venta")]
        [Key] public int id_detail { get; set; }

        [DisplayName("ID Venta")]
        [Column("id_sale")]
        public int id_sale { get; set; }

        [DisplayName("ID Producto")]
        public int id_product { get; set; }

        [DisplayName("Cantidad Productos")]
        public int amount_products { get; set; }

        [DisplayName("Precio Unitario")]
        public double unit_price { get; set; }

        [DisplayName("Subtotal")]
        public double subtotal { get; set; }

    }
}
