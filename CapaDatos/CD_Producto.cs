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

        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdProducto, Codigo, Nombre, p.Descripcion, c.IdCategoria, c.Descripcion[DescripcionCategoria],Stock, PrecioCompra,PrecioVenta, p.Estado from PRODUCTO p");
                query.AppendLine("inner join CATEGORIA c  on c.IdCategoria = p.IdCategoria");


                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

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
                    lista.Add(Producto);
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
        public int Registrar(Producto obj, out string Mensaje)
        {
            int idProductogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                AccesoDatos datos = new AccesoDatos();

                datos.setearProcedimiento("SP_RegistrarProducto");

                datos.setearParametro("@Codigo", obj.Codigo);
                datos.setearParametro("@Nombre", obj.Nombre);
                datos.setearParametro("@Descripcion", obj.Descripcion);
                datos.setearParametro("@IdCategoria", obj.oCategoria.IdCategoria);
                datos.setearParametro("@Estado", obj.Estado);

                datos.setearParametro("@Resultado", SqlDbType.Int, ParameterDirection.Output);  
                datos.setearParametro("@Mensaje", SqlDbType.VarChar, ParameterDirection.Output, 500);    


                datos.ejecutarAccion();

                idProductogenerado = Convert.ToInt32(datos.ObtenerParametro("@Resultado").Value);
                Mensaje = datos.ObtenerParametro("@Mensaje").Value.ToString();
            }
            catch (Exception ex)
            {
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
                datos.setearProcedimiento("SP_ModificarProducto");

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
                datos.setearProcedimiento("SP_EliminarProducto");

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