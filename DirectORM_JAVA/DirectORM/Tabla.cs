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
	public class Tabla
	{
		public String NombreTabla{get; set;}
		public List<ColumaTabla> Columnas{get; set;}
		
		public Tabla(){
			Columnas = new List<ColumaTabla>();
		}
		
		public Tabla(string nombre){
			this.NombreTabla = nombre;
			Columnas = new List<ColumaTabla>();
		}
	}
}
