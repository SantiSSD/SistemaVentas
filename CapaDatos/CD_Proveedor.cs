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
           // Método para obtener una lista de Proveedors desde la base de datos
            public List<Proveedor> Listar()
            {
                // Lista donde se almacenarán los objetos Proveedor obtenidos de la base de datos
                List<Proveedor> lista = new List<Proveedor>();

                // Instancia de la clase AccesoDatos para realizar la conexión y las consultas
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    // Preparamos la consulta SQL para obtener los registros de la tabla "Proveedors"
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select IdProveedor, Documento, RazonSocial, Correo, Telefono, Estado from PROVEEDOR");

                    datos.setearConsulta(query.ToString());
                    datos.ejecutarLectura();

                    // Recorreremos el SqlDataReader, leyendo cada fila obtenida de la base de datos
                    while (datos.Lector.Read())
                    {
                        Proveedor Proveedor = new Proveedor();
                        Proveedor.IdProveedor = Convert.ToInt32(datos.Lector["IdProveedor"]);
                        Proveedor.Documento = datos.Lector["Documento"].ToString();
                        Proveedor.RazonSocial = datos.Lector["RazonSocial"].ToString();
                        Proveedor.Correo = datos.Lector["Correo"].ToString();
                        Proveedor.Telefono = datos.Lector["Telefono"].ToString();
                        Proveedor.Estado = Convert.ToBoolean(datos.Lector["Estado"]);



                        // Agregamos el objeto Proveedor a la lista
                        lista.Add(Proveedor);
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

                // Retornamos la lista de Proveedors obtenidos
                return lista;
            }
            public int Registrar(Proveedor obj, out string Mensaje)
            {
                int idProveedorgenerado = 0;
                Mensaje = string.Empty;

                try
                {
                    // Creamos una instancia de la clase AccesoDatos
                    AccesoDatos datos = new AccesoDatos();

                    // Configuramos que vamos a usar un procedimiento almacenado
                    datos.setearProcedimiento("sp_RegistrarProveedor");

                    // Establecemos los parámetros necesarios para el procedimiento almacenado
                    datos.setearParametro("@Documento", obj.Documento);
                    datos.setearParametro("@RazonSocial", obj.RazonSocial);
                    datos.setearParametro("@Correo", obj.Correo);
                    datos.setearParametro("@Telefono", obj.Telefono); // Asegúrate de pasar el ID del rol
                    datos.setearParametro("@Estado", obj.Estado);

                    // Los parámetros de salida del procedimiento almacenado
                    datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);  // No es necesario el tamaño porque es INT.
                    datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);    // Aquí especificamos el tamaño para VARCHAR.


                    // Ejecutamos la acción (comando)
                    datos.ejecutarAccion();

                    // Obtenemos los valores de los parámetros de salida
                    idProveedorgenerado = Convert.ToInt32(datos.ObtenerParametro("@Resultado").Value);
                    Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
                }
                catch (Exception ex)
                {
                    // En caso de error, devolvemos el mensaje de la excepción
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
                    // Configuramos que vamos a usar un procedimiento almacenado
                    datos.setearProcedimiento("sp_ModificarProveedor");

                    // Establecemos los parámetros necesarios para el procedimiento almacenado
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
                    // Configuramos que vamos a usar un procedimiento almacenado
                    datos.setearProcedimiento("sp_EliminarProveedor");

                    // Establecemos los parámetros necesarios para el procedimiento almacenado
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
