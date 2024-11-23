using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=DBSISTEMA_VENTA; integrated security=true");
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void setearProcedimiento(string sp)
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = sp;
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ejecutarAccionConResultado()
        {
            comando.Connection = conexion;
            try
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }
                return comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la acción en la base de datos.", ex);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }
        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ejecutarEscalar()
        {
            object valor = null;
            comando.Connection = conexion; // Asegúrate de asociar la conexión al comando.

            try
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open(); // Abre la conexión si no está abierta.
                }
                valor = comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la consulta escalar: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close(); // Cierra la conexión después de ejecutar.
                }
            }
            return valor;
        }

        public void setearParametroBinario(string nombre, byte[] valor)
        {
            if (comando != null)
            {
                comando.Parameters.Add(nombre, SqlDbType.VarBinary).Value = valor;
            }
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void setearParametro(string nombre, SqlDbType tipo, ParameterDirection direccion = ParameterDirection.Input, int tamaño = 0)
        {
            SqlParameter parametro = new SqlParameter(nombre, tipo);
            parametro.Direction = direccion;

            if (tipo == SqlDbType.VarChar && tamaño > 0)
            {
                parametro.Size = tamaño;
            }

            comando.Parameters.Add(parametro);
        }
        public void setearParametro(string nombre, SqlDbType tipo, byte[] valor, ParameterDirection direccion = ParameterDirection.Input)
        {
            SqlParameter parametro = new SqlParameter(nombre, tipo);
            parametro.Direction = direccion;
            parametro.Value = valor;

            comando.Parameters.Add(parametro);
        }
        public SqlParameter ObtenerParametro(string nombre)
        {
            return comando.Parameters[nombre];
        }

        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }
    }
}



