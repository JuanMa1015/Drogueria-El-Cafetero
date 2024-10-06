using System.ComponentModel.DataAnnotations;
namespace Drogueria_el_cafetero.Models
{
    public class customers
    {
        [Key] public int id_customer { get; set; }
        public string customer_name { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public string password_hash { get; set; }
        public string reset_password { get; set; }
        public string confirmed_password { get; set; }
        public string token { get; set; }     

    }
}
