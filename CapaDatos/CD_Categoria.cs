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
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdCategoria,Descripcion, Estado from CATEGORIA");                
                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria Categoria = new Categoria();
                    Categoria.IdCategoria = Convert.ToInt32(datos.Lector["IdCategoria"]);
                    Categoria.Descripcion = datos.Lector["Descripcion"].ToString();
                    Categoria.Estado = Convert.ToBoolean(datos.Lector["Estado"]);

                    lista.Add(Categoria);
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
        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idCategoriagenerado = 0;
            Mensaje = string.Empty;

            try
            {
                AccesoDatos datos = new AccesoDatos();

                datos.setearProcedimiento("SP_RegistrarCategoria");

                datos.setearParametro("@Descripcion", obj.Descripcion);
                datos.setearParametro("@Estado", obj.Estado);
                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);  // No es necesario el tamaño porque es INT.
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);    // Aquí especificamos el tamaño para VARCHAR.

                datos.ejecutarAccion();

                idCategoriagenerado = Convert.ToInt32(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
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
             
                datos.setearProcedimiento("SP_EditarCategoria");
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
                datos.setearProcedimiento("SP_EliminarCategoria");

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





    

