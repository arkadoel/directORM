/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 07/26/2013
 * Hora: 18:55
 * 
 * 
 */
using System;
using System.Collections.Generic;

namespace DirectORM
{
	/// <summary>
	/// Description of Tabla.
	/// </summary>
	public class Table
	{
		public String TableName{get; set;}
		public List<TableColumn> Cols {get; set;}
		
		public Table(){
			Cols = new List<TableColumn>();
		}
		
		public Table(string nombre){
			this.TableName = nombre;
			Cols = new List<TableColumn>();
			Cols.Add(new TableColumn("id", "int", true, true));
			Cols.Add(new TableColumn("fecha", "datetime"));
			Cols.Add(new TableColumn("campo", "varchar", true));
			Cols.Add(new TableColumn("numero", "int"));
		}
	}
}
