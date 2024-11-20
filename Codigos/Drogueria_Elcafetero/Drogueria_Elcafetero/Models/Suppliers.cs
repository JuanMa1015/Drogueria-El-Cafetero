using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class suppliers
    {
        [DisplayName("ID Proveedor")]
        [Key] public int id_supplier { get; set; }

        [DisplayName("Proveedor")]
        public string supplier_name { get; set; }

        [DisplayName("Teléfono")]
        public string telephone { get; set; }

        [DisplayName("Correo")]
        public string email { get; set; }

        [DisplayName("Estado")]
        public bool active { get; set; } = true;
    }
}
