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
    }
}
