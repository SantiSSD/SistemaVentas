using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
namespace CapaDatos
{
    public class CD_PERMISO
    {
        public List<Permiso> Listar(int idusuario)
        {
            // Lista donde se almacenarán los objetos Usuario obtenidos de la base de datos
            List<Permiso> lista = new List<Permiso>();

            // Instancia de la clase AccesoDatos para realizar la conexión y las consultas
            AccesoDatos datos = new AccesoDatos();

            try
            {

                StringBuilder query = new StringBuilder();
                query.AppendLine("select p.IdRol, p.NombreMenu from PERMISO p");
                query.AppendLine("inner join ROL r on r.IdRol = p.IdRol");
                query.AppendLine("inner join USUARIO u on u.IdRol = r.IdRol");
                query.AppendLine("where u.IdUsuario = @idusuario");

                // Usar tu método setearConsulta en lugar de SqlCommand
                datos.setearConsulta(query.ToString());

                // Parámetros en tu método de setear consulta (si no lo tienes, podrías añadir un método para parámetros)
                datos.setearParametro("@idusuario", idusuario);

                // Ejecutar la consulta
                datos.ejecutarLectura();

                // Recorreremos el SqlDataReader, leyendo cada fila obtenida de la base de datos
                // Leer los resultados
                while (datos.Lector.Read())
                {
                    Permiso permiso = new Permiso
                    {
                        oRol = new Rol() {IdRol =Convert.ToInt32(datos.Lector["IdRol"]) },
                        NombreMenu = datos.Lector["NombreMenu"].ToString()
                    };

                    lista.Add(permiso);
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
    }
}

