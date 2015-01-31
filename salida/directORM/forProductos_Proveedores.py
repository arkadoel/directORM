
import sqlite3
import directORM

class Productos_Proveedor:
    def __init__(self):

        self.id = -1
        self.id_producto = -1
        self.id_proveedor = -1
        self.fecha = ''
        self.cantidad = 0.0

class TbProductos_Proveedores:
    INSERT = '''
        insert into Productos_Proveedores
        ( fecha, cantidad)
        values (?,?)
        '''
    DELETE = 'delete from Productos_Proveedores where id = ? and id_producto = ? and id_proveedor = ?'
    SELECT = 'select * from Productos_Proveedores'
    UPDATE = '''
        update Productos_Proveedores set  
        fecha = ?,
        cantidad = ?
        where  id = ? and id_producto = ? and id_proveedor = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def remove(self, productos_proveedor ):
        sql = self.DELETE
        self.gestorDB.ejecutarSQL(sql, (productos_proveedor.id, productos_proveedor.id_producto, productos_proveedor.id_proveedor))

    def get_productos_proveedor(self, id=None, id_producto=None, id_proveedor=None):
        sql = self.SELECT + " where id=" + str(id) +" and id_producto=" + str(id_producto) +" and id_proveedor=" + str(id_proveedor) +";"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = self.mapear_objeto(fila)
            return o


    def save(self, productos_proveedor=None):
        if productos_proveedor is not None:
            if self.get_productos_proveedor(productos_proveedor.id, productos_proveedor.id_producto, productos_proveedor.id_proveedor) is None:
                sql = self.INSERT
                self.gestorDB.ejecutarSQL(sql, (
                    productos_proveedor.fecha,
                    productos_proveedor.cantidad))
            else:
                sql = self.UPDATE
                self.gestorDB.ejecutarSQL(sql, (
                    productos_proveedor.fecha,
                    productos_proveedor.cantidad,
                    productos_proveedor.id,
                    productos_proveedor.id_producto,
                    productos_proveedor.id_proveedor))

    def mapear_objeto(self, fila=None):
        if fila is None:
            return None
        else:
            o = Productos_Proveedor()
            o.id = fila['id']
            o.id_producto = fila['id_producto']
            o.id_proveedor = fila['id_proveedor']
            o.fecha = fila['fecha']
            o.cantidad = fila['cantidad']
            return o

    def get_productos_proveedores(self, filtro=None):
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




