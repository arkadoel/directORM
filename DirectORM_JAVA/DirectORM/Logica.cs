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
            
            
			txt = generarEntidadesObjeto();
			txt +="\r\n\r\n";
			txt = generarObjetosTabla();
			txt +="\r\n\r\n";
			txt = cargarGestorDB();
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

            texto = "package " + NAMESPACE + ";\r\n\r\n" + texto;
            escribirFichero("out\\GestorDB.java", texto);
			
			return texto;
		}
		#endregion
		
		
		#region "Generador de entidades-Objeto"
		private string generarEntidadesObjeto(){
			string txt="";
            
			//txt +="\t#region \"ENTIDADES-OBJETO\"\r\n";
			foreach(Tabla tb in Tablas)
			{
				txt = "package " + NAMESPACE + ";\r\n\r\n";
			    ColumaTabla col = null;
			
						
			    txt +="\tpublic class " + tb.NombreTabla +"\r\n\t{";
			
			    //poner propiedades
			    for(int i=0; i< tb.Columnas.Count(); i++)
			    {
				    col = tb.Columnas[i];
				    txt +="\r\n\t\tprivate ";
				    switch(col.TipoDato)
				    {
					    case "varchar":
						    txt+="String ";
						    break;
					    case "int":						
						    txt+="Integer ";
						    break;
					    case "datetime":
						    txt+="DateTime ";
						    break;
                        case "bool":
                            txt+="Boolean ";
                            break;
                        case "float":
                            txt +="Float ";
                            break;
				    }
				    txt+= Capitalizar(col.Nombre) + ";";                
			    }

                txt += "\r\n";
                //poner metodos get y set
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
						    txt+="Integer ";
						    break;
					    case "datetime":
						    txt+="DateTime ";
						    break;
                        case "bool":
                            txt+="Boolean ";
                            break;
                        case "float":
                            txt +="Float ";
                            break;
				    }
				    txt+= "get" + Capitalizar(col.Nombre) + "(){";
                    txt += "\r\n\t\t\treturn " + Capitalizar(col.Nombre) + ";\r\n\t\t}";
			    }

                txt += "\r\n";
                for (int i = 0; i < tb.Columnas.Count(); i++)
                {
                    col = tb.Columnas[i];
                    txt += "\r\n\t\tpublic void set" + Capitalizar(col.Nombre) +"(";
                    switch (col.TipoDato)
                    {
                        case "varchar":
                            txt += "String ";
                            break;
                        case "int":
                            txt += "Integer ";
                            break;
                        case "datetime":
                            txt += "DateTime ";
                            break;
                        case "bool":
                            txt += "Boolean ";
                            break;
                        case "float":
                            txt +="Float ";
                            break;
                    }
                    txt += " _valor){";
                    txt += "\r\n\t\t\t " + Capitalizar(col.Nombre) + " = _valor;\r\n\t\t}";
                }
			    txt +="\r\n";
			
			    txt +="\t}\r\n"; //final de clase
                escribirFichero("out\\" + tb.NombreTabla + ".java", txt);
			 }
			txt +="\r\n";
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

            
			foreach(Tabla tb in Tablas)
			{
				txt = "package " + NAMESPACE + ";\r\n\r\n";
            	txt += "import java.sql.ResultSet;\r\n" +
                "import java.util.ArrayList;\r\n\r\n";
				txt +="\t///////////////#region \"TABLAS-OBJETO\"\r\n";	
				string nombreTabla = pluralizar(tb.NombreTabla);
				ColumaTabla col = null;
				
							
				txt +="\tpublic class " + nombreTabla +"\r\n\t{\r\n";
				
				
				//SENTENCIA INSERT
				{
				txt +="\t\tprivate static String INSERT = \"INSERT INTO " + tb.NombreTabla +" (";
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
				txt +="\t\tprivate static String DELETE = \"DELETE FROM " + tb.NombreTabla +" WHERE ";
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
					txt +="\t\tprivate static String UPDATE = \"UPDATE " + tb.NombreTabla +" SET ";
					
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
				txt +="\t\tprivate static String SELECT = \"SELECT * FROM " + tb.NombreTabla +"\";\r\n\r\n";
				
				
				
				txt +=@"
		        public static int Add(" + tb.NombreTabla + @" _conf)
		        {
		            String sql = mapeoSQL(INSERT, _conf);
		            int resultado = GestorDB.Ejecutar(sql);
		            return resultado;
		        }
	
		        public static int Delete(" + tb.NombreTabla + @" _conf)
		        {
		            String sql = mapeoSQL(DELETE, _conf);
		            int resultado = GestorDB.Ejecutar(sql);
		            return resultado;
		        }
		
		        public static int Update(" + tb.NombreTabla + @" _conf)
		        {
		            String sql = mapeoSQL(UPDATE, _conf);
		            int resultado = GestorDB.Ejecutar(sql);
		            return resultado;
		        }
		
		        private static String mapeoSQL(String _sql, " + tb.NombreTabla + @" _conf)
		        {
		            String s = _sql;";
					string campo ="";
					foreach(ColumaTabla col1 in tb.Columnas){
						campo="_conf.get" + Capitalizar(col1.Nombre) + "()";
	                    txt += "\r\n\t\t\t\ts = s.replaceAll(\"@" + col1.Nombre + "\", " + campo + ".toString());";
					}
				
				txt+=@"
		            return s;
		        }
		
	            ";
	
	            txt += @"
	                public static ArrayList<" + tb.NombreTabla + @"> toArrayList(){
	                        ArrayList<" + tb.NombreTabla + @"> lista = new ArrayList<>();
	                        ResultSet rs = GestorDB.Consulta(SELECT);
	                        " + tb.NombreTabla + @" g =null;
	        
	                        try{
	                            while(rs.next()){
	                                g = new " + tb.NombreTabla + @"();
	                                ";
	            campo = "";
	            foreach (ColumaTabla col2 in tb.Columnas)
	            {
	                campo = "g.set" + Capitalizar(col2.Nombre) + "(rs.get";
	                switch (col2.TipoDato)
	                {
	                    case "varchar":
	                        campo += "String";
	                        break;
	                    case "int":
	                        campo += "Int";
	                        break;
	                    case "bool":
	                        campo += "Boolean";
	                        break;
	                    case "float":
	                        campo += "Float ";
	                        break;
	                }
	
	                campo += "(\"" + col2.Nombre + "\")";
	                
	                txt += "\r\n\t\t\t\t\t" + campo +");";
	            }
	            txt +=@"
	                                lista.add(g);
	                            }
	            
	                        }catch(Exception ex){
	                                ex.printStackTrace();
	                        }
	                        return lista;               
	                    }
	
	            ";
				
				
				
				txt +="\r\n\t}\r\n"; //final de clase
	            escribirFichero("out\\" + nombreTabla + ".java", txt);
			}
			//txt +="\t#endregion\r\n";
			return txt;
		}
		#endregion

        #region "ESCRIBIR EN FICHEROS"
        public static void escribirFichero(String ruta, String texto)
        {
            try
            {
                StreamWriter fich = new StreamWriter(ruta, false);
                fich.WriteLine(texto);
                fich.Close();
            }catch(Exception ex){
            	Console.WriteLine(ex.Message.ToString());
            }
        }
        #endregion
    }

    
}
