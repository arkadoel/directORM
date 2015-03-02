/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 30/07/2013
 * Hora: 8:48
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Pruebas
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			GestorDB.CADENA_CONEXION=@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\db.mdb;User Id=admin;Password=;";
			
			
			
			Nacimiento n = new Nacimiento();
			n.Fecha = DateTime.Today;
			n.Campo="Hola";
			n.Numero=32;
			
			Nacimientos.Add(n);
			
			List<Nacimiento> nac = Nacimientos.toList();
			
			Console.WriteLine("Numero de filas: " + nac.Count.ToString());
			
			foreach(Nacimiento nu in nac){
				Nacimientos.Delete(nu);
			}
			Console.WriteLine("Numero de filas tras borrar: " + Nacimientos.toList().Count.ToString());
			
			n = new Nacimiento();
			n.Fecha = DateTime.Today;
			n.Campo="Hola";
			n.Numero=32;
			
			Nacimientos.Add(n);
			
				
			n = (from u in Nacimientos.toList()
				where u.Fecha == DateTime.Today
				select u).First();
			
			n.Campo = "33";
			Nacimientos.Update(n);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}