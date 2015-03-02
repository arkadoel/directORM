
import sqlite3
import directORM

class Productos_en_Orden:
    def __init__(self):

        self.idOrden = -1
        self.idProducto = -1
        self.cantidad = 0.0

class TbProductos_en_Ordenes:
    INSERT = '''
        insert into Productos_en_Ordenes
        ( cantidad)
        values (?)
        '''
    DELETE = 'delete from Productos_en_Ordenes where idOrden = ? and idProducto = ?'
    SELECT = 'select * from Productos_en_Ordenes'
    UPDATE = '''
        update Productos_en_Ordenes set  
        cantidad = ?
        where  idOrden = ? and idProducto = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def remove(self, productos_en_orden ):
        sql = self.DELETE
        self.gestorDB.ejecutarSQL(sql, (productos_en_orden.idOrden, productos_en_orden.idProducto))

    def get_productos_en_orden(self, idOrden=None, idProducto=None):
        sql = self.SELECT + " where idOrden=" + str(idOrden) +" and idProducto=" + str(idProducto) +";"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = self.mapear_objeto(fila)
            return o


    def save(self, productos_en_orden=None):
        if productos_en_orden is not None:
            if self.get_productos_en_orden(productos_en_orden.idOrden, productos_en_orden.idProducto) is None:
                sql = self.INSERT
                self.gestorDB.ejecutarSQL(sql, (
                    productos_en_orden.cantidad))
            else:
                sql = self.UPDATE
                self.gestorDB.ejecutarSQL(sql, (
                    productos_en_orden.cantidad,
                    productos_en_orden.idOrden,
                    productos_en_orden.idProducto))

    def mapear_objeto(self, fila=None):
        if fila is None:
            return None
        else:
            o = Productos_en_Orden()
            o.idOrden = fila['idOrden']
            o.idProducto = fila['idProducto']
            o.cantidad = fila['cantidad']
            return o

    def get_productos_en_ordenes(self, filtro=None):
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




