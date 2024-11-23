using CapaEntidad;
using System;
using System.Collections.Generic;
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
                // Validar parámetros
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
    }
}


