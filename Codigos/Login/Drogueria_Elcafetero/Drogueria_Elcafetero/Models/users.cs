﻿using Drogueria_Elcafetero.Models;
using System.ComponentModel.DataAnnotations;
namespace Drogueria_el_cafetero.Models
{
    public class users
    {
        [Key] public int id_user { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password_hash { get; set; }
        public string token { get; set; }
        public bool confirmed { get; set; }
        public bool reset_password { get; set; }
        public string confirmed_password { get; set; }

        public string rol {  get; set; }     

    }
}