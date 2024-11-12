using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class sales_invoices
    {
        [DisplayName("ID Factura Venta")]
        [Key] public int id_invoice { get; set; }

        [DisplayName("ID Venta")]
        public int id_sale { get; set; }

        [DisplayName("Número Factura")]
        public string issue_number { get; set; }

        [DisplayName("Fecha Emisión")]
        public DateTime issue_date { get; set; }

        [DisplayName("Total")]
        public double total_invoice { get; set; }

        [DisplayName("Estado")]
        public string state { get; set; }
    }
}
