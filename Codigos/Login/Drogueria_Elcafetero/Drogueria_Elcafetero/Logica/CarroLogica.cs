
//using Drogueria_Elcafetero.Models;
//using Npgsql;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Globalization;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Web;
//using System.Xml;
//using System.Xml.Linq;

//namespace ProyectoTest.Logica
//{
//    public class CarritoLogica
//    {
//        private static string CadenaSQL = "Host=ep-jolly-math-a5dsqqss.us-east-2.aws.neon.tech;Database=Drogueria_El_Cafetero;Username=Drogueria_El_Cafetero_owner;Password=ATl8nUMi6IVx;Ssl Mode=Require";
//        private static CarritoLogica _instancia = null;

//        public CarritoLogica()
//        {

//        }

//        public static CarritoLogica Instancia
//        {
//            get
//            {
//                if (_instancia == null)
//                {
//                    _instancia = new CarritoLogica();
//                }

//                return _instancia;
//            }
//        }

//        public int Registrar(car oCarrito)
//        {
//            int respuesta = 0;
//            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
//            {
//                try
//                {
//                    NpgsqlCommand cmd = new NpgsqlCommand("sp_InsertarCarrito", oConexion);
//                    cmd.Parameters.AddWithValue("id_user", oCarrito.id_user);
//                    cmd.Parameters.AddWithValue("id_product", oCarrito.id_product);
//                    cmd.Parameters.Add("Resultado", NpgsqlTypes.NpgsqlDbType.Integer).Direction = ParameterDirection.Output;
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    oConexion.Open();
//                    cmd.ExecuteNonQuery();
//                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

//                }
//                catch (Exception ex)
//                {
//                    respuesta = 0;
//                }
//            }
//            return respuesta;
//        }


//        public int Cantidad(int idusuario)
//        {
//            int respuesta = 0;
//            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
//            {
//                try
//                {
//                    NpgsqlCommand cmd = new NpgsqlCommand("select count(*) from car where id_user = @idusuario", oConexion);
//                    cmd.Parameters.AddWithValue("@idusuario", idusuario);
//                    cmd.CommandType = CommandType.Text;

//                    oConexion.Open();
//                    respuesta = Convert.ToInt32(cmd.ExecuteScalar().ToString());

//                }
//                catch (Exception ex)
//                {
//                    respuesta = 0;
//                }
//            }
//            return respuesta;
//        }

//        public List<car> Obtener(int _idusuario)
//        {
//            List<car> lst = new List<car>();
//            using (NpgsqlConnection oConexion = new NpgsqlConnection(CadenaSQL))
//            {
//                try
//                {
//                    NpgsqlCommand cmd = new NpgsqlCommand("sp_ObtenerCarrito", oConexion);
//                    cmd.Parameters.AddWithValue("IdUsuario", _idusuario);
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    oConexion.Open();

//                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
//                    {
//                        while (dr.Read())
//                        {
//                            lst.Add(new car()
//                            {

//                                IdCarrito = Convert.ToInt32(dr["IdCarrito"].ToString()),
//                                oProducto = new Producto()
//                                {
//                                    IdProducto = Convert.ToInt32(dr["IdProducto"].ToString()),
//                                    Nombre = dr["Nombre"].ToString(),
//                                    oMarca = new Marca() { Descripcion = dr["Descripcion"].ToString() },
//                                    Precio = Convert.ToDecimal(dr["Precio"].ToString(), new CultureInfo("es-PE")),
//                                    RutaImagen = dr["RutaImagen"].ToString()
//                                },

//                            });
//                        }
//                    }

//                }
//                catch (Exception ex)
//                {
//                    lst = new List<Carrito>();
//                }
//            }
//            return lst;
//        }

//        public bool Eliminar(string IdCarrito, string IdProducto)
//        {

//            bool respuesta = true;
//            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
//            {
//                try
//                {
//                    StringBuilder query = new StringBuilder();
//                    query.AppendLine("delete from carrito where idcarrito = @idcarrito");
//                    query.AppendLine("update PRODUCTO set Stock = Stock + 1 where IdProducto = @idproducto");

//                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
//                    cmd.Parameters.AddWithValue("@idcarrito", IdCarrito);
//                    cmd.Parameters.AddWithValue("@idproducto", IdProducto);
//                    cmd.CommandType = CommandType.Text;

//                    oConexion.Open();
//                    cmd.ExecuteNonQuery();

//                }
//                catch (Exception ex)
//                {
//                    respuesta = false;
//                }
//            }
//            return respuesta;
//        }

//        public List<Compra> ObtenerCompra(int IdUsuario)
//        {
//            List<Compra> rptDetalleCompra = new List<Compra>();
//            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
//            {
//                SqlCommand cmd = new SqlCommand("sp_ObtenerCompra", oConexion);
//                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
//                cmd.CommandType = CommandType.StoredProcedure;


//                try
//                {
//                    oConexion.Open();
//                    using (XmlReader dr = cmd.ExecuteXmlReader())
//                    {
//                        while (dr.Read())
//                        {
//                            XDocument doc = XDocument.Load(dr);
//                            if (doc.Element("DATA") != null)
//                            {
//                                rptDetalleCompra = (from c in doc.Element("DATA").Elements("COMPRA")
//                                                    select new Compra()
//                                                    {
//                                                        Total = Convert.ToDecimal(c.Element("Total").Value, new CultureInfo("es-PE")),
//                                                        FechaTexto = c.Element("Fecha").Value,
//                                                        oDetalleCompra = (from d in c.Element("DETALLE_PRODUCTO").Elements("PRODUCTO")
//                                                                          select new DetalleCompra()
//                                                                          {
//                                                                              oProducto = new Producto()
//                                                                              {
//                                                                                  oMarca = new Marca() { Descripcion = d.Element("Descripcion").Value },
//                                                                                  Nombre = d.Element("Nombre").Value,
//                                                                                  RutaImagen = d.Element("RutaImagen").Value
//                                                                              },
//                                                                              Total = Convert.ToDecimal(d.Element("Total").Value, new CultureInfo("es-PE")),
//                                                                              Cantidad = Convert.ToInt32(d.Element("Cantidad").Value)
//                                                                          }).ToList()
//                                                    }).ToList();
//                            }
//                            else
//                            {
//                                rptDetalleCompra = new List<Compra>();
//                            }
//                        }

//                        dr.Close();

//                    }


//                    return rptDetalleCompra;
//                }
//                catch (Exception ex)
//                {
//                    rptDetalleCompra = null;
//                    return rptDetalleCompra;
//                }
//            }
//        }



//    }
//}
