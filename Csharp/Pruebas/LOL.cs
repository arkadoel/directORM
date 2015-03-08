using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DirectORM
{
	#region "ENTIDADES-OBJETO"
	public class Empleado
	{
		public Nullable<Int32> IdEmpleado{ get; set; }
		public String Nombre{ get; set; }
		public String Apellidos{ get; set; }
		public String Email{ get; set; }
		public String Tlf_fijo{ get; set; }
		public String Tlf_movil{ get; set; }
		public String Direccion{ get; set; }
		public String Foto_empleado{ get; set; }
		public String Banco{ get; set; }
		public String Cuenta_bancaria{ get; set; }
		public String Sexo{ get; set; }
		public String Password{ get; set; }
		public String Login_name{ get; set; }
		public String Cargo{ get; set; }
	}
	public class Horario
	{
		public Nullable<Int32> IdHorario{ get; set; }
		public Nullable<Int32> IdEmpleado{ get; set; }
		public String Dia_semana{ get; set; }
		public String Hora_entrada{ get; set; }
		public String Hora_salida{ get; set; }
	}
	public class Producto
	{
		public Nullable<Int32> IdProducto{ get; set; }
		public String Nombre_producto{ get; set; }
		public Nullable<float> Precio_unidad{ get; set; }
		public Nullable<Int32> Iva{ get; set; }
		public String Foto_producto{ get; set; }
		public String Ingredientes{ get; set; }
		public String Familia{ get; set; }
	}
	public class Proveedor
	{
		public Nullable<Int32> IdProveedor{ get; set; }
		public String Nombre{ get; set; }
		public String Email{ get; set; }
		public String Tlf_fijo{ get; set; }
		public String Tlf_movil{ get; set; }
		public String Tlf_fijo2{ get; set; }
		public String Tlf_movil2{ get; set; }
		public String Banco{ get; set; }
		public String Cuenta_bancaria{ get; set; }
		public String Direccion{ get; set; }
		public String Foto_logo{ get; set; }
	}
	public class Productos_Proveedor
	{
		public Nullable<Int32> Id{ get; set; }
		public Nullable<Int32> Id_producto{ get; set; }
		public Nullable<Int32> Id_proveedor{ get; set; }
		public DateTime Fecha{ get; set; }
		public Nullable<float> Cantidad{ get; set; }
	}
	public class Orden
	{
		public Nullable<Int32> IdOrden{ get; set; }
		public Nullable<Int32> IdEmpleado{ get; set; }
		public String Num_ticket{ get; set; }
		public DateTime Fecha{ get; set; }
		public DateTime Hora{ get; set; }
		public String Lugar{ get; set; }
	}
	public class Productos_en_Ordenes
	{
		public Nullable<Int32> IdOrden{ get; set; }
		public Nullable<Int32> IdProducto{ get; set; }
		public Nullable<float> Cantidad{ get; set; }
	}
	#endregion


	#region "TABLAS-OBJETO"
	namespace Tablas{
	public class Empleados
	{
		private const String INSERT = "INSERT INTO Empleado (nombre,apellidos,email,tlf_fijo,tlf_movil,direccion,foto_empleado,banco,cuenta_bancaria,sexo,password,login_name,cargo) VALUES ('@nombre','@apellidos','@email','@tlf_fijo','@tlf_movil','@direccion','@foto_empleado','@banco','@cuenta_bancaria','@sexo','@password','@login_name','@cargo')";
		private const String DELETE = "DELETE FROM Empleado WHERE idEmpleado = @idEmpleado";
		private const String UPDATE = "UPDATE Empleado SET nombre = '@nombre', apellidos = '@apellidos', email = '@email', tlf_fijo = '@tlf_fijo', tlf_movil = '@tlf_movil', direccion = '@direccion', foto_empleado = '@foto_empleado', banco = '@banco', cuenta_bancaria = '@cuenta_bancaria', sexo = '@sexo', password = '@password', login_name = '@login_name', cargo = '@cargo' WHERE idEmpleado = @idEmpleado";
		private const String SELECT = "SELECT * FROM Empleado";

		private static List<Empleado> _lista = null;

		public static List<Empleado> toList()
		{

                if (_lista == null)
                {
                    _lista = new List<Empleado>();
                }

                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));

                return _lista;
	
		    
		}


	        public static int Add(Empleado _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(Empleado _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(Empleado _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, Empleado _conf)
	        {
	            String s = _sql;
				s = s.Replace("@idEmpleado", _conf.IdEmpleado.ToString());
				s = s.Replace("@nombre", _conf.Nombre.ToString());
				s = s.Replace("@apellidos", _conf.Apellidos.ToString());
				s = s.Replace("@email", _conf.Email.ToString());
				s = s.Replace("@tlf_fijo", _conf.Tlf_fijo.ToString());
				s = s.Replace("@tlf_movil", _conf.Tlf_movil.ToString());
				s = s.Replace("@direccion", _conf.Direccion.ToString());
				s = s.Replace("@foto_empleado", _conf.Foto_empleado.ToString());
				s = s.Replace("@banco", _conf.Banco.ToString());
				s = s.Replace("@cuenta_bancaria", _conf.Cuenta_bancaria.ToString());
				s = s.Replace("@sexo", _conf.Sexo.ToString());
				s = s.Replace("@password", _conf.Password.ToString());
				s = s.Replace("@login_name", _conf.Login_name.ToString());
				s = s.Replace("@cargo", _conf.Cargo.ToString());
	            return s;
	        }
	
	        private static List<Empleado> mapeoObjeto(DataTable dt)
	        {
	            List<Empleado> resp = new List<Empleado>();
	            foreach (DataRow fila in dt.Rows)
	            {
			Empleado conf = new Empleado();
				conf.IdEmpleado = fila.Field<Nullable<Int32>>("idEmpleado");
				conf.Nombre = fila.Field<String>("nombre");
				conf.Apellidos = fila.Field<String>("apellidos");
				conf.Email = fila.Field<String>("email");
				conf.Tlf_fijo = fila.Field<String>("tlf_fijo");
				conf.Tlf_movil = fila.Field<String>("tlf_movil");
				conf.Direccion = fila.Field<String>("direccion");
				conf.Foto_empleado = fila.Field<String>("foto_empleado");
				conf.Banco = fila.Field<String>("banco");
				conf.Cuenta_bancaria = fila.Field<String>("cuenta_bancaria");
				conf.Sexo = fila.Field<String>("sexo");
				conf.Password = fila.Field<String>("password");
				conf.Login_name = fila.Field<String>("login_name");
				conf.Cargo = fila.Field<String>("cargo");
	                resp.Add(conf);
	            }
	            return resp;
	        }

			
	}
	public class Horarios
	{
		private const String INSERT = "INSERT INTO Horario (idEmpleado,dia_semana,hora_entrada,hora_salida) VALUES (@idEmpleado,'@dia_semana','@hora_entrada','@hora_salida')";
		private const String DELETE = "DELETE FROM Horario WHERE idHorario = @idHorario and idEmpleado = @idEmpleado";
		private const String UPDATE = "UPDATE Horario SET dia_semana = '@dia_semana', hora_entrada = '@hora_entrada', hora_salida = '@hora_salida' WHERE idHorario = @idHorario, idEmpleado = @idEmpleado";
		private const String SELECT = "SELECT * FROM Horario";

		private static List<Horario> _lista = null;

		public static List<Horario> toList()
		{

                if (_lista == null)
                {
                    _lista = new List<Horario>();
                }

                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));

                return _lista;
	
		    
		}


	        public static int Add(Horario _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(Horario _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(Horario _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, Horario _conf)
	        {
	            String s = _sql;
				s = s.Replace("@idHorario", _conf.IdHorario.ToString());
				s = s.Replace("@idEmpleado", _conf.IdEmpleado.ToString());
				s = s.Replace("@dia_semana", _conf.Dia_semana.ToString());
				s = s.Replace("@hora_entrada", _conf.Hora_entrada.ToString());
				s = s.Replace("@hora_salida", _conf.Hora_salida.ToString());
	            return s;
	        }
	
	        private static List<Horario> mapeoObjeto(DataTable dt)
	        {
	            List<Horario> resp = new List<Horario>();
	            foreach (DataRow fila in dt.Rows)
	            {
			Horario conf = new Horario();
				conf.IdHorario = fila.Field<Nullable<Int32>>("idHorario");
				conf.IdEmpleado = fila.Field<Nullable<Int32>>("idEmpleado");
				conf.Dia_semana = fila.Field<String>("dia_semana");
				conf.Hora_entrada = fila.Field<String>("hora_entrada");
				conf.Hora_salida = fila.Field<String>("hora_salida");
	                resp.Add(conf);
	            }
	            return resp;
	        }

			
	}
	public class Productos
	{
		private const String INSERT = "INSERT INTO Producto (nombre_producto,precio_unidad,iva,foto_producto,ingredientes,familia) VALUES ('@nombre_producto',@precio_unidad,@iva,'@foto_producto','@ingredientes','@familia')";
		private const String DELETE = "DELETE FROM Producto WHERE idProducto = @idProducto";
		private const String UPDATE = "UPDATE Producto SET nombre_producto = '@nombre_producto', precio_unidad = @precio_unidad, iva = @iva, foto_producto = '@foto_producto', ingredientes = '@ingredientes', familia = '@familia' WHERE idProducto = @idProducto";
		private const String SELECT = "SELECT * FROM Producto";

		private static List<Producto> _lista = null;

		public static List<Producto> toList()
		{

                if (_lista == null)
                {
                    _lista = new List<Producto>();
                }

                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));

                return _lista;
	
		    
		}


	        public static int Add(Producto _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(Producto _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(Producto _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, Producto _conf)
	        {
	            String s = _sql;
				s = s.Replace("@idProducto", _conf.IdProducto.ToString());
				s = s.Replace("@nombre_producto", _conf.Nombre_producto.ToString());
				s = s.Replace("@precio_unidad", _conf.Precio_unidad.ToString());
				s = s.Replace("@iva", _conf.Iva.ToString());
				s = s.Replace("@foto_producto", _conf.Foto_producto.ToString());
				s = s.Replace("@ingredientes", _conf.Ingredientes.ToString());
				s = s.Replace("@familia", _conf.Familia.ToString());
	            return s;
	        }
	
	        private static List<Producto> mapeoObjeto(DataTable dt)
	        {
	            List<Producto> resp = new List<Producto>();
	            foreach (DataRow fila in dt.Rows)
	            {
			Producto conf = new Producto();
				conf.IdProducto = fila.Field<Nullable<Int32>>("idProducto");
				conf.Nombre_producto = fila.Field<String>("nombre_producto");
				conf.Precio_unidad = fila.Field<Nullable<float>>("precio_unidad");
				conf.Iva = fila.Field<Nullable<Int32>>("iva");
				conf.Foto_producto = fila.Field<String>("foto_producto");
				conf.Ingredientes = fila.Field<String>("ingredientes");
				conf.Familia = fila.Field<String>("familia");
	                resp.Add(conf);
	            }
	            return resp;
	        }

			
	}
	public class Proveedores
	{
		private const String INSERT = "INSERT INTO Proveedor (nombre,email,tlf_fijo,tlf_movil,tlf_fijo2,tlf_movil2,banco,cuenta_bancaria,direccion,foto_logo) VALUES ('@nombre','@email','@tlf_fijo','@tlf_movil','@tlf_fijo2','@tlf_movil2','@banco','@cuenta_bancaria','@direccion','@foto_logo')";
		private const String DELETE = "DELETE FROM Proveedor WHERE idProveedor = @idProveedor";
		private const String UPDATE = "UPDATE Proveedor SET nombre = '@nombre', email = '@email', tlf_fijo = '@tlf_fijo', tlf_movil = '@tlf_movil', tlf_fijo2 = '@tlf_fijo2', tlf_movil2 = '@tlf_movil2', banco = '@banco', cuenta_bancaria = '@cuenta_bancaria', direccion = '@direccion', foto_logo = '@foto_logo' WHERE idProveedor = @idProveedor";
		private const String SELECT = "SELECT * FROM Proveedor";

		private static List<Proveedor> _lista = null;

		public static List<Proveedor> toList()
		{

                if (_lista == null)
                {
                    _lista = new List<Proveedor>();
                }

                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));

                return _lista;
	
		    
		}


	        public static int Add(Proveedor _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(Proveedor _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(Proveedor _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, Proveedor _conf)
	        {
	            String s = _sql;
				s = s.Replace("@idProveedor", _conf.IdProveedor.ToString());
				s = s.Replace("@nombre", _conf.Nombre.ToString());
				s = s.Replace("@email", _conf.Email.ToString());
				s = s.Replace("@tlf_fijo", _conf.Tlf_fijo.ToString());
				s = s.Replace("@tlf_movil", _conf.Tlf_movil.ToString());
				s = s.Replace("@tlf_fijo2", _conf.Tlf_fijo2.ToString());
				s = s.Replace("@tlf_movil2", _conf.Tlf_movil2.ToString());
				s = s.Replace("@banco", _conf.Banco.ToString());
				s = s.Replace("@cuenta_bancaria", _conf.Cuenta_bancaria.ToString());
				s = s.Replace("@direccion", _conf.Direccion.ToString());
				s = s.Replace("@foto_logo", _conf.Foto_logo.ToString());
	            return s;
	        }
	
	        private static List<Proveedor> mapeoObjeto(DataTable dt)
	        {
	            List<Proveedor> resp = new List<Proveedor>();
	            foreach (DataRow fila in dt.Rows)
	            {
			Proveedor conf = new Proveedor();
				conf.IdProveedor = fila.Field<Nullable<Int32>>("idProveedor");
				conf.Nombre = fila.Field<String>("nombre");
				conf.Email = fila.Field<String>("email");
				conf.Tlf_fijo = fila.Field<String>("tlf_fijo");
				conf.Tlf_movil = fila.Field<String>("tlf_movil");
				conf.Tlf_fijo2 = fila.Field<String>("tlf_fijo2");
				conf.Tlf_movil2 = fila.Field<String>("tlf_movil2");
				conf.Banco = fila.Field<String>("banco");
				conf.Cuenta_bancaria = fila.Field<String>("cuenta_bancaria");
				conf.Direccion = fila.Field<String>("direccion");
				conf.Foto_logo = fila.Field<String>("foto_logo");
	                resp.Add(conf);
	            }
	            return resp;
	        }

			
	}
	public class Productos_Proveedores
	{
		private const String INSERT = "INSERT INTO Productos_Proveedor (id_producto,id_proveedor,fecha,cantidad) VALUES (@id_producto,@id_proveedor,#@fecha#,@cantidad)";
		private const String DELETE = "DELETE FROM Productos_Proveedor WHERE id = @id and id_producto = @id_producto and id_proveedor = @id_proveedor";
		private const String UPDATE = "UPDATE Productos_Proveedor SET fecha = #@fecha#, cantidad = @cantidad WHERE id = @id, id_producto = @id_producto, id_proveedor = @id_proveedor";
		private const String SELECT = "SELECT * FROM Productos_Proveedor";

		private static List<Productos_Proveedor> _lista = null;

		public static List<Productos_Proveedor> toList()
		{

                if (_lista == null)
                {
                    _lista = new List<Productos_Proveedor>();
                }

                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));

                return _lista;
	
		    
		}


	        public static int Add(Productos_Proveedor _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(Productos_Proveedor _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(Productos_Proveedor _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, Productos_Proveedor _conf)
	        {
	            String s = _sql;
				s = s.Replace("@id", _conf.Id.ToString());
				s = s.Replace("@id_producto", _conf.Id_producto.ToString());
				s = s.Replace("@id_proveedor", _conf.Id_proveedor.ToString());
				s = s.Replace("@fecha", (_conf.Fecha.Year + "/" + _conf.Fecha.Month + "/" + _conf.Fecha.Day).ToString());
				s = s.Replace("@cantidad", _conf.Cantidad.ToString());
	            return s;
	        }
	
	        private static List<Productos_Proveedor> mapeoObjeto(DataTable dt)
	        {
	            List<Productos_Proveedor> resp = new List<Productos_Proveedor>();
	            foreach (DataRow fila in dt.Rows)
	            {
			Productos_Proveedor conf = new Productos_Proveedor();
				conf.Id = fila.Field<Nullable<Int32>>("id");
				conf.Id_producto = fila.Field<Nullable<Int32>>("id_producto");
				conf.Id_proveedor = fila.Field<Nullable<Int32>>("id_proveedor");
				conf.Fecha = fila.Field<DateTime>("fecha");
				conf.Cantidad = fila.Field<Nullable<float>>("cantidad");
	                resp.Add(conf);
	            }
	            return resp;
	        }

			
	}
	public class Ordenes
	{
		private const String INSERT = "INSERT INTO Orden (idEmpleado,num_ticket,fecha,hora,lugar) VALUES (@idEmpleado,'@num_ticket',#@fecha#,#@hora#,'@lugar')";
		private const String DELETE = "DELETE FROM Orden WHERE idOrden = @idOrden and idEmpleado = @idEmpleado";
		private const String UPDATE = "UPDATE Orden SET num_ticket = '@num_ticket', fecha = #@fecha#, hora = #@hora#, lugar = '@lugar' WHERE idOrden = @idOrden, idEmpleado = @idEmpleado";
		private const String SELECT = "SELECT * FROM Orden";

		private static List<Orden> _lista = null;

		public static List<Orden> toList()
		{

                if (_lista == null)
                {
                    _lista = new List<Orden>();
                }

                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));

                return _lista;
	
		    
		}


	        public static int Add(Orden _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(Orden _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(Orden _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, Orden _conf)
	        {
	            String s = _sql;
				s = s.Replace("@idOrden", _conf.IdOrden.ToString());
				s = s.Replace("@idEmpleado", _conf.IdEmpleado.ToString());
				s = s.Replace("@num_ticket", _conf.Num_ticket.ToString());
				s = s.Replace("@fecha", (_conf.Fecha.Year + "/" + _conf.Fecha.Month + "/" + _conf.Fecha.Day).ToString());
				s = s.Replace("@hora", (_conf.Hora.Year + "/" + _conf.Hora.Month + "/" + _conf.Hora.Day).ToString());
				s = s.Replace("@lugar", _conf.Lugar.ToString());
	            return s;
	        }
	
	        private static List<Orden> mapeoObjeto(DataTable dt)
	        {
	            List<Orden> resp = new List<Orden>();
	            foreach (DataRow fila in dt.Rows)
	            {
			Orden conf = new Orden();
				conf.IdOrden = fila.Field<Nullable<Int32>>("idOrden");
				conf.IdEmpleado = fila.Field<Nullable<Int32>>("idEmpleado");
				conf.Num_ticket = fila.Field<String>("num_ticket");
				conf.Fecha = fila.Field<DateTime>("fecha");
				conf.Hora = fila.Field<DateTime>("hora");
				conf.Lugar = fila.Field<String>("lugar");
	                resp.Add(conf);
	            }
	            return resp;
	        }

			
	}
	public class Productos_en_Ordeneses
	{
		private const String INSERT = "INSERT INTO Productos_en_Ordenes (idProducto,cantidad) VALUES (@idProducto,@cantidad)";
		private const String DELETE = "DELETE FROM Productos_en_Ordenes WHERE idOrden = @idOrden and idProducto = @idProducto";
		private const String UPDATE = "UPDATE Productos_en_Ordenes SET cantidad = @cantidad WHERE idOrden = @idOrden, idProducto = @idProducto";
		private const String SELECT = "SELECT * FROM Productos_en_Ordenes";

		private static List<Productos_en_Ordenes> _lista = null;

		public static List<Productos_en_Ordenes> toList()
		{

                if (_lista == null)
                {
                    _lista = new List<Productos_en_Ordenes>();
                }

                _lista = mapeoObjeto(GestorDB.Consulta(SELECT));

                return _lista;
	
		    
		}


	        public static int Add(Productos_en_Ordenes _conf)
	        {
	            String sql = mapeoSQL(INSERT, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }

	        public static int Delete(Productos_en_Ordenes _conf)
	        {
	            String sql = mapeoSQL(DELETE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        public static int Update(Productos_en_Ordenes _conf)
	        {
	            String sql = mapeoSQL(UPDATE, _conf);
	            int resultado = GestorDB.Ejecuta(sql);
	            return resultado;
	        }
	
	        private static String mapeoSQL(String _sql, Productos_en_Ordenes _conf)
	        {
	            String s = _sql;
				s = s.Replace("@idOrden", _conf.IdOrden.ToString());
				s = s.Replace("@idProducto", _conf.IdProducto.ToString());
				s = s.Replace("@cantidad", _conf.Cantidad.ToString());
	            return s;
	        }
	
	        private static List<Productos_en_Ordenes> mapeoObjeto(DataTable dt)
	        {
	            List<Productos_en_Ordenes> resp = new List<Productos_en_Ordenes>();
	            foreach (DataRow fila in dt.Rows)
	            {
			Productos_en_Ordenes conf = new Productos_en_Ordenes();
				conf.IdOrden = fila.Field<Nullable<Int32>>("idOrden");
				conf.IdProducto = fila.Field<Nullable<Int32>>("idProducto");
				conf.Cantidad = fila.Field<Nullable<float>>("cantidad");
	                resp.Add(conf);
	            }
	            return resp;
	        }

			
	}
	}
	#endregion


#region "GESTOR DE LA BASE DE DATOS"
    public class GestorDB
    {
        /***
         * Se hace mediante LINQ to Dataset debido a que se quiere hacer lo
         * mas portable posible. No todos los equipos disponen de SQL Server
         * instalado, pero las librerias para el acceso a access vienen por 
         * defecto con el .NET framework. 
         */
        //public static String CADENA_CONEXION = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\DB2015.mdb;User Id=admin;Password=;";
		public static String CADENA_CONEXION = @"Provider=SQLNCLI11; Server=.\SQLEXPRESS;Initial Database=DB2015; Trusted_Connection=yes; ";
        private static SqlConnection _conexion = null;

        /// <summary>
        /// Conexion automatica con la base de datos
        /// </summary>
        public static SqlConnection Conexion 
        {
            get 
            {
                if (_conexion == null)
                {
                    _conexion = new SqlConnection(CADENA_CONEXION);
                }

                if (_conexion.State != ConnectionState.Open)
                {
                    _conexion.Open();
                }
                return _conexion;
            }
            set
            {
                _conexion = value;
            }
        }
        
        /// <summary>
        /// Finalizar la conexion existente si se puede
        /// </summary>
        /// <returns>Devuelve un string con 'OK' o con la descripcion
        /// del error resultante
        /// </returns>
        public static String FinConexion()
        {
            try
            {
                if (Conexion != null)
                {
                    if (Conexion.State == ConnectionState.Open)
                    {
                        Conexion.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            return "OK";
        }

        /// <summary>
        /// Permite ejecutar sentencias SQL SELECT contra la base de datos,
        /// devuelve un dataset con los datos seleccionados.
        /// </summary>
        /// <param name="_sql">Sentencia select</param>
        /// <param name="_nombreTabla">Nombre de la tabla resultado</param>
        /// <returns></returns>
        public static DataTable Consulta(String _sql)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(_sql, GestorDB.Conexion);
            da.Fill(ds, "t");
            GestorDB.FinConexion();
            return ds.Tables["t"];
        }

        /// <summary>
        /// Ejecuta sentencias insert, delete o update. Retorna el numero
        /// de filas afectadas o -1 si no se modifico nada.
        /// </summary>
        /// <param name="_sql"></param>
        /// <returns></returns>
        public static int Ejecuta(String _sql)
        {
            int resultado = -1;
            SqlCommand comando = new SqlCommand(_sql, GestorDB.Conexion);

            resultado = comando.ExecuteNonQuery();
            Console.WriteLine("filas afectadas : " + resultado.ToString());
            GestorDB.FinConexion();
            return resultado;
        }


    }

	#endregion


}


