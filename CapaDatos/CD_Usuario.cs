using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using System.Reflection;
using System.Collections;
using System.Security.Claims;
using System.Xml.Linq;
namespace CapaDatos
{
    public class CD_Usuario
    {

        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("SELECT u.IdUsuario,  u.Documento, u.NombreCompleto, u.Correo, u.Clave, u.Estado, r.IdRol, r.Descripcion FROM usuario u");
                query.AppendLine("inner join rol r on r.IdRol = u.IdRol");


                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.IdUsuario = Convert.ToInt32(datos.Lector["IdUsuario"]);
                    usuario.Documento = datos.Lector["Documento"].ToString();
                    usuario.NombreCompleto = datos.Lector["NombreCompleto"].ToString();
                    usuario.Correo = datos.Lector["Correo"].ToString();
                    usuario.Clave = datos.Lector["Clave"].ToString();
                    usuario.Estado = Convert.ToBoolean(datos.Lector["Estado"]);
                    usuario.oRol = new Rol() { IdRol = Convert.ToInt32(datos.Lector["IdRol"]), Descripcion = datos.Lector["Descripcion"].ToString() };

                    lista.Add(usuario);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idusuariogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                AccesoDatos datos = new AccesoDatos();

                datos.setearProcedimiento("SP_REGISTRARUSUARIO");

                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@NombreCompleto", obj.NombreCompleto);
                datos.setearParametro("@Correo", obj.Correo);
                datos.setearParametro("@Clave", obj.Clave);
                datos.setearParametro("@IdRol", obj.oRol.IdRol); 
                datos.setearParametro("@Estado", obj.Estado);

                datos.setearParametro("@IdUsuarioResultado", SqlDbType.Int, ParameterDirection.Output); 
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);   

                datos.ejecutarAccion();

                idusuariogenerado = Convert.ToInt32(datos.ObtenerParametro("@IdUsuarioResultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                idusuariogenerado = 0;
                Mensaje = ex.Message;
            }

            return idusuariogenerado;
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearProcedimiento("SP_EDITARUSUARIO");

                datos.setearParametro("@IdUsuario", obj.IdUsuario);
                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@NombreCompleto", obj.NombreCompleto);
                datos.setearParametro("@Correo", obj.Correo);
                datos.setearParametro("@Clave", obj.Clave);
                datos.setearParametro("@IdRol", obj.oRol.IdRol);  
                datos.setearParametro("@Estado", obj.Estado);
                datos.setearParametro("@Respuesta", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);                                                                                                                                                                                                   

              

                datos.ejecutarAccion();

                respuesta = Convert.ToBoolean(datos.ObtenerParametro("@Respuesta").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();

            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;

            }

            return respuesta;
        }

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
         
                datos.setearProcedimiento("SP_ELIMINARUSUARIO");
                datos.setearParametro("@IdUsuario", obj.IdUsuario);
                datos.setearParametro("@Respuesta", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);

                datos.ejecutarAccion();

                respuesta = Convert.ToBoolean(datos.ObtenerParametro("@Respuesta").Value);
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








