using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class purchase_orders_invoice
    {
        [Key] public int id_purchase_invoice { get; set; }
         public int id_purchase_order { get; set; }
         public string invoice_number { get; set; }
         public DateTime issue_date { get; set; }
         public double total_invoice { get; set; }
         public string state { get; set; }
    }
}
