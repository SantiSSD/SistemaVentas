using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Venta
    {
        AccesoDatos datos = new AccesoDatos();
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("select COUNT(*) + 1 from VENTA");


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

        public bool RestarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("update producto set stock = stock - @cantidad where idproducto = @idproducto");

                datos.setearParametro("@cantidad", cantidad);
                datos.setearParametro("@idproducto", idproducto);
                datos.setearConsulta(query.ToString());

                respuesta = datos.ejecutarAccionConResultado() > 0 ? true : false;
            }

            catch (Exception ex)
            {
                respuesta = false;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return respuesta;
        }


        public bool SumarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("update producto set stock = stock + @cantidad where idproducto = @idproducto");

                datos.setearParametro("@cantidad", cantidad);
                datos.setearParametro("@idproducto", idproducto);
                datos.setearConsulta(query.ToString());

                respuesta = datos.ejecutarAccionConResultado() > 0 ? true : false;
            }

            catch (Exception ex)
            {
                respuesta = false;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return respuesta;
        }
        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = String.Empty;

            try
            {
                if (obj == null || DetalleVenta == null)
                {
                    Mensaje = "Los parámetros no pueden ser nulos.";
                    return false;
                }
                datos.setearProcedimiento("usp_RegistrarVenta");


                datos.setearParametro("IdUsuario", obj.oUsuario.IdUsuario);
                datos.setearParametro("TipoDocumento", obj.TipoDocumento);
                datos.setearParametro("NumeroDocumento", obj.NumeroDocumento);
                datos.setearParametro("DocumentoCliente", obj.DocumentoCliente);
                datos.setearParametro("NombreCliente", obj.NombreCliente);
                datos.setearParametro("MontoPago", obj.MontoPago);
                datos.setearParametro("MontoCambio", obj.MontoCambio);
                datos.setearParametro("MontoTotal", obj.MontoTotal);
                datos.setearParametro("DetalleVenta", DetalleVenta);

                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);

                datos.ejecutarAccion();

                Respuesta = Convert.ToBoolean(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                Respuesta = false;
                Mensaje = "Error al registrar la venta: " + ex.Message;
                throw new Exception("Error interno: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return Respuesta;
        }

        public Venta ObtenerVenta(string numero) 
        {
         Venta obj = new Venta();

            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select v.IdVenta, u.NombreCompleto, ");
                    query.AppendLine("v.DocumentoCliente, v.NombreCliente, ");
                    query.AppendLine("v.TipoDocumento, v.NumeroDocumento,");
                    query.AppendLine("v.MontoPago, v.MontoCambio, v.MontoTotal,");
                    query.AppendLine("convert(char(10),v.FechaRegistro,103)[FechaRegistro]");
                    query.AppendLine("from VENTA v");
                    query.AppendLine("inner join  USUARIO u on u.IdUsuario = v.IdUsuario");
                    query.AppendLine("where v.NumeroDocumento = @numero");


                    datos.setearConsulta(query.ToString());
                    datos.setearParametro("numero", numero);
                    datos.ejecutarLectura();

                    while (datos.Lector.Read())
                    {
                        obj = new Venta();
                        {
                            obj.IdVenta = Convert.ToInt32(datos.Lector["IdVenta"]);
                            obj.oUsuario = new Usuario() { NombreCompleto = datos.Lector["NombreCompleto"].ToString() };
                            obj.DocumentoCliente = datos.Lector["DocumentoCliente"].ToString();
                            obj.NombreCliente = datos.Lector["NombreCliente"].ToString();
                            obj.TipoDocumento = datos.Lector["TipoDocumento"].ToString();
                            obj.NumeroDocumento = datos.Lector["NumeroDocumento"].ToString();
                            obj.MontoPago = Convert.ToDecimal(datos.Lector["MontoPago"].ToString());
                            obj.MontoCambio = Convert.ToDecimal(datos.Lector["MontoCambio"].ToString());
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

        public List<Detalle_Venta> ObtenerDetalleVenta(int idventa) 
        {
            List<Detalle_Venta> oLista = new List<Detalle_Venta>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                StringBuilder query = new StringBuilder();

                query.AppendLine("select p.Nombre, dv.PrecioVenta,dv.Cantidad,dv.SubTotal");
                query.AppendLine("from DETALLE_VENTA dv");
                query.AppendLine("inner join PRODUCTO p on p.IdProducto = dv.IdProducto");
                query.AppendLine("where dv.IdVenta = @idventa");

                datos.setearConsulta(query.ToString());
                datos.setearParametro("@idventa", idventa);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Detalle_Venta detalle = new Detalle_Venta
                    {
                        oProducto = new Producto { Nombre = datos.Lector["Nombre"].ToString() },
                        PrecioVenta = Convert.ToDecimal(datos.Lector["PrecioVenta"]),
                        Cantidad = Convert.ToInt32(datos.Lector["Cantidad"]),
                        SubTotal = Convert.ToDecimal(datos.Lector["SubTotal"])
                    };

                    oLista.Add(detalle);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el detalle de la venta: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return oLista;
        }

    }
}
