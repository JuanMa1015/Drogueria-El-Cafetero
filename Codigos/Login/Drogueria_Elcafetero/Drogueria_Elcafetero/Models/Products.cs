using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class products 
    {
        [DisplayName("ID Producto")]
        [Key] public int id_product { get; set; }

        [DisplayName("Producto")]
        public string product_name { get; set; }

        [DisplayName("Precio")]
        public double price { get; set; }

        [DisplayName("Unidades Disponibles")]
        public int units_in_stock { get; set; }

        [DisplayName("ID Proveedor")]
        public int id_supplier { get; set; }

        [DisplayName("Fecha Expiración")]
        public DateTime expiration_date { get; set; }

        [DisplayName("Activo")]
        public bool active { get; set; } = true;

        [DisplayName("Imagen")]
        public string image { get; set; }

        [DisplayName("Categoría")]
        public int id_category { get; set; }

    }
}
