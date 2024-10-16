using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Rol
    {
        public List<Rol> Listar()
        {
            // Lista donde se almacenarán los objetos Usuario obtenidos de la base de datos
            List<Rol> lista = new List<Rol>();

            // Instancia de la clase AccesoDatos para realizar la conexión y las consultas
            AccesoDatos datos = new AccesoDatos();

            try
            {

                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdRol, Descripcion from ROL");
              

                // Usar tu método setearConsulta en lugar de SqlCommand
                datos.setearConsulta(query.ToString());
              
                // Ejecutar la consulta
                datos.ejecutarLectura();

                // Recorreremos el SqlDataReader, leyendo cada fila obtenida de la base de datos
                // Leer los resultados
                while (datos.Lector.Read())
                {
                    Rol rol = new Rol
                    {
                        IdRol = Convert.ToInt32(datos.Lector["IdRol"]),
                        Descripcion = datos.Lector["Descripcion"].ToString(),
                    };

                    lista.Add(rol);
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
