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
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdCliente, Documento, NombreCompleto, Correo, Telefono, Estado from CLIENTE");


                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Cliente Cliente = new Cliente();
                    Cliente.IdCliente = Convert.ToInt32(datos.Lector["IdCliente"]);
                    Cliente.Documento = datos.Lector["Documento"].ToString();
                    Cliente.NombreCompleto = datos.Lector["NombreCompleto"].ToString();
                    Cliente.Correo = datos.Lector["Correo"].ToString();
                    Cliente.Telefono = datos.Lector["Telefono"].ToString();
                    Cliente.Estado = Convert.ToBoolean(datos.Lector["Estado"]);
                    lista.Add(Cliente);
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
        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idClientegenerado = 0;
            Mensaje = string.Empty;

            try
            {
                AccesoDatos datos = new AccesoDatos();

                datos.setearProcedimiento("sp_RegistrarCliente");

                datos.setearParametro("@Documento", obj.Documento);
                datos.setearParametro("@NombreCompleto", obj.NombreCompleto);
                datos.setearParametro("@Correo", obj.Correo);
                datos.setearParametro("@Telefono", obj.Telefono);
                datos.setearParametro("@Estado", obj.Estado);
                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);  // No es necesario el tamaño porque es INT.
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);    // Aquí especificamos el tamaño para VARCHAR.

                datos.ejecutarAccion();

                idClientegenerado = Convert.ToInt32(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
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
                datos.setearProcedimiento("sp_ModificarCliente");

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
            AccesoDatos datos = new AccesoDatos(); 

            try
            {
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
                datos.cerrarConexion();
            }

            return respuesta;
        }

    }
}