using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace Pruebas
{
	#region "ENTIDADES-OBJETO"
	public class Nacimiento
	{
		Nullable<int> id;
		
		public Nullable<int> Id {
			get { return id; }
			set { id = value; }
		}
		public DateTime Fecha{ get; set; }
		public String Campo{ get; set; }
		public Int32 Numero{ get; set; }
	}
	#endregion


	#region "TABLAS-OBJETO"
	public class Nacimientos
	{
		private const String INSERT = "INSERT INTO Nacimiento (fecha,campo,numero) VALUES (#@fecha#,'@campo',@numero)";
		private const String DELETE = "DELETE FROM Nacimiento WHERE id = @id";
		private const String UPDATE = "UPDATE Nacimiento SET fecha = #@fecha#, campo = '@campo', numero = @numero WHERE id = @id";
		private const String SELECT = "SELECT * FROM Nacimiento";

		private static List<Nacimiento> _lista = null;

		public static List<Nacimiento> toList()
		{
	                if (_lista == null)
	                {
	                    _lista = new List<Nacimiento>();
	                }
	
	                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));
	
	                return _lista;
		}


	        public static int Add(Nacimiento _conf)
	        {
	        	_conf.Id = null;
	        	
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(Nacimiento _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(Nacimiento _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, Nacimiento _conf)
	        {
	        	
	            String s = _sql;
				s = s.Replace("@id", _conf.Id.ToString());
				s = s.Replace("@fecha", (_conf.Fecha.Year + "/" + _conf.Fecha.Month + "/" + _conf.Fecha.Day).ToString());
				s = s.Replace("@campo", _conf.Campo.ToString());
				s = s.Replace("@numero", _conf.Numero.ToString());
	            return s;
	        }
	
	        private static List<Nacimiento> mapeoObjeto(DataTable dt)
	        {
	            List<Nacimiento> resp = new List<Nacimiento>();
	            foreach (DataRow fila in dt.Rows)
	            {
					Nacimiento conf = new Nacimiento();
					conf.Id = fila.Field<Int32>("id");
					conf.Fecha = fila.Field<DateTime>("fecha");
					conf.Campo = fila.Field<String>("campo");
					conf.Numero = fila.Field<Int32>("numero");
	                resp.Add(conf);
	            }
	            return resp;
	        }

			
	}
	#endregion




	#region "GESTOR DE LA BASE DE DATOS"
    public class GestorDB
    {
        /***
         * Se hace mediante LINQ to Dataset debido a que se quiere hacer lo
         * mas portable posible. No todos los equipos disponen de SQL Server
         * instalado, pero las librerias para el acceso a access vienen por 
         * defecto con el .NET framework. 
         */
        public static String CADENA_CONEXION = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\DB\ISISDB.mdb;User Id=admin;Password=;";

        private static OleDbConnection _conexion = null;

        /// <summary>
        /// Conexion automatica con la base de datos
        /// </summary>
        public static OleDbConnection Conexion 
        {
            get 
            {
                if (_conexion == null)
                {
                    _conexion = new OleDbConnection(CADENA_CONEXION);
                }

                if (_conexion.State != ConnectionState.Open)
                {
                    _conexion.Open();
                }
                return _conexion;
            }
            set
            {
                _conexion = value;
            }
        }
        
        /// <summary>
        /// Finalizar la conexion existente si se puede
        /// </summary>
        /// <returns>Devuelve un string con 'OK' o con la descripcion
        /// del error resultante
        /// </returns>
        public static String FinConexion()
        {
            try
            {
                if (Conexion != null)
                {
                    if (Conexion.State == ConnectionState.Open)
                    {
                        Conexion.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// Permite ejecutar sentencias SQL SELECT contra la base de datos,
        /// devuelve un dataset con los datos seleccionados.
        /// </summary>
        /// <param name="_sql">Sentencia select</param>
        /// <param name="_nombreTabla">Nombre de la tabla resultado</param>
        /// <returns></returns>
        public static DataTable Consulta(String _sql)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(_sql, GestorDB.Conexion);
            da.Fill(ds, "t");
            GestorDB.FinConexion();
            return ds.Tables["t"];
        }

        /// <summary>
        /// Ejecuta sentencias insert, delete o update. Retorna el numero
        /// de filas afectadas o -1 si no se modifico nada.
        /// </summary>
        /// <param name="_sql"></param>
        /// <returns></returns>
        public static int Ejecuta(String _sql)
        {
            int resultado = -1;
            OleDbCommand comando = new OleDbCommand(_sql, GestorDB.Conexion);
            resultado = comando.ExecuteNonQuery();
            GestorDB.FinConexion();
            return resultado;
        }


    }

	#endregion


}

