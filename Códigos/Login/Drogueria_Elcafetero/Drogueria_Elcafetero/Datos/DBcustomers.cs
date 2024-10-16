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
    public class DBcustomers
    {
        private static string CadenaSQL = "server=localhost;username=postgres;database=Drogueria El Cafetero;password=Xmd2004";

        public static bool Registrar(customers customers)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection (CadenaSQL))
                {
                    string query = "insert into customers (customer_name,email,password_hash,token,confirmed," +
                        "reset_password)";
                    query += " values (@customer_name,@email,@password_hash,@token,@confirmed,@reset_password)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@customer_name", customers.customer_name);
                    cmd.Parameters.AddWithValue("@email", customers.email);
                    cmd.Parameters.AddWithValue("@password_hash", customers.password_hash);
                    cmd.Parameters.AddWithValue("@token", customers.token);
                    cmd.Parameters.AddWithValue("@confirmed", NpgsqlDbType.Boolean).Value = customers.confirmed;
                    cmd.Parameters.AddWithValue("@reset_password", customers.reset_password);


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

        public static customers Validar(string email, string password_hash)
        {
            customers customer = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "SELECT customer_name, reset_password, confirmed FROM customers";
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
                            customer = new customers()
                            {
                                customer_name = reader["customer_name"].ToString(),
                                reset_password = (bool)reader["reset_password"],
                                confirmed = (bool)reader["confirmed"]
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return customer;
        }
        [HttpPost]
        public static customers Obtener(string email)
        {
            customers customers = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "select customer_name,password_hash,reset_password,confirmed,token from customers";
                    query += " where email=@email";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = email;
                    cmd.CommandType= CommandType.Text;


                    conexion.Open();

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            customers = new customers()
                            {
                                customer_name = dr["customer_name"].ToString(),
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

            return customers;
        }

        public static bool RestablecerActualizar(bool reset_password, string password_hash, string token)
        {
            bool respuesta = false;

            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"UPDATE customers SET " +
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
                    string query = @"UPDATE customers SET " +
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
