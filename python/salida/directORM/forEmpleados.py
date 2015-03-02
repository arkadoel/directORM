
import sqlite3
import directORM

class Empleado:
    def __init__(self):

        self.idEmpleado = -1
        self.nombre = ''
        self.apellidos = ''
        self.email = ''
        self.tlf_fijo = ''
        self.tlf_movil = ''
        self.direccion = ''
        self.foto_empleado = ''
        self.banco = ''
        self.cuenta_bancaria = ''
        self.sexo = ''
        self.password = ''
        self.login_name = ''
        self.cargo = ''

class TbEmpleados:
    INSERT = '''
        insert into Empleados
        ( nombre, apellidos, email, tlf_fijo, tlf_movil, direccion, foto_empleado, banco, cuenta_bancaria, sexo, password, login_name, cargo)
        values (?,?,?,?,?,?,?,?,?,?,?,?,?)
        '''
    DELETE = 'delete from Empleados where idEmpleado = ?'
    SELECT = 'select * from Empleados'
    UPDATE = '''
        update Empleados set  
        nombre = ?,
        apellidos = ?,
        email = ?,
        tlf_fijo = ?,
        tlf_movil = ?,
        direccion = ?,
        foto_empleado = ?,
        banco = ?,
        cuenta_bancaria = ?,
        sexo = ?,
        password = ?,
        login_name = ?,
        cargo = ?
        where  idEmpleado = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def remove(self, empleado ):
        sql = self.DELETE
        self.gestorDB.ejecutarSQL(sql, (empleado.idEmpleado))

    def get_empleado(self, idEmpleado=None):
        sql = self.SELECT + " where idEmpleado=" + str(idEmpleado) +";"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = self.mapear_objeto(fila)
            return o


    def save(self, empleado=None):
        if empleado is not None:
            if self.get_empleado(empleado.idEmpleado) is None:
                sql = self.INSERT
                self.gestorDB.ejecutarSQL(sql, (
                    empleado.nombre,
                    empleado.apellidos,
                    empleado.email,
                    empleado.tlf_fijo,
                    empleado.tlf_movil,
                    empleado.direccion,
                    empleado.foto_empleado,
                    empleado.banco,
                    empleado.cuenta_bancaria,
                    empleado.sexo,
                    empleado.password,
                    empleado.login_name,
                    empleado.cargo))
            else:
                sql = self.UPDATE
                self.gestorDB.ejecutarSQL(sql, (
                    empleado.nombre,
                    empleado.apellidos,
                    empleado.email,
                    empleado.tlf_fijo,
                    empleado.tlf_movil,
                    empleado.direccion,
                    empleado.foto_empleado,
                    empleado.banco,
                    empleado.cuenta_bancaria,
                    empleado.sexo,
                    empleado.password,
                    empleado.login_name,
                    empleado.cargo,
                    empleado.idEmpleado))

    def mapear_objeto(self, fila=None):
        if fila is None:
            return None
        else:
            o = Empleado()
            o.idEmpleado = fila['idEmpleado']
            o.nombre = fila['nombre']
            o.apellidos = fila['apellidos']
            o.email = fila['email']
            o.tlf_fijo = fila['tlf_fijo']
            o.tlf_movil = fila['tlf_movil']
            o.direccion = fila['direccion']
            o.foto_empleado = fila['foto_empleado']
            o.banco = fila['banco']
            o.cuenta_bancaria = fila['cuenta_bancaria']
            o.sexo = fila['sexo']
            o.password = fila['password']
            o.login_name = fila['login_name']
            o.cargo = fila['cargo']
            return o

    def get_empleados(self, filtro=None):
        if filtro is None:
            sql = self.SELECT
        else: 
            sql = self.SELECT + " where " + filtro
        filas = self.gestorDB.consultaSQL(sql)
        objetos = list()
        for fila in filas:
            o = self.mapear_objeto(fila)
            objetos.append(o)

        return objetos




