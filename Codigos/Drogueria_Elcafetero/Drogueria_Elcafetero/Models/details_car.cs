using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class details_car
    {

         [Key] public int CarritoId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public DateTime Fecha { get; set; }
    }
}
