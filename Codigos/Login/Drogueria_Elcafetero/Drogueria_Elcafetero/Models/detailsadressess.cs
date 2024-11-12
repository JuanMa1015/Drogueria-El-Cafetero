using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class detailsadressess
    {
        [DisplayName("ID Dirección")]
        [Key] public int IdAddress { get; set; }  // Id de la dirección

        [DisplayName("Cuidad")]
        public string CityName { get; set; }  // Nombre de la ciudad

        [DisplayName("Descripción")]
        public string Description { get; set; }  // Descripción

        [DisplayName("Proveedor")]
        public string SupplierName { get; set; }
    }
}
