using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Categoria
    {
        // Método para obtener una lista de Categorias desde la base de datos
        public List<Categoria> Listar()
        {
            // Lista donde se almacenarán los objetos Categoria obtenidos de la base de datos
            List<Categoria> lista = new List<Categoria>();

            // Instancia de la clase AccesoDatos para realizar la conexión y las consultas
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Preparamos la consulta SQL para obtener los registros de la tabla "Categorias"
                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdCategoria,Descripcion, Estado from CATEGORIA");                
                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                // Recorreremos el SqlDataReader, leyendo cada fila obtenida de la base de datos
                while (datos.Lector.Read())
                {
                    Categoria Categoria = new Categoria();
                    Categoria.IdCategoria = Convert.ToInt32(datos.Lector["IdCategoria"]);
                    Categoria.Descripcion = datos.Lector["Descripcion"].ToString();
                    Categoria.Estado = Convert.ToBoolean(datos.Lector["Estado"]);
                 



                    // Agregamos el objeto Categoria a la lista
                    lista.Add(Categoria);
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

            // Retornamos la lista de Categorias obtenidos
            return lista;
        }
        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idCategoriagenerado = 0;
            Mensaje = string.Empty;

            try
            {
                // Creamos una instancia de la clase AccesoDatos
                AccesoDatos datos = new AccesoDatos();

                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_RegistrarCategoria");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@Descripcion", obj.Descripcion);
                datos.setearParametro("@Estado", obj.Estado);
                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);  // No es necesario el tamaño porque es INT.
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);    // Aquí especificamos el tamaño para VARCHAR.


                // Ejecutamos la acción (comando)
                datos.ejecutarAccion();

                // Obtenemos los valores de los parámetros de salida
                idCategoriagenerado = Convert.ToInt32(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                // En caso de error, devolvemos el mensaje de la excepción
                idCategoriagenerado = 0;
                Mensaje = ex.Message;
            }

            return idCategoriagenerado;
        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_EditarCategoria");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@IdCategoria", obj.IdCategoria);
                datos.setearParametro("@Descripcion", obj.Descripcion);
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

        public bool Eliminar(Categoria obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_EliminarCategoria");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@IdCategoria", obj.IdCategoria);
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





    

