using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class detailssales
    {
        [DisplayName("ID Venta")]
        [Key] public int IdSale { get; set; }

        [DisplayName("Cliente")]
        public string CustomerName { get; set; }

        [DisplayName("Empleado")]
        public string EmployeeName { get; set; }

        [DisplayName("Fecha")]
        public DateTime SaleDate { get; set; }

        [DisplayName("Total")]
        public decimal TotalSale { get; set; }
    }
}
