using Drogueria_Elcafetero.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Drogueria_el_cafetero.Models
{
    public class users
    {
        [DisplayName("ID Usuario")]
        [Key] public int id_user { get; set; }

        [DisplayName("Nombre")]
        public string user_name { get; set; }

        [DisplayName("Correo")]
        public string email { get; set; }

        [DisplayName("Contraseña")]
        public string password_hash { get; set; }

        [DisplayName("Token")]
        public string token { get; set; }

        [DisplayName("Correo Confirmado")]
        public bool confirmed { get; set; }

        [DisplayName("Contraseña Restablecida")]
        public bool reset_password { get; set; }

        [DisplayName("Contraseña Confirmada")]
        public string confirmed_password { get; set; }

        [DisplayName("Rol")]
        public string rol {  get; set; }     

    }
}
