using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {

        // Método para obtener una lista de Clientes desde la base de datos
        public List<Cliente> Listar()
        {
            // Lista donde se almacenarán los objetos Cliente obtenidos de la base de datos
            List<Cliente> lista = new List<Cliente>();

            // Instancia de la clase AccesoDatos para realizar la conexión y las consultas
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Preparamos la consulta SQL para obtener los registros de la tabla "Clientes"
                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdCliente, Documento, NombreCompleto, Correo, Telefono, Estado from CLIENTE");


                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                // Recorreremos el SqlDataReader, leyendo cada fila obtenida de la base de datos
                while (datos.Lector.Read())
                {
                    Cliente Cliente = new Cliente();
                    Cliente.IdCliente = Convert.ToInt32(datos.Lector["IdCliente"]);
                    Cliente.Documento = datos.Lector["Documento"].ToString();
                    Cliente.NombreCompleto = datos.Lector["NombreCompleto"].ToString();
                    Cliente.Correo = datos.Lector["Correo"].ToString();
                    Cliente.Telefono = datos.Lector["Telefono"].ToString();
                    Cliente.Estado = Convert.ToBoolean(datos.Lector["Estado"]);
                    // Agregamos el objeto Cliente a la lista
                    lista.Add(Cliente);
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

            // Retornamos la lista de Clientes obtenidos
            return lista;
        }
        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idClientegenerado = 0;
            Mensaje = string.Empty;

            try
            {
                // Creamos una instancia de la clase AccesoDatos
                AccesoDatos datos = new AccesoDatos();

                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("sp_RegistrarCliente");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@NombreCompleto", obj.NombreCompleto);
                datos.setearParametro("@Correo", obj.Correo);
                datos.setearParametro("@Telefono", obj.Telefono);
                datos.setearParametro("@Estado", obj.Estado);

                // Los parámetros de salida del procedimiento almacenado
                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);  // No es necesario el tamaño porque es INT.
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);    // Aquí especificamos el tamaño para VARCHAR.


                // Ejecutamos la acción (comando)
                datos.ejecutarAccion();

                // Obtenemos los valores de los parámetros de salida
                idClientegenerado = Convert.ToInt32(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
                // En caso de error, devolvemos el mensaje de la excepción
                idClientegenerado = 0;
                Mensaje = ex.Message;
            }

            return idClientegenerado;
        }

        public bool Editar(Cliente obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try

            {
                AccesoDatos datos = new AccesoDatos();
                // Configuramos que vamos a usar un procedimiento almacenado
                datos.setearProcedimiento("sp_ModificarCliente");

                // Establecemos los parámetros necesarios para el procedimiento almacenado
                datos.setearParametro("@IdCliente", obj.IdCliente);
                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@NombreCompleto", obj.NombreCompleto);
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

        public bool Eliminar(Cliente obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos(); // Declaración de datos antes del try

            try
            {
                // Configuramos la consulta directa para eliminar un cliente por su ID
                string consulta = "DELETE FROM cliente WHERE IdCliente = @id";
                datos.setearConsulta(consulta);
                datos.setearParametro("@id",obj.IdCliente);

                datos.ejecutarAccion();
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            finally
            {
                datos.cerrarConexion(); // Cierra la conexión en cualquier caso
            }

            return respuesta;
        }

    }
}