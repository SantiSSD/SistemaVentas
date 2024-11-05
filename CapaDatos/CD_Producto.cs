using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Producto
    {

        // Método para obtener una lista de Productos desde la base de datos
        public List<Producto> Listar()
        {
            // Lista donde se almacenarán los objetos Producto obtenidos de la base de datos
            List<Producto> lista = new List<Producto>();

            // Instancia de la clase AccesoDatos para realizar la conexión y las consultas
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Preparamos la consulta SQL para obtener los registros de la tabla "Productos"
                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdProducto, Codigo, Nombre, p.Descripcion, c.IdCategoria, c.Descripcion[DescripcionCategoria],Stock, PrecioCompra,PrecioVenta, p.Estado from PRODUCTO p");
                query.AppendLine("inner join CATEGORIA c  on c.IdCategoria = p.IdCategoria");


                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                // Recorreremos el SqlDataReader, leyendo cada fila obtenida de la base de datos
                while (datos.Lector.Read())
                {
                    Producto Producto = new Producto();
                    Producto.IdProducto = Convert.ToInt32(datos.Lector["IdProducto"]);
                    Producto.Codigo = datos.Lector["Codigo"].ToString();
                    Producto.Nombre = datos.Lector["Nombre"].ToString();
                    Producto.Descripcion = datos.Lector["Descripcion"].ToString();
                    Producto.oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(datos.Lector["IdProducto"]), Descripcion = datos.Lector["DescripcionCategoria"].ToString(), };
                    Producto.Stock = Convert.ToInt32 (datos.Lector["Stock"].ToString());
                    Producto.PrecioCompra = Convert.ToDecimal(datos.Lector["PrecioCompra"].ToString());
                    Producto.PrecioVenta = Convert.ToDecimal(datos.Lector["PrecioVenta"].ToString());
                    Producto.Estado = Convert.ToBoolean(datos.Lector["Estado"]);



                    // Agregamos el objeto Producto a la lista
                    lista.Add(Producto);
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

            // Retornamos la lista de Productos obtenidos
            return lista;
        }
        public int Registrar(Producto obj, out string Mensaje)
        {
            int idProductogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                // Creamos una instancia de la clase AccesoDatos
                AccesoDatos datos = new AccesoDatos();

                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_RegistrarProducto");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@Codigo", obj.Codigo);
                datos.setearParametro("@Nombre", obj.Nombre);
                datos.setearParametro("@Descripcion", obj.Descripcion);
                datos.setearParametro("@IdCategoria", obj.oCategoria.IdCategoria);
                datos.setearParametro("@Estado", obj.Estado);

                // Los parámetros de salida del procedimiento almacenado
                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);  // No es necesario el tamaño porque es INT.
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);    // Aquí especificamos el tamaño para VARCHAR.


                // Ejecutamos la acción (comando)
                datos.ejecutarAccion();

                // Obtenemos los valores de los parámetros de salida
                idProductogenerado = Convert.ToInt32(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                // En caso de error, devolvemos el mensaje de la excepción
                idProductogenerado = 0;
                Mensaje = ex.Message;
            }

            return idProductogenerado;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_ModificarProducto");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@IdProducto", obj.IdProducto);
                datos.setearParametro("@Codigo", obj.Codigo);
                datos.setearParametro("@Nombre", obj.Nombre);
                datos.setearParametro("@Descripcion", obj.Descripcion);
                datos.setearParametro("@IdCategoria", obj.oCategoria.IdCategoria);  
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

        public bool Eliminar(Producto obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("SP_EliminarProducto");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@IdProducto", obj.IdProducto);
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