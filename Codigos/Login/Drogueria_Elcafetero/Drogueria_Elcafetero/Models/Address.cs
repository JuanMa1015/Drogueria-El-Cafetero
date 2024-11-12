using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Drogueria_Elcafetero.Models
{
    public class address 
    {
        [DisplayName("ID De La Dirección")]
        [Key] public int id_address { get; set; }

        [DisplayName("Cuidad")]
        public string city_name { get; set; }

        [DisplayName("Descripción")]
        public string description { get; set; }

        [DisplayName("Proveedor")]
        public int id_supplier { get; set; }
    }
}
