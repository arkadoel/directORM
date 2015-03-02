/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 07/26/2013
 * Hora: 18:56
 * 
 * 
 */
using System;

namespace DirectORM
{
	/// <summary>
	/// Description of ColumaTabla.
	/// </summary>
	public class ColumaTabla
	{
		public string Nombre{get; set;}
		public string TipoDato{get; set;}
		public bool isKey {get; set;}
		
		public ColumaTabla(){ }
		
		/// <summary>
		/// Crea la columna de la tabla especificando todas las propiedades
		/// </summary>
		/// <param name="nombre"></param>
		/// <param name="tipoDato"></param>
		/// <param name="esClave"></param>
		public ColumaTabla(string nombre, string tipoDato, bool esClave){
			Nombre = nombre;
			TipoDato = tipoDato;
			isKey = esClave;
		}
		
		/// <summary>
		/// Crea la columna de la tabla presuponiendo que no es clave
		/// </summary>
		/// <param name="nombre"></param>
		/// <param name="tipoDato"></param>
		public ColumaTabla(string nombre, string tipoDato){
			Nombre = nombre;
			TipoDato = tipoDato;
			isKey = false;
		}
	}
}
