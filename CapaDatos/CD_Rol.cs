using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Rol
    {
        public List<Rol> Listar()
        {
            List<Rol> lista = new List<Rol>();

            AccesoDatos datos = new AccesoDatos();

            try
            {

                StringBuilder query = new StringBuilder();
                query.AppendLine("select IdRol, Descripcion from ROL");
              

                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Rol rol = new Rol
                    {
                        IdRol = Convert.ToInt32(datos.Lector["IdRol"]),
                        Descripcion = datos.Lector["Descripcion"].ToString(),
                    };

                    lista.Add(rol);
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
