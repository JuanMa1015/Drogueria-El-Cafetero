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
    public class DBCustomers
    {
        private static string CadenaSQL = "Host=ep-delicate-bread-a5yfcfjs.us-east-2.aws.neon.tech;Port=5432;Database=Drogueria_El_Cafetero;Username=Drogueria_El_Cafetero_owner;Password=JZNHkhQ3Cl0V;SSL Mode=Require";

        public static bool Registrar(Customers Customers)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection (CadenaSQL))
                {
                    string query = "insert into Customers (customer_name,email,password_hash,token,confirmed," +
                        "reset_password)";
                    query += " values (@customer_name,@email,@password_hash,@token,@confirmed,@reset_password)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@customer_name", Customers.customer_name);
                    cmd.Parameters.AddWithValue("@email", Customers.email);
                    cmd.Parameters.AddWithValue("@password_hash", Customers.password_hash);
                    cmd.Parameters.AddWithValue("@token", Customers.token);
                    cmd.Parameters.AddWithValue("@confirmed", NpgsqlDbType.Boolean).Value = Customers.confirmed;
                    cmd.Parameters.AddWithValue("@reset_password", Customers.reset_password);


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

        public static Customers Validar(string email, string password_hash)
        {
            Customers customer = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "SELECT customer_name, reset_password, confirmed FROM Customers";
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
                            customer = new Customers()
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
        public static Customers Obtener(string email)
        {
            Customers Customers = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "select customer_name,password_hash,reset_password,confirmed,token from Customers";
                    query += " where email=@email";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = email;
                    cmd.CommandType= CommandType.Text;


                    conexion.Open();

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Customers = new Customers()
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

            return Customers;
        }

        public static bool RestablecerActualizar(bool reset_password, string password_hash, string token)
        {
            bool respuesta = false;

            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"UPDATE Customers SET " +
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
                    string query = @"UPDATE Customers SET " +
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
