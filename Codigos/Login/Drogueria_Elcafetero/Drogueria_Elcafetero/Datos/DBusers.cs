using Drogueria_el_cafetero.Models;
using Npgsql;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NpgsqlTypes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc;

namespace Drogueria_Elcafetero.Datos
{
    public class DBusers
    {
        private static string CadenaSQL = "Host=ep-jolly-math-a5dsqqss.us-east-2.aws.neon.tech;Database=Drogueria_El_Cafetero;Username=Drogueria_El_Cafetero_owner;Password=ATl8nUMi6IVx;Ssl Mode=Require";

        public static bool Registrar(users users)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection (CadenaSQL))
                {
                    string query = "insert into users (user_name,email,password_hash,token,confirmed," +
                        " reset_password)";
                    query += " values (@user_name,@email,@password_hash,@token,@confirmed,@reset_password)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@user_name", NpgsqlDbType.Varchar).Value = users.user_name;
                    cmd.Parameters.AddWithValue("@email", users.email);
                    cmd.Parameters.AddWithValue("@password_hash", users.password_hash);
                    cmd.Parameters.AddWithValue("@token", users.token);
                    cmd.Parameters.AddWithValue("@confirmed", NpgsqlDbType.Boolean).Value = users.confirmed;
                    cmd.Parameters.AddWithValue("@reset_password", users.reset_password);
                    //cmd.Parameters.AddWithValue("@confirmed_password", users.confirmed_password);
                    //cmd.Parameters.AddWithValue("@rol", NpgsqlDbType.Varchar).Value = users.rol;


                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    int filasafectadas = cmd.ExecuteNonQuery();
                    if (filasafectadas > 0) respuesta = true;
                }
                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public static users Validar(string email, string password_hash)
        {
            users user = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "SELECT user_name, reset_password, confirmed, rol FROM users";
                    query += " WHERE email = @email and password_hash = @password_hash";



                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password_hash", password_hash);

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new users()
                            {
                                user_name = reader["user_name"].ToString(),
                                reset_password = (bool)reader["reset_password"],
                                confirmed = (bool)reader["confirmed"],
                                rol = reader["rol"].ToString()
                            };

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return user;
        }
        [HttpPost]
        public static users Obtener(string email)
        {
            users users = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "select user_name,password_hash,reset_password,confirmed,token from users";
                    query += " where email=@email";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = email;
                    cmd.CommandType= CommandType.Text;


                    conexion.Open();

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            users = new users()
                            {
                                user_name = dr["user_name"].ToString(),
                                password_hash = dr["password_hash"].ToString(),
                                reset_password = (bool)dr["reset_password"],
                                confirmed = (bool)dr["confirmed"],
                                token = dr["token"].ToString()
                            };
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return users;
        }

        public static bool RestablecerActualizar(bool reset_password, string password_hash, string token)
        {
            bool respuesta = false;

            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"UPDATE users SET " +
                        "reset_password = @reset_password, " +
                        "password_hash = @password_hash " +
                        "WHERE token = @token";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@reset_password", reset_password); 
                    cmd.Parameters.AddWithValue("@password_hash", password_hash);
                    cmd.Parameters.AddWithValue("@token", NpgsqlTypes.NpgsqlDbType.Varchar).Value = token;

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    int filasafectadas = cmd.ExecuteNonQuery();
                    if (filasafectadas > 0) respuesta = true;
                }
                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool Confirmar(string token)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"UPDATE users SET " +
                        "confirmed = true " +
                        "WHERE token = @token";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@token", token);

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    int filasafectadas = cmd.ExecuteNonQuery();
                    if (filasafectadas > 0) respuesta = true;
                }
                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
