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
    public class CD_Reporte
    {
        public List<ReporteCompra> Compra(string fechainicio, string fechafin, int idproveedor)
        {
            List<ReporteCompra> lista = new List<ReporteCompra>();

            AccesoDatos datos = new AccesoDatos();
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    datos.setearProcedimiento("sp_ReporteCompras");
                    datos.setearParametro("fechainicio", fechainicio);
                    datos.setearParametro("fechafin", fechafin);
                    datos.setearParametro("idproveedor", idproveedor);

                    datos.ejecutarLectura();
                    {
                        while (datos.Lector.Read())
                        {

                            lista.Add(new ReporteCompra()
                            {
                                FechaRegistro = datos.Lector["FechaRegistro"].ToString(),
                                TipoDocumento = datos.Lector["TipoDocumento"].ToString(),
                                NumeroDocumento = datos.Lector["NumeroDocumento"].ToString(),
                                MontoTotal = datos.Lector["MontoTotal"].ToString(),
                                UsuarioRegistro = datos.Lector["UsuarioRegistro"].ToString(),
                                DocumentoProveedor = datos.Lector["DocumentoProveedor"].ToString(),
                                RazonSocial = datos.Lector["RazonSocial"].ToString(),
                                CodigoProducto = datos.Lector["CodigoProducto"].ToString(),
                                NombreProducto = datos.Lector["NombreProducto"].ToString(),
                                Categoria = datos.Lector["Categoria"].ToString(),
                                PrecioCompra = datos.Lector["PrecioCompra"].ToString(),
                                PrecioVenta = datos.Lector["PrecioVenta"].ToString(),
                                Cantidad = datos.Lector["Cantidad"].ToString(),
                                SubTotal = datos.Lector["SubTotal"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {

                    lista = new List<ReporteCompra>();
                }
            }

            return lista;

        }

        public List<ReporteVenta> Venta(string fechainicio, string fechafin)
        {
            List<ReporteVenta> lista = new List<ReporteVenta>();

            AccesoDatos datos = new AccesoDatos();
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    datos.setearProcedimiento("sp_ReporteVentas");
                    datos.setearParametro("fechainicio", fechainicio);
                    datos.setearParametro("fechafin", fechafin);

                    datos.ejecutarLectura();


                    {
                        while (datos.Lector.Read())
                        {
                            lista.Add(new ReporteVenta()
                            {
                                FechaRegistro = datos.Lector["FechaRegistro"].ToString(),
                                TipoDocumento = datos.Lector["TipoDocumento"].ToString(),
                                NumeroDocumento = datos.Lector["NumeroDocumento"].ToString(),
                                MontoTotal = datos.Lector["MontoTotal"].ToString(),
                                UsuarioRegistro = datos.Lector["UsuarioRegistro"].ToString(),
                                DocumentoCliente = datos.Lector["DocumentoCliente"].ToString(),
                                NombreCliente = datos.Lector["NombreCliente"].ToString(),
                                CodigoProducto = datos.Lector["CodigoProducto"].ToString(),
                                NombreProducto = datos.Lector["NombreProducto"].ToString(),
                                Categoria = datos.Lector["Categoria"].ToString(),
                                PrecioVenta = datos.Lector["PrecioVenta"].ToString(),
                                Cantidad = datos.Lector["Cantidad"].ToString(),
                                SubTotal = datos.Lector["SubTotal"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ReporteVenta>();
                }
            }

            return lista;

        }
    }
}