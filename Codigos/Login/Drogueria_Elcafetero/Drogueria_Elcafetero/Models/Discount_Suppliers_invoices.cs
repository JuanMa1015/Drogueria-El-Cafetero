using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;

namespace Drogueria_Elcafetero.Models
{
    public class discount_suppliers_invoices 
    {
        [DisplayName("ID Descuento Factura Proveedor")]
        [Key] public int id_discount_suppliers_invoices { get; set; }

        [DisplayName("ID Descuento")]
        public int id_discount {  get; set; }

        [DisplayName("ID Factura Proveedor")]
        public int id_supplier_invoice { get; set; }

        [DisplayName("Monto Del Descuento")]
        public double discount_amount { get; set; }
    }
}
