using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class sales_invoices
    {
        [Key] public int id_invoice { get; set; }
        public int id_sale { get; set; }
        public string issue_number { get; set; }
        public DateTime issue_date { get; set; }
        public double total_invoice { get; set; }
        public string state { get; set; }
    }
}
