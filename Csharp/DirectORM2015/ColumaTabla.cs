/*
 * 
 * Usuario: https://github.com/arkadoel
 * Fecha: 1-Mar-2015
 * Hora: 18:56
 * 
 */
using System;

namespace DirectORM
{
	/// <summary>
	/// Description of ColumaTabla.
	/// </summary>
	public class TableColumn
	{
		public string Name{get; set;}
		public string ValueType{get; set;}
		public bool isKey {get; set;}
		public bool AutoIncrement{ get; set; }
		
		public TableColumn(){ }		
		
		public TableColumn(string name, string valueType, bool key){
			Name = name;
			ValueType = valueType;
			isKey = key;
			AutoIncrement = false;
		}
		
		public TableColumn(string name, string valueType){
			Name = name;
			ValueType = valueType;
			isKey = false;
		}
		
		public TableColumn(string name, string valueType, bool key, bool autoIncrement){
			Name = name;
			ValueType = valueType;
			isKey = key;
			AutoIncrement = autoIncrement;
		}
	}
}
