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

        // Método para obtener una lista de usuarios desde la base de datos
        public List<Usuario> Listar()
        {
            // Lista donde se almacenarán los objetos Usuario obtenidos de la base de datos
            List<Usuario> lista = new List<Usuario>();

            // Instancia de la clase AccesoDatos para realizar la conexión y las consultas
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Preparamos la consulta SQL para obtener los registros de la tabla "Usuarios"
                StringBuilder query = new StringBuilder();
                query.AppendLine("SELECT u.IdUsuario,  u.Documento, u.NombreCompleto, u.Correo, u.Clave, u.Estado, r.IdRol, r.Descripcion FROM usuario u");
                query.AppendLine("inner join rol r on r.IdRol = u.IdRol");


                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                // Recorreremos el SqlDataReader, leyendo cada fila obtenida de la base de datos
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



                    // Agregamos el objeto Usuario a la lista
                    lista.Add(usuario);
                }
            }
            catch (Exception ex)
            {
                throw ex; // Capturar cualquier excepción y relanzarla
            }
            finally
            {
                // Cerramos la conexión a la base de datos y el SqlDataReader
                datos.cerrarConexion();
            }

            // Retornamos la lista de usuarios obtenidos
            return lista;
        }
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idusuariogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                // Creamos una instancia de la clase AccesoDatos
                AccesoDatos datos = new AccesoDatos();

                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_REGISTRARUSUARIO");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@NombreCompleto", obj.NombreCompleto);
                datos.setearParametro("@Correo", obj.Correo);
                datos.setearParametro("@Clave", obj.Clave);
                datos.setearParametro("@IdRol", obj.oRol.IdRol); // Asegúrate de pasar el ID del rol
                datos.setearParametro("@Estado", obj.Estado);

                // Los parámetros de salida del procedimiento almacenado
                datos.setearParametro("@IdUsuarioResultado", SqlDbType.Int, ParameterDirection.Output);  // No es necesario el tamaño porque es INT.
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);    // Aquí especificamos el tamaño para VARCHAR.


                // Ejecutamos la acción (comando)
                datos.ejecutarAccion();

                // Obtenemos los valores de los parámetros de salida
                idusuariogenerado = Convert.ToInt32(datos.ObtenerParametro("@IdUsuarioResultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                // En caso de error, devolvemos el mensaje de la excepción
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
                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_EDITARUSUARIO");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@IdUsuario", obj.IdUsuario);
                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@NombreCompleto", obj.NombreCompleto);
                datos.setearParametro("@Correo", obj.Correo);
                datos.setearParametro("@Clave", obj.Clave);
                datos.setearParametro("@IdRol", obj.oRol.IdRol);  // Asegúrate de pasar el ID del rol
                datos.setearParametro("@Estado", obj.Estado);
                datos.setearParametro("@Respuesta", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output);

                datos.ejecutarLectura();

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
                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_ELIMINARUSUARIO");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@IdUsuario", obj.IdUsuario);
                datos.setearParametro("@Respuesta", SqlDbType.Int, ParameterDirection.Output);
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);

                datos.ejecutarLectura();

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








