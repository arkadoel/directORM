using DirectORM;
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

            GestorDB.CADENA_CONEXION = @"Server=.\SQLEXPRESS; Database=DBTPV2015; Trusted_Connection=yes; ";

            Horario h = new Horario();
            h.IdEmpleado = 32;
            h.Hora_salida = "23";
            h.Hora_entrada = "23";

            DirectORM.Tablas.Horarios.Add(h);

            foreach(Horario hr in DirectORM.Tablas.Horarios.toList() )
            {
                if (hr.Dia_semana == null)
                {
                    Console.WriteLine("[" + hr.IdHorario.ToString() + "] NULL");
                }
                else Console.WriteLine("[" + hr.IdHorario.ToString() + "] " + hr.Dia_semana.ToString());
            }
            
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}