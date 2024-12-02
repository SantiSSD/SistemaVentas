using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Proveedor
    {
        public List<Proveedor> Listar()
        {
            List<Proveedor> lista = new List<Proveedor>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdProveedor, Documento, RazonSocial, Correo, Telefono, Estado from PROVEEDOR");

                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Proveedor Proveedor = new Proveedor();
                    Proveedor.IdProveedor = Convert.ToInt32(datos.Lector["IdProveedor"]);
                    Proveedor.Documento = datos.Lector["Documento"].ToString();
                    Proveedor.RazonSocial = datos.Lector["RazonSocial"].ToString();
                    Proveedor.Correo = datos.Lector["Correo"].ToString();
                    Proveedor.Telefono = datos.Lector["Telefono"].ToString();
                    Proveedor.Estado = Convert.ToBoolean(datos.Lector["Estado"]);
                    lista.Add(Proveedor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }
        public int Registrar(Proveedor obj, out string Mensaje)
        {
            int idProveedorgenerado = 0;
            Mensaje = string.Empty;

            try
            {
                AccesoDatos datos = new AccesoDatos();

                datos.setearProcedimiento("sp_RegistrarProveedor");

                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@RazonSocial", obj.RazonSocial);
                datos.setearParametro("@Correo", obj.Correo);
                datos.setearParametro("@Telefono", obj.Telefono);
                datos.setearParametro("@Estado", obj.Estado);

                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);
                datos.ejecutarAccion();

                idProveedorgenerado = Convert.ToInt32(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                idProveedorgenerado = 0;
                Mensaje = ex.Message;
            }

            return idProveedorgenerado;
        }

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearProcedimiento("sp_ModificarProveedor");

                datos.setearParametro("@IdProveedor", obj.IdProveedor);
                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@RazonSocial", obj.RazonSocial);
                datos.setearParametro("@Correo", obj.Correo);
                datos.setearParametro("@Telefono", obj.Telefono);
                datos.setearParametro("@Estado", obj.Estado);
                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);



                datos.ejecutarAccion();

                respuesta = Convert.ToBoolean(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();




            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;

            }

            return respuesta;
        }

        public bool Eliminar(Proveedor obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearProcedimiento("sp_EliminarProveedor");

                datos.setearParametro("@IdProveedor", obj.IdProveedor);
                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);

                datos.ejecutarAccion();

                respuesta = Convert.ToBoolean(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();




            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;

            }

            return respuesta;

        }

    }
}
