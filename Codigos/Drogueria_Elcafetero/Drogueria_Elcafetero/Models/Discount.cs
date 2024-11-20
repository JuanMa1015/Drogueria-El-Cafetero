using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class discount 
    {
        [DisplayName("ID Descuento")]
        [Key] public int id_discount { get; set; }

        [DisplayName("ID Factura Proveedor")]
        public int id_supplier_invoice { get; set; }

        [DisplayName("Descripción")]
        public string description { get; set; }

        [DisplayName("Tipo De Descuento")]
        public string discount_type { get;  set; }

        [DisplayName("Valor Del Descuento")]
        public double discount_value { get;  set; }

        [DisplayName("Fceha Inicio Descuento")]
        public DateTime start_date { get;  set; }

        [DisplayName("Fceha Final Descuento")]
        public DateTime end_date { get;  set; }

        [DisplayName("Condiciones")]
        public string conditions { get;  set; }
    }
}
