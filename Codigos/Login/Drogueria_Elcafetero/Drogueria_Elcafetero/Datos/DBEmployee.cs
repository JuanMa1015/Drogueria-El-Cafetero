using Drogueria_el_cafetero.Models;
using Npgsql;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NpgsqlTypes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc;
using Drogueria_Elcafetero.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Drogueria_Elcafetero.Datos
{
    public class DBEmployee
    {
        private static string CadenaSQL = "Host=ep-round-sun-a5bxi93t.us-east-2.aws.neon.tech;Database=Drogueria_El_Cafetero;Username=Drogueria_El_Cafetero_owner;Password=rxaQSkAh92RU;SSL Mode=Require;Trust Server Certificate=true";

        public Employees EncontrarUsuarios(string email, string password_hash)
        {
            Employees employees = new Employees();

            using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
            {
                string query = "Select employee_name, email, password_hash, id_rol from Employees where @email = email and @password_hash = " +
                    "password_hash";

                NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password_hash", password_hash);

                cmd.CommandType = CommandType.Text;

                conexion.Open();    

                using (NpgsqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        employees = new Employees()
                        {
                            employee_name = dr["employee_name"].ToString(),
                            email = dr["email"].ToString(),
                            password_hash = dr["password_hash"].ToString(),
                            id_rol = (rol)dr["id_Rol"]
                        };
                    }
                }
            }
            return employees;
        }

        public Employees EncontrarAdministrador(string email, string password_hash, int id_rol)
        {
            Employees employees = new Employees();

            using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
            {
                string query = "Select employee_name, email, password_hash, id_rol from Employees where @email = email and @password_hash = " +
                    "password_hash and id:rol = 1";

                NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password_hash", password_hash);
                cmd.Parameters.AddWithValue("id_rol", id_rol); 

                cmd.CommandType = CommandType.Text;

                conexion.Open();

                using (NpgsqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        employees = new Employees()
                        {
                            employee_name = dr["employee_name"].ToString(),
                            email = dr["email"].ToString(),
                            password_hash = dr["password_hash"].ToString(),
                            id_rol = (rol)dr["id_Rol"]
                        };
                    }
                }
            }
            return employees;
        }
    }
}
