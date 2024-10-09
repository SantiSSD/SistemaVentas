﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using System.Reflection;
namespace CapaDatos
{
    public class CD_Usuario
    {
           
        // Método para obtener una lista de usuarios desde la base de datos
        public List<Usuario> Listar()
        {
            // Lista donde se almacenarán los objetos Usuario obtenidos de la base de datos
            List<Usuario> lista = new List<Usuario>();

            // Instancia de la clase AccesoDatos para realizar la conexión y las consultas
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Preparamos la consulta SQL para obtener los registros de la tabla "Usuarios"
                StringBuilder query = new StringBuilder () ;
                query.AppendLine("SELECT u.IdUsuario,  u.Documento, u.NombreCompleto, u.Correo, u.Clave, u.Estado, r.IdRol, r.Descripcion FROM usuario u");
                query.AppendLine("inner join rol r on r.IdRol = u.IdRol");
                           

                datos.setearConsulta(query.ToString());
                datos.ejecutarLectura();

                // Recorreremos el SqlDataReader, leyendo cada fila obtenida de la base de datos
                while (datos.Lector.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.IdUsuario = Convert.ToInt32(datos.Lector["IdUsuario"]);
                    usuario.Documento = datos.Lector["Documento"].ToString();
                    usuario.NombreCompleto = datos.Lector["NombreCompleto"].ToString();
                    usuario.Correo = datos.Lector["Correo"].ToString();
                    usuario.Clave = datos.Lector["Clave"].ToString();
                    usuario.Estado = Convert.ToBoolean(datos.Lector["Estado"]);
                    usuario.oRol = new Rol() { IdRol = Convert.ToInt32(datos.Lector["IdRol"]) , Descripcion = datos.Lector["Descripcion"].ToString() };



                    // Agregamos el objeto Usuario a la lista
                    lista.Add(usuario);
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

            // Retornamos la lista de usuarios obtenidos
            return lista;
        }
    }
}




    
