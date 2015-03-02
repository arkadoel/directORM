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
		public List<Tabla> Tablas{get; set;}
		public string NAMESPACE ="";
		public VentanaResultado ventana;
		
		public Logica()
		{
			Tablas =  new List<Tabla>();
		}
		
		public void procesar(){
			string txt ="";
			txt +="using System;\r\n" +
				"using System.Collections.Generic;\r\n" +
				"using System.Linq;\r\n" +
				"using System.Text;\r\n" +
				"using System.Data.OleDb;\r\n" +
				"using System.Data;\r\n\r\n";
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
		
		private string ponerFormato(ColumaTabla col){
			string txt="";
			switch(col.TipoDato){
				case "varchar":
					txt+="'@" + col.Nombre + "'";
					break;
				case "datetime":
					txt+="#@" + col.Nombre + "#";
					break;
				default: 
					txt+="@" + col.Nombre + "";
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
			StreamReader fich = new StreamReader("DB.txt");
			texto = fich.ReadToEnd();			
			fich.Close();
			
			return texto;
		}
		#endregion
		
		
		#region "Generador de entidades-Objeto"
		private string generarEntidadesObjeto(){
			string txt="";
				
			txt +="\t#region \"ENTIDADES-OBJETO\"\r\n";
			foreach(Tabla tb in Tablas)
			{
			ColumaTabla col = null;
			
						
			txt +="\tpublic class " + tb.NombreTabla +"\r\n\t{";
			
			//poner propiedades
			for(int i=0; i< tb.Columnas.Count(); i++)
			{
				col = tb.Columnas[i];
				txt +="\r\n\t\tpublic ";
				switch(col.TipoDato)
				{
					case "varchar":
						txt+="String ";
						break;
					case "int":						
						txt+="Int32 ";
						break;
					case "datetime":
						txt+="DateTime ";
						break;
				}
				txt+= Capitalizar(col.Nombre) + "{ get; set; }";
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
						
			txt +="\t#region \"TABLAS-OBJETO\"\r\n";	
			foreach(Tabla tb in Tablas)
			{
			string nombreTabla = pluralizar(tb.NombreTabla);
			ColumaTabla col = null;
			
						
			txt +="\tpublic class " + nombreTabla +"\r\n\t{\r\n";
			
			
			//SENTENCIA INSERT
			{
			txt +="\t\tprivate const String INSERT = \"INSERT INTO " + tb.NombreTabla +" (";
			for(int i=0; i< tb.Columnas.Count(); i++){
				col = tb.Columnas[i];
				if(col.isKey==false){
					txt+=col.Nombre;
					if((i+1)<tb.Columnas.Count() == true){
						txt+=",";
					}
				}
			}
			txt +=") VALUES (";
			for(int i=0; i< tb.Columnas.Count(); i++){
				col = tb.Columnas[i];
				if(col.isKey==false){
					
					txt+=ponerFormato(col);
					
					if((i+1)<tb.Columnas.Count() == true) txt+=","; 
				}
			}
			txt +=")\";\r\n";
			
			//delete
			txt +="\t\tprivate const String DELETE = \"DELETE FROM " + tb.NombreTabla +" WHERE ";
			List<ColumaTabla> idCols = (from u in tb.Columnas
				where u.isKey == true
				select u).ToList();
			
			for(int i=0; i< idCols.Count(); i++){
				col = idCols[i];
				
				
				txt+=col.Nombre + " = " + ponerFormato(col);
				
				if((i+1)<idCols.Count() == true){
					txt+=" and ";
				}
			}
			txt +="\";\r\n";
		}
			
			//update (no actualiza los campos id)
			{
				txt +="\t\tprivate const String UPDATE = \"UPDATE " + tb.NombreTabla +" SET ";
				
				List<ColumaTabla> notIdCols = (from u in tb.Columnas
					where u.isKey == false
					select u).ToList();
				
				for(int i=0; i< notIdCols.Count(); i++){
					col = notIdCols[i];
					
					txt+=col.Nombre + " = " + ponerFormato(col);
					
					if((i+1)<notIdCols.Count() == true){
						txt+=", ";
					}
				}
				
				txt +=" WHERE ";
				List<ColumaTabla> idCols = (from u in tb.Columnas
					where u.isKey == true
					select u).ToList();
				
				for(int i=0; i< idCols.Count(); i++){
					col = idCols[i];
					txt+=col.Nombre + " = " + ponerFormato(col);
					if((i+1)<idCols.Count() == true){
						txt+=", ";
					}
				}
				txt +="\";\r\n";
			}
			
			//select
			txt +="\t\tprivate const String SELECT = \"SELECT * FROM " + tb.NombreTabla +"\";\r\n\r\n";
			
			txt+="\t\tprivate static List<"+ tb.NombreTabla +"> _lista = null;\r\n\r\n";
			
			txt +="\t\tpublic static List<"+ tb.NombreTabla +"> toList()\r\n\t\t{\r\n";
        
			txt+=@"
                if (_lista == null)
                {
                    _lista = new List<"+ tb.NombreTabla + @">();
                }

                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));

                return _lista;
	
		    ";
			txt+="\r\n\t\t}\r\n\r\n";//final metodo Filas
			
			txt +=@"
	        public static int Add(" + tb.NombreTabla + @" _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(" + tb.NombreTabla + @" _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(" + tb.NombreTabla + @" _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, " + tb.NombreTabla + @" _conf)
	        {
	            String s = _sql;";
				string campo ="";
				foreach(ColumaTabla col1 in tb.Columnas){
					campo="_conf." + Capitalizar(col1.Nombre);
					if(col1.TipoDato =="datetime"){
						
						campo = "(_conf." + Capitalizar(col1.Nombre) + ".Year + \"/\" + _conf."+ Capitalizar(col1.Nombre) + ".Month + \"/\" + _conf."+ Capitalizar(col1.Nombre) + ".Day)";
					}
					txt+="\r\n\t\t\t\ts = s.Replace(\"@" + col1.Nombre + "\", "+ campo + ".ToString());";
				}
			
			txt+=@"
	            return s;
	        }
	
	        private static List<" + tb.NombreTabla + @"> mapeoObjeto(DataTable dt)
	        {
	            List<" + tb.NombreTabla + @"> resp = new List<" + tb.NombreTabla + @">();
	            foreach (DataRow fila in dt.Rows)
	            {";
			
	        txt+="\r\n\t\t\t" + tb.NombreTabla + " conf = new " + tb.NombreTabla + "();";
	        foreach(ColumaTabla col2 in tb.Columnas)
	        {
	        	//conf.Id = fila.Field<Int32>("ID");
	        	txt+="\r\n\t\t\t\tconf." + Capitalizar(col2.Nombre) + " = fila.Field<";
	        	switch(col2.TipoDato)
				{
					case "varchar":
						txt+="String";
						break;
					case "int":						
						txt+="Int32";
						break;
					case "datetime":
						txt+="DateTime";
						break;
				}
	        	
	        	txt+=">(\"" + col2.Nombre + "\");";
	        }
	        
			
	        txt+=@"
	                resp.Add(conf);
	            }
	            return resp;
	        }

			";
			
			
			
			txt +="\r\n\t}\r\n"; //final de clase
			}
			txt +="\t#endregion\r\n";
			return txt;
		}
		#endregion
	}
}
