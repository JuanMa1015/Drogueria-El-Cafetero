using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class detailspurchaseorders
    {
        [DisplayName("ID Orden")]
        [Key] public int IdPurchaseOrder { get; set; }

        [DisplayName("Proveedor")]
        public string SupplierName { get; set; }

        [DisplayName("Fecha")]
        public DateTime OrderDate { get; set; }

        [DisplayName("Total")]
        public decimal TotalOrder { get; set; }

        [DisplayName("Estado")]
        public string State { get; set; }
    }
}
