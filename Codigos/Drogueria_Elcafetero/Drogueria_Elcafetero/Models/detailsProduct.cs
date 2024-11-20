using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class detailsProduct
    {
        [DisplayName("ID Producto")]
        [Key] public int IdProduct { get; set; }

        [DisplayName("Producto")]
        public string ProductName { get; set; }

        [DisplayName("Proveedor")]
        public string SupplierName { get; set; }

        [DisplayName("Categoría")]
        public string CategoryName { get; set; }

        [DisplayName("Precio")]
        public decimal Price { get; set; }

        [DisplayName("Unidades Disponibles")]
        public int UnitsInStock { get; set; }

        [DisplayName("Fecha De Vencimiento")]
        public DateTime ExpirationDate { get; set; }

        [DisplayName("Activo")]
        public bool Active { get; set; }

        [DisplayName("Imagen")]
        public string Image { get; set; }
    }
}
