using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
namespace CapaDatos
{
    public class CD_Permiso
    {
        public List<Permiso> Listar(int idusuario)
        {
            List<Permiso> lista = new List<Permiso>();

            AccesoDatos datos = new AccesoDatos();

            try
            {

                StringBuilder query = new StringBuilder();
                query.AppendLine("select p.IdRol, p.NombreMenu from PERMISO p");
                query.AppendLine("inner join ROL r on r.IdRol = p.IdRol");
                query.AppendLine("inner join USUARIO u on u.IdRol = r.IdRol");
                query.AppendLine("where u.IdUsuario = @idusuario");

                datos.setearConsulta(query.ToString());

                datos.setearParametro("@idusuario", idusuario);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Permiso permiso = new Permiso
                    {
                        oRol = new Rol() {IdRol =Convert.ToInt32(datos.Lector["IdRol"]) },
                        NombreMenu = datos.Lector["NombreMenu"].ToString()
                    };

                    lista.Add(permiso);
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
    }
}

