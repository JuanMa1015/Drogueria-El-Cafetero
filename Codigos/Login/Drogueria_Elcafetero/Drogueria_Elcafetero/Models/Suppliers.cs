using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class suppliers
    {
        [Key] public int id_supplier { get; set; }
        public string supplier_name { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public bool active { get; set; } = true;
    }
}
