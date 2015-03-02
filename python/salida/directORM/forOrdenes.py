
import sqlite3
import directORM

class Orden:
    def __init__(self):

        self.idOrden = -1
        self.idEmpleado = -1
        self.num_ticket = ''
        self.fecha = ''
        self.hora = ''
        self.lugar = ''

class TbOrdenes:
    INSERT = '''
        insert into Ordenes
        ( num_ticket, fecha, hora, lugar)
        values (?,?,?,?)
        '''
    DELETE = 'delete from Ordenes where idOrden = ? and idEmpleado = ?'
    SELECT = 'select * from Ordenes'
    UPDATE = '''
        update Ordenes set  
        num_ticket = ?,
        fecha = ?,
        hora = ?,
        lugar = ?
        where  idOrden = ? and idEmpleado = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def remove(self, orden ):
        sql = self.DELETE
        self.gestorDB.ejecutarSQL(sql, (orden.idOrden, orden.idEmpleado))

    def get_orden(self, idOrden=None, idEmpleado=None):
        sql = self.SELECT + " where idOrden=" + str(idOrden) +" and idEmpleado=" + str(idEmpleado) +";"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = self.mapear_objeto(fila)
            return o


    def save(self, orden=None):
        if orden is not None:
            if self.get_orden(orden.idOrden, orden.idEmpleado) is None:
                sql = self.INSERT
                self.gestorDB.ejecutarSQL(sql, (
                    orden.num_ticket,
                    orden.fecha,
                    orden.hora,
                    orden.lugar))
            else:
                sql = self.UPDATE
                self.gestorDB.ejecutarSQL(sql, (
                    orden.num_ticket,
                    orden.fecha,
                    orden.hora,
                    orden.lugar,
                    orden.idOrden,
                    orden.idEmpleado))

    def mapear_objeto(self, fila=None):
        if fila is None:
            return None
        else:
            o = Orden()
            o.idOrden = fila['idOrden']
            o.idEmpleado = fila['idEmpleado']
            o.num_ticket = fila['num_ticket']
            o.fecha = fila['fecha']
            o.hora = fila['hora']
            o.lugar = fila['lugar']
            return o

    def get_ordenes(self, filtro=None):
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




