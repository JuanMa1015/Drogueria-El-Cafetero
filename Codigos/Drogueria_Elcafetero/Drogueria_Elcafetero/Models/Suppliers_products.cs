using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class suppliers_products
    {
        [DisplayName("ID Producto Proveedor")]
        [Key] public int id_supplier_product { get; set; }

        [DisplayName("ID Producto")]
        public int id_product { get; set; }

        [DisplayName("ID Proveedor")]
        public int id_supplier { get; set; }

        [DisplayName("Precio")]
        public double price { get; set; }

        [DisplayName("Fecha")]
        public DateTime agreement_date { get; set; }
    }
}
