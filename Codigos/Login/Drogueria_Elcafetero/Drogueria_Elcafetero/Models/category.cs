using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class category
    {
        [DisplayName("ID Categoría")]
        [Key] public int id_category { get; set; }

        [DisplayName("Categoría")]
        public string category_name { get; set; }

        [DisplayName("Descripción")]
        public string description { get; set; }

        [DisplayName("Activo")]
        public bool active { get; set; } = true;
    }
}
