using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class Suppliers
    {
        [Key] public string id_supplier { get; set; }
        public string supplier_name { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
    }
}
