/*
 * 
 * Usuario: https://github.com/arkadoel
 * Fecha: 03/04/2015
 * Hora: 18:22
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;

using DirectORM;
using System.Xml;
using System.Xml.Linq;

namespace DirectORM2015
{
	/// <summary>
	/// Clase encargada de la gestion de los XML tanto de 
	/// cargar la estructura de la DB desde un XML como 
	/// de guardar la db a un xml para exportarlo
	/// 
	/// </summary>
	public class gestorXML
	{
		public static void CreateFile(string filename)
		{
			System.IO.StreamWriter fich = new StreamWriter(filename, false);
			fich.WriteLine("<?xml version=\"1.0\"?>");
			fich.WriteLine("<DirectORM>");
			fich.WriteLine("</DirectORM>");
			fich.Close();
			
		}
		
		/// <summary>
		/// Exporta una serie de tablas a un archivo xml
		/// </summary>
		/// <param name="tablas"></param>
		/// <param name="filename"></param>
		public static void ExportarXML(List<Table> tablas, string filename)
		{
			//sino existe el archivo, lo creamos e iniciamos
			gestorXML.CreateFile(filename);
			
			//cargar archivo y agregar elmentos
			XDocument doc = XDocument.Load(filename);
						
			XElement xtable = null;
			XElement xcol = null;
			
			foreach(Table tabla in tablas)
			{
				//generamos el nodo de la tabla
				xtable = new XElement("table");
				xtable.SetAttributeValue("name", tabla.TableName);
				
				//al nodo de la tabla le añadimos las columnas
				foreach(TableColumn col in tabla.Cols)
				{
					xcol = new XElement("column");
					xcol.SetAttributeValue("name", col.Name);
                    if (col.ValueType == "date")
                    {
                        xcol.SetAttributeValue("type", "datetime");
                    }
                    else 
                    { 
                        xcol.SetAttributeValue("type", col.ValueType); 
                    }
					
					if(col.isKey == true)
					{
						xcol.SetAttributeValue("key", true);
						if(col.AutoIncrement == true)
						{
							xcol.SetAttributeValue("auto_increment", true);
						}
					}					
					
					xtable.Add(xcol);
				}
				
				//lo agregamos al archivo XML
				doc.Element("DirectORM").Add(xtable);
			}
			doc.Save(filename, SaveOptions.None);
			
			gestorXML.CloseXMLDoc(doc);
		}
		
		public static void CloseXMLDoc(XDocument doc)
		{
			doc = null;
			GC.Collect();
		}
		
		/// <summary>
		/// Importa de un archivo xml la definicion de 
		/// las tablas de la base de datos
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static List<Table> ImportarXML(string filename)
		{
			List<Table> tablas = new List<Table>();
			
			XDocument doc = XDocument.Load(filename);
			
			var xtables = doc.Element("DirectORM").Elements();
			
			Table tabla = null;
			TableColumn col = null;
			
			foreach(XElement xtable in xtables)
			{
				tabla = new Table();
				tabla.TableName = xtable.Attribute("name").Value;
				
				var xcols = xtable.Elements();
				
				foreach(XElement xcol in xcols)
				{
					col = new TableColumn();
					col.Name = xcol.Attribute("name").Value;
					col.ValueType = xcol.Attribute("type").Value;
					
					if(xcol.Attribute("key") != null)
					{
						col.isKey = true;
						if(xcol.Attribute("auto_increment") != null)
						{
							col.AutoIncrement = true;
						}
					}
					
					tabla.Cols.Add(col);
				}
				
				tablas.Add(tabla);
			}
			
			gestorXML.CloseXMLDoc(doc);
			return tablas;
		}
	}
}
