
using Drogueria_Elcafetero.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProyectoTest.Logica
{
    public class CategoriaLogica
    {
        private static string CadenaSQL = "Host=ep-jolly-math-a5dsqqss.us-east-2.aws.neon.tech;Database=Drogueria_El_Cafetero;Username=Drogueria_El_Cafetero_owner;Password=ATl8nUMi6IVx;Ssl Mode=Require";

        private static CategoriaLogica _instancia = null;

        public CategoriaLogica()
        {

        }

        public static CategoriaLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CategoriaLogica();
                }

                return _instancia;
            }
        }

        public List<category> Listar()
        {

            List<category> rptListaCategoria = new List<category>();
            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("sp_obtenerCategoria", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaCategoria.Add(new category()
                        {
                            id_category = Convert.ToInt32(dr["id_category"].ToString()),
                            description = dr["description"].ToString(),
                            active = Convert.ToBoolean(dr["active"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaCategoria;

                }
                catch (Exception ex)
                {
                    rptListaCategoria = null;
                    return rptListaCategoria;
                }
            }
        }


        public bool Registrar(category oCategoria)
        {
            bool respuesta = true;
            using (NpgsqlConnection oConexion = new NpgsqlConnection())
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_RegistrarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("description", oCategoria.description);
                    cmd.Parameters.Add("Resultado", NpgsqlTypes.NpgsqlDbType.Boolean).Direction = ParameterDirection.Output;
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

        public bool Modificar(category oCategoria)
        {
            bool respuesta = true;
            using (NpgsqlConnection oConexion = new NpgsqlConnection())
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("sp_ModificarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("id_category", oCategoria.id_category);
                    cmd.Parameters.AddWithValue("description", oCategoria.description);
                    cmd.Parameters.AddWithValue("active", oCategoria.active);
                    cmd.Parameters.Add("Resultado", NpgsqlTypes.NpgsqlDbType.Boolean).Direction = ParameterDirection.Output;

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

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (NpgsqlConnection oConexion = new NpgsqlConnection())
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("delete from category where id_category = @id", oConexion);
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
