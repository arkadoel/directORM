/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 07/26/2013
 * Hora: 19:03
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.IO;

namespace DirectORM
{
	/// <summary>
	/// Description of Logica.
	/// </summary>
	public class Logica
	{
		public List<Table> Tablas{get; set;}
		public string NAMESPACE ="";
		public VentanaResultado ventana;
        public String MotorDB { get; set; }

        public static class MOTORES
        {
            public const String SQL_CLIENT = @"Motores\SQLClient.txt";
            public const String OLE_DB = @"Motores\OleDB.txt";
        }
		
		public Logica(String motor)
		{
			Tablas =  new List<Table>();
            MotorDB = motor;
		}
		
		public void procesar(){
			string txt ="";
            txt += "using System;\r\n" +
                "using System.Collections.Generic;\r\n" +
                "using System.Linq;\r\n" +
                "using System.Text;\r\n";
            switch(MotorDB)
            {
                case MOTORES.OLE_DB:
                    txt += "using System.Data.OleDb;\r\n";
                    break;
                case MOTORES.SQL_CLIENT:
                    txt += "using System.Data.SqlClient;\r\n";
                    break;
            }
                
		    txt += "using System.Data;\r\n\r\n";
            
			txt +="namespace " + NAMESPACE + "\r\n{\r\n";
			
			txt += generarEntidadesObjeto();
			txt +="\r\n\r\n";
			txt += generarObjetosTabla();
			txt +="\r\n\r\n";
			txt += cargarGestorDB();
			txt +="\r\n\r\n";
			
			txt +="}\r\n\r\n"; //final namespace
			
			ventana.txtResultado.Text = txt;
		}
		
		#region "Otros metodos"
		
		private string ponerFormato(TableColumn col){
			string txt="";
			switch(col.ValueType){
				case "varchar":
					txt+="'@" + col.Name + "'";
					break;
				case "datetime":
					txt+="#@" + col.Name + "#";
					break;
				default: 
					txt+="@" + col.Name + "";
					break;
			}	
			return txt;
		}
		
		/// <summary>
		/// Pone en plural la palabra pasada como parametro
		/// </summary>
		/// <param name="palabra"></param>
		/// <returns></returns>
		private string pluralizar(string palabra){
			char terminacion = palabra[palabra.Length-1];
			
			if(terminacion=='a' || terminacion=='e' || terminacion=='i' || terminacion=='o' || terminacion=='u'){
				return palabra + "s";
			}
			else{
				return palabra + "es";
			}
			
		}
		
		/// <summary>
		/// Pone en mayuscula la primera letra de la palabra
		/// </summary>
		/// <param name="palabra"></param>
		/// <returns></returns>
		private string Capitalizar(string palabra){
			string letra = palabra[0].ToString().ToUpper();			
			palabra = letra + palabra.Substring(1,palabra.Length-1);
			return palabra;
		}
		
		private string cargarGestorDB()
		{
			string texto = "";
			StreamReader fich = new StreamReader(MotorDB);
			texto = fich.ReadToEnd();			
			fich.Close();
			
			return texto;
		}
		#endregion
		
		
		#region "Generador de entidades-Objeto"
		private string generarEntidadesObjeto(){
			string txt="";
				
			txt +="\t#region \"ENTIDADES-OBJETO\"\r\n";
			foreach(Table tb in Tablas)
			{
			TableColumn col = null;
			
						
			txt +="\tpublic class " + tb.TableName +"\r\n\t{";
			
			//poner propiedades
			for(int i=0; i< tb.Cols.Count(); i++)
			{
				col = tb.Cols[i];
				txt +="\r\n\t\tpublic ";
				switch(col.ValueType)
				{
					case "varchar":
						txt+="String ";
						break;
					case "int":
                        txt += "Nullable<Int32> ";                        
						break;
                    case "float":
                        txt += "Nullable<float> ";
                        break;
					case "datetime":
						txt+="DateTime ";
						break;
				}
				txt+= Capitalizar(col.Name) + "{ get; set; }";
			}
			
			txt +="\r\n";
			
			txt +="\t}\r\n"; //final de clase
			}
			txt +="\t#endregion\r\n";
			return txt;
		}
		#endregion
		
		
		#region "Gestion de tablas"
		/// <summary>
		/// Genera los objetos tabla que enlazaran DB y objetos-entidad
		/// </summary>
		/// <returns></returns>
		private string generarObjetosTabla(){
			string txt ="";
						
			txt += "\t#region \"TABLAS-OBJETO\"\r\n";
            txt += "\tnamespace Tablas{\r\n";
			foreach(Table tb in Tablas)
			{
			string nombreTabla = pluralizar(tb.TableName);
			TableColumn col = null;
			
						
			txt +="\tpublic class " + nombreTabla +"\r\n\t{\r\n";
			
			
			//SENTENCIA INSERT
			{
			txt +="\t\tprivate const String INSERT = \"INSERT INTO " + tb.TableName +" (";
			for(int i=0; i< tb.Cols.Count(); i++){
				col = tb.Cols[i];
				if(col.AutoIncrement==false){
					txt+=col.Name;
					if((i+1)<tb.Cols.Count() == true){
						txt+=",";
					}
				}
			}
			txt +=") VALUES (";
			for(int i=0; i< tb.Cols.Count(); i++){
				col = tb.Cols[i];
				if(col.AutoIncrement==false){
					
					txt+=ponerFormato(col);
					
					if((i+1)<tb.Cols.Count() == true) txt+=","; 
				}
			}
			txt +=")\";\r\n";
			
			//delete
			txt +="\t\tprivate const String DELETE = \"DELETE FROM " + tb.TableName +" WHERE ";
			List<TableColumn> idCols = (from u in tb.Cols
				where u.isKey == true
				select u).ToList();
			
			for(int i=0; i< idCols.Count(); i++){
				col = idCols[i];
				
				
				txt+=col.Name + " = " + ponerFormato(col);
				
				if((i+1)<idCols.Count() == true){
					txt+=" and ";
				}
			}
			txt +="\";\r\n";
		}
			
			//update (no actualiza los campos id)
			{
				txt +="\t\tprivate const String UPDATE = \"UPDATE " + tb.TableName +" SET ";
				
				List<TableColumn> notIdCols = (from u in tb.Cols
					where u.isKey == false
					select u).ToList();
				
				for(int i=0; i< notIdCols.Count(); i++){
					col = notIdCols[i];
					
					txt+=col.Name + " = " + ponerFormato(col);
					
					if((i+1)<notIdCols.Count() == true){
						txt+=", ";
					}
				}
				
				txt +=" WHERE ";
				List<TableColumn> idCols = (from u in tb.Cols
					where u.isKey == true
					select u).ToList();
				
				for(int i=0; i< idCols.Count(); i++){
					col = idCols[i];
					txt+=col.Name + " = " + ponerFormato(col);
					if((i+1)<idCols.Count() == true){
						txt+=", ";
					}
				}
				txt +="\";\r\n";
			}
			
			//select
			txt +="\t\tprivate const String SELECT = \"SELECT * FROM " + tb.TableName +"\";\r\n\r\n";
			
			txt+="\t\tprivate static List<"+ tb.TableName +"> _lista = null;\r\n\r\n";

            //ToList()
            txt += "\t\tpublic static List<" + tb.TableName + "> ToList()\r\n\t\t{\r\n";
            txt += @"
                    return ToList("");
                    }
                    \r\n";
			
			txt +="\t\tpublic static List<"+ tb.TableName +"> ToList(string filtro)\r\n\t\t{\r\n";
        
			txt+=@"
                if (_lista == null)
                {
                    _lista = new List<"+ tb.TableName + @">();
                }

                if (string.IsNullOrWhiteSpace(filtro))
                {
                    _lista = mapeoObjeto(GestorDB.Consulta(SELECT));
                }
                else
                {
                    _lista = mapeoObjeto(GestorDB.Consulta(SELECT + " +
                                                                      "\" where \"" +
                                                                      @" + filtro));
                }

                return _lista;
	
		    ";
			txt+="\r\n\t\t}\r\n\r\n";//final metodo Filas
			
			txt +=@"
	        public static int Add(" + tb.TableName + @" _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(" + tb.TableName + @" _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(" + tb.TableName + @" _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, " + tb.TableName + @" _conf)
	        {
	            String s = _sql;";
				string campo ="";
				foreach(TableColumn col1 in tb.Cols){
					campo="_conf." + Capitalizar(col1.Name);
					if(col1.ValueType =="datetime"){
						
						campo = "(_conf." + Capitalizar(col1.Name) + ".Year + \"/\" + _conf."+ Capitalizar(col1.Name) + ".Month + \"/\" + _conf."+ Capitalizar(col1.Name) + ".Day)";
					}
                    if (col1.ValueType.ToLower() == "varchar")
                    {
                        txt += "\r\n\t\t\t\ts = s.Replace(\"@" + col1.Name + "\", " + campo + ");";
                    }
                    else
                    {
                        txt += "\r\n\t\t\t\ts = s.Replace(\"@" + col1.Name + "\", " + campo + ".ToString());";
                    }
				}
			
			txt+=@"
	            return s;
	        }
	
	        private static List<" + tb.TableName + @"> mapeoObjeto(DataTable dt)
	        {
	            List<" + tb.TableName + @"> resp = new List<" + tb.TableName + @">();
	            foreach (DataRow fila in dt.Rows)
	            {";
			
	        txt+="\r\n\t\t\t" + tb.TableName + " conf = new " + tb.TableName + "();";
	        foreach(TableColumn col2 in tb.Cols)
	        {
	        	//conf.Id = fila.Field<Int32>("ID");
	        	txt+="\r\n\t\t\t\tconf." + Capitalizar(col2.Name) + " = fila.Field<";
	        	switch(col2.ValueType)
				{
					case "varchar":
						txt+="String";
						break;
					case "int":
                        txt += "Nullable<Int32>";
						break;
                    case "float":
                        txt += "Nullable<float>";
                        break;
					case "datetime":
						txt+="DateTime";
						break;
				}
	        	
	        	txt+=">(\"" + col2.Name + "\");";
	        }
	        
			
	        txt+=@"
	                resp.Add(conf);
	            }
	            return resp;
	        }

			";
			
			
			
			txt +="\r\n\t}\r\n"; //final de clase
			}
            txt += "\t}\r\n"; // final namespace Tablas
			txt +="\t#endregion\r\n";
			return txt;
		}
		#endregion
	}
}
