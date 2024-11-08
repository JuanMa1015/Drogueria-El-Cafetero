using Drogueria_Elcafetero.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ProyectoTest.Logica
{
    public class ProductoLogica
    {
        private static string CadenaSQL = "Host=ep-jolly-math-a5dsqqss.us-east-2.aws.neon.tech;Database=Drogueria_El_Cafetero;Username=Drogueria_El_Cafetero_owner;Password=ATl8nUMi6IVx;Ssl Mode=Require";
        private static ProductoLogica _instancia = null;

        public ProductoLogica()
        {

        }

        public static ProductoLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new ProductoLogica();
                }

                return _instancia;
            }
        }

        public List<products> Listar()
        {

            List<products> rptListaProducto = new List<products>();
            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_obtenerProducto", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaProducto.Add(new products()
                        {
                            id_product = Convert.ToInt32(dr["id_product"].ToString()),
                            product_name = dr["product_name"].ToString(),
                            price = Convert.ToDouble(dr["Descripcion"].ToString()),
                            units_in_stock = Convert.ToInt32(dr["units_in_stock"].ToString()),
                            expiration_date = Convert.ToDateTime(dr["expiration_date"].ToString()),
                            active = Convert.ToBoolean(dr["active"].ToString()),
                            image = dr["image"].ToString(),
                            id_category = Convert.ToInt32(dr["id_category"].ToString())

                        });
                    }
                    dr.Close();

                    return rptListaProducto;

                }
                catch (Exception ex)
                {
                    rptListaProducto = null;
                    return rptListaProducto;
                }
            }
        }



        public int Registrar(products oProducto)
        {
            int respuesta = 0;
            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_registrarProducto", oConexion);
                    cmd.Parameters.AddWithValue("product_name", oProducto.product_name);
                    cmd.Parameters.AddWithValue("price", oProducto.price);
                    cmd.Parameters.AddWithValue("units_in_stock", oProducto.units_in_stock);
                    cmd.Parameters.AddWithValue("expiration_date", oProducto.expiration_date);
                    cmd.Parameters.AddWithValue("active", oProducto.active);
                    cmd.Parameters.AddWithValue("image", oProducto.image);
                    cmd.Parameters.AddWithValue("id_category", oProducto.id_category);
                    cmd.Parameters.Add("Resultado", NpgsqlTypes.NpgsqlDbType.Integer).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }

        public bool Modificar(products oProducto)
        {
            bool respuesta = false;
            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_editarProducto", oConexion);
                    cmd.Parameters.AddWithValue("id_product", oProducto.id_product);
                    cmd.Parameters.AddWithValue("product_name", oProducto.product_name);
                    cmd.Parameters.AddWithValue("price", oProducto.price);
                    cmd.Parameters.AddWithValue("units_in_stock", oProducto.units_in_stock);
                    cmd.Parameters.AddWithValue("expiration_date", oProducto.expiration_date);
                    cmd.Parameters.AddWithValue("active", oProducto.active);
                    cmd.Parameters.AddWithValue("image", oProducto.image);
                    cmd.Parameters.AddWithValue("id_category", oProducto.id_category);
                    cmd.Parameters.Add("Resultado", NpgsqlTypes.NpgsqlDbType.Integer).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }


        public bool ActualizarRutaImagen(products oProducto)
        {
            bool respuesta = true;
            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_actualizarRutaImagen", oConexion);
                    cmd.Parameters.AddWithValue("id_product", oProducto.id_product);
                    cmd.Parameters.AddWithValue("image", oProducto.image);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("delete from products where id_product = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }

    }
}