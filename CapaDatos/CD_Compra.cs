using CapaEntidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Compra
    {
        AccesoDatos datos = new AccesoDatos();
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("select COUNT(*) + 1 from COMPRA");


                datos.setearConsulta(query.ToString());
                idcorrelativo = Convert.ToInt32(datos.ejecutarEscalar());

            }

            catch (Exception ex)
            {
                throw new Exception("Error al obtener el correlativo: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return idcorrelativo;
        }

        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = String.Empty;

            try
            {
                if (obj == null || DetalleCompra == null)
                {
                    Mensaje = "Los parámetros no pueden ser nulos.";
                    return false;
                }
                datos.setearProcedimiento("sp_RegistrarCompra");
                datos.setearParametro("IdUsuario", obj.oUsuario.IdUsuario);
                datos.setearParametro("IdProveedor", obj.oProveedor.IdProveedor);
                datos.setearParametro("TipoDocumento", obj.TipoDocumento);
                datos.setearParametro("NumeroDocumento", obj.NumeroDocumento);
                datos.setearParametro("MontoTotal", obj.MontoTotal);
                datos.setearParametro("DetalleCompra", DetalleCompra);
                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);

                datos.ejecutarAccion();

                Respuesta = Convert.ToBoolean(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                Respuesta = false;
                Mensaje = "Error al registrar la compra: " + ex.Message;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return Respuesta;
        }

        public Compra ObtenerCompra(string numero)
        {
            Compra obj = new Compra();

            {
                List<Usuario> lista = new List<Usuario>();

                AccesoDatos datos = new AccesoDatos();

                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT c.IdCompra,");
                    query.AppendLine("u.NombreCompleto, ");
                    query.AppendLine("pr.Documento, pr.RazonSocial,");
                    query.AppendLine("c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,convert(char(10), c.FechaRegistro, 103)[FechaRegistro]");
                    query.AppendLine("from COMPRA c");
                    query.AppendLine("inner join USUARIO u on u.IdUsuario = c.IdUsuario");
                    query.AppendLine("inner join PROVEEDOR pr on pr.IdProveedor = c.IdProveedor");
                    query.AppendLine("where c.NumeroDocumento = @numero");


                    datos.setearConsulta(query.ToString());
                    datos.setearParametro("numero", numero);
                    datos.ejecutarLectura();

                    while (datos.Lector.Read())
                    {
                        obj = new Compra();
                        {
                            obj.IdCompra = Convert.ToInt32(datos.Lector["IdCompra"]);
                            obj.oUsuario = new Usuario() { NombreCompleto = datos.Lector["NombreCompleto"].ToString() };
                            obj.oProveedor = new Proveedor() { Documento = datos.Lector["Documento"].ToString(), RazonSocial = datos.Lector["RazonSocial"].ToString() };
                            obj.TipoDocumento = datos.Lector["TipoDocumento"].ToString();
                            obj.NumeroDocumento = datos.Lector["NumeroDocumento"].ToString();
                            obj.MontoTotal = Convert.ToDecimal(datos.Lector["MontoTotal"].ToString());
                            obj.FechaRegistro = datos.Lector["FechaRegistro"].ToString();
                        }
                      
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    datos.cerrarConexion();
                }

            }
                return obj;            
        }
        public List<Detalle_Compra> ObtenerDetalleCompra(int idcompra)
        {
            List<Detalle_Compra> oLista = new List<Detalle_Compra>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                StringBuilder query = new StringBuilder();

                query.AppendLine("SELECT p.Nombre, dc.PrecioCompra, dc.Cantidad, dc.MontoTotal");
                query.AppendLine("FROM DETALLE_COMPRA dc");
                query.AppendLine("INNER JOIN PRODUCTO p ON p.IdProducto = dc.IdProducto");
                query.AppendLine("WHERE dc.IdCompra = @idcompra");

                datos.setearConsulta(query.ToString());
                datos.setearParametro("@idcompra", idcompra);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Detalle_Compra detalle = new Detalle_Compra
                    {
                        oProducto = new Producto { Nombre = datos.Lector["Nombre"].ToString() },
                        PrecioCompra = Convert.ToDecimal(datos.Lector["PrecioCompra"]),
                        Cantidad = Convert.ToInt32(datos.Lector["Cantidad"]),
                        MontoTotal = Convert.ToDecimal(datos.Lector["MontoTotal"])
                    };

                    oLista.Add(detalle);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el detalle de la compra: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return oLista;
        }
    }

}

