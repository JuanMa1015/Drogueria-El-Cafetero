using System.ComponentModel.DataAnnotations;

namespace Drogueria_Elcafetero.Models
{
    public class category
    {
        [Key] public int id_category { get; set; }
        public string category_name { get; set; }
        public string description { get; set; }
        public bool active { get; set; } = true;
    }
}
