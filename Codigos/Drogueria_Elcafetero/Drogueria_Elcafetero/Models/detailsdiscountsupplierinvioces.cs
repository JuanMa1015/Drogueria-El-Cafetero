using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class detailsdiscountsupplierinvioces
    {
        [DisplayName("ID Descuento Factura Proveedor")]
        [Key] public int IdDiscountSupplierInvoice {  get; set; }

        [DisplayName("Número Factura")]
        public string SupplierInvioceNumber { get; set; }

        [DisplayName("Descripción Descuento")]
        public string DiscountDescription { get; set; }

        [DisplayName("Cantidad Descuento")]
        public double DiscountAmount { get; set; }

        [DisplayName("Total Factura")]
        public double InvioceTotal { get; set; }
    }
}
