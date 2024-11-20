using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class suppliers_invoices
    {
        [DisplayName("ID Factura Proveedor")]
        [Key] public int id_supplier_invoice { get; set; }

        [DisplayName("ID Orden")]
        public int id_purchase_order { get; set; }

        [DisplayName("Número Factura")]
        public string invoice_number { get; set; }

        [DisplayName("Fecha Emisión")]
        public DateTime issue_date { get; set; }

        [DisplayName("Total")]
        public double total_invoice { get; set; }

        [DisplayName("Estado")]
        public string state { get; set; }
    }
}
