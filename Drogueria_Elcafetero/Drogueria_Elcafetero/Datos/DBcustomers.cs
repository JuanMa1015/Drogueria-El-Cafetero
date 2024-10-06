using Drogueria_el_cafetero.Models;
using Npgsql;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Drogueria_Elcafetero.Datos
{
    public class DBcustomers
    {
        private static string CadenaSQL = "server=localhost;username=postgres;database=Drogueria El Cafetero; password=Xmd2004";

        public static bool Registrar(customers customers)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection (CadenaSQL))
                {
                    string query = "INSERT INTO customers (customer_name,telephone,email,password_hash,reset_password," +
                        "confirmed_password,token)";
                    query += " VALUES (@customer_name,@telephone,@email,@password_hash,@reset_password,@token)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@customer_name", customers.customer_name);
                    cmd.Parameters.AddWithValue("@telephone", customers.telephone);
                    cmd.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = customers.email;
                    cmd.Parameters.AddWithValue("@password_hash", customers.password_hash);
                    cmd.Parameters.AddWithValue("@reset_password", customers.reset_password);
                    cmd.Parameters.AddWithValue("@token", customers.token);

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

        public static customers Validar(string correo, string clave)
        {
            customers customer = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "SELECT customer_name,reset_password FROM customers";
                    query += " WHERE email = @email and password_hash = @password_hash";



                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@email", correo);
                    cmd.Parameters.AddWithValue("@password_hash", customer.password_hash);

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new customers()
                            {
                                customer_name = reader["customer_name"].ToString(),
                                reset_password = reader["reset_password"].ToString(),
                                //confirmed = (bool)reader["confirmed"]
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

        public static customers Obtener(string correo)
        {
            customers customer = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "select customer_name, password_hash, reset_password, token from customers";
                    query += " where email = @email";



                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = correo;

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new customers()
                            {
                                customer_name = reader["customer_name"].ToString(),
                                password_hash = reader["password_hash"].ToString(),
                                reset_password = reader["reset_password"].ToString(),
                                //confirmed = (bool)reader["confirmed"],
                                token = reader["token"].ToString()
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

        public static bool RestablecerActualizar(int restablecer, string clave, string token)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"UPDATE customers SET " +
                        "reset_password = @reset_password, " +
                        "password_hash = @password_hash, " +
                        "WHERE token = @token";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@reset_password", restablecer); 
                    cmd.Parameters.AddWithValue("@password_hash", clave);
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
                        "confirmed_password = password_hash " +
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
