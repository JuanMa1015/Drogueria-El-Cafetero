using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;

namespace Drogueria_Elcafetero.Models
{
    public class Discount_Suppliers_invoices 
    {
        [Key] public int id_discount_suppliers_invoices { get; set; }
        public int id_discount {  get; set; }
        public string id_supplier_invoice { get; set; }
        public double discount_amount { get; set; }
    }
}
