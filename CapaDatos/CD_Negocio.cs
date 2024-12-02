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
    public class CD_Negocio
    {
       
            private AccesoDatos accesoDatos;

            public CD_Negocio()
            {
                accesoDatos = new AccesoDatos();
            }

            public Negocio ObtenerDatos()
            {
                Negocio obj = new Negocio();

                try
                {
                    string query = "select IdNegocio, Nombre, RUC, Direccion from NEGOCIO where IdNegocio = 1";
                    accesoDatos.setearConsulta(query);
                    accesoDatos.ejecutarLectura();

                    if (accesoDatos.Lector.Read())
                    {
                        obj = new Negocio()
                        {
                            IdNegocio = int.Parse(accesoDatos.Lector["IdNegocio"].ToString()),
                            Nombre = accesoDatos.Lector["Nombre"].ToString(),
                            RUC = accesoDatos.Lector["RUC"].ToString(),
                            Direccion = accesoDatos.Lector["Direccion"].ToString()
                        };
                    }
                }
                catch (Exception ex)
                {
                    obj = new Negocio();
                }
                finally
                {
                    accesoDatos.cerrarConexion();
                }

                return obj;
            }

            public bool GuardarDatos(Negocio objeto, out string mensaje)
            {
                mensaje = string.Empty;
                bool respuesta = true;

                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO set Nombre = @nombre,");
                    query.AppendLine("RUC = @ruc,");
                    query.AppendLine("Direccion = @direccion");
                    query.AppendLine("where IdNegocio = 1;");

                    accesoDatos.setearConsulta(query.ToString());
                    accesoDatos.setearParametro("@nombre", objeto.Nombre);
                    accesoDatos.setearParametro("@ruc", objeto.RUC);
                    accesoDatos.setearParametro("@direccion", objeto.Direccion);

                    if (accesoDatos.ejecutarAccionConResultado() < 1)
                    {
                        mensaje = "No se pudo guardar los datos";
                        respuesta = false;
                    }
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    respuesta = false;
                }

                return respuesta;
            }

            public byte[] ObtenerLogo(out bool obtenido)
            {
                obtenido = true;
                byte[] LogoBytes = new byte[0];

                try
                {
                    string query = "select Logo from NEGOCIO where IdNegocio = 1";
                    accesoDatos.setearConsulta(query);
                    accesoDatos.ejecutarLectura();

                    if (accesoDatos.Lector.Read())
                    {
                        LogoBytes = (byte[])accesoDatos.Lector["Logo"];
                    }
                }
                catch (Exception ex)
                {
                    obtenido = false;
                    LogoBytes = new byte[0];
                }
                finally
                {
                    accesoDatos.cerrarConexion();
                }

                return LogoBytes;
            }

        public bool ActualizarLogo(byte[] image, out string mensaje)
        {
            mensaje = string.Empty;
            bool respuesta = true;

            try
            {
                if (image == null || image.Length == 0)
                {
                    mensaje = "La imagen no puede estar vacía.";
                    return false;
                }

                string query = "UPDATE NEGOCIO SET Logo = @imagen WHERE IdNegocio = 1;";
                accesoDatos.setearConsulta(query);
                accesoDatos.setearParametroBinario("@imagen", image);

                int filasAfectadas = accesoDatos.ejecutarAccionConResultado();
                if (filasAfectadas < 1)
                {
                    mensaje = "No se pudo actualizar el logo.";
                    respuesta = false;
                }
                else
                {
                    mensaje = "Logo actualizado correctamente.";
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error al actualizar el logo: {ex.Message}";
                respuesta = false;
            }

            return respuesta;
        }
    }
    }
