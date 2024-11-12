using Drogueria_el_cafetero.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class sales 
    {
        [DisplayName("ID Venta")]
        [Key] public int id_sale { get; set; }

        [DisplayName("ID Cliente")]
        public int id_customer { get; set; }

        [DisplayName("ID Empleado")]
        public int id_employee { get; set; }

        [DisplayName("Fecha")]
        public DateTime sale_date { get; set; }

        [DisplayName("Total")]
        public double total_sale { get; set; }

        public ICollection<sales_details> sales_details { get; set; }
    }
}
