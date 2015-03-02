
import sqlite3
import directORM

class Producto:
    def __init__(self):

        self.idProducto = -1
        self.nombre_producto = ''
        self.precio_unidad = 0.0
        self.iva = 0
        self.foto_producto = ''
        self.ingredientes = ''
        self.familia = ''

class TbProductos:
    INSERT = '''
        insert into Productos
        ( nombre_producto, precio_unidad, iva, foto_producto, ingredientes, familia)
        values (?,?,?,?,?,?)
        '''
    DELETE = 'delete from Productos where idProducto = ?'
    SELECT = 'select * from Productos'
    UPDATE = '''
        update Productos set  
        nombre_producto = ?,
        precio_unidad = ?,
        iva = ?,
        foto_producto = ?,
        ingredientes = ?,
        familia = ?
        where  idProducto = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def remove(self, producto ):
        sql = self.DELETE
        self.gestorDB.ejecutarSQL(sql, (producto.idProducto))

    def get_producto(self, idProducto=None):
        sql = self.SELECT + " where idProducto=" + str(idProducto) +";"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = self.mapear_objeto(fila)
            return o


    def save(self, producto=None):
        if producto is not None:
            if self.get_producto(producto.idProducto) is None:
                sql = self.INSERT
                self.gestorDB.ejecutarSQL(sql, (
                    producto.nombre_producto,
                    producto.precio_unidad,
                    producto.iva,
                    producto.foto_producto,
                    producto.ingredientes,
                    producto.familia))
            else:
                sql = self.UPDATE
                self.gestorDB.ejecutarSQL(sql, (
                    producto.nombre_producto,
                    producto.precio_unidad,
                    producto.iva,
                    producto.foto_producto,
                    producto.ingredientes,
                    producto.familia,
                    producto.idProducto))

    def mapear_objeto(self, fila=None):
        if fila is None:
            return None
        else:
            o = Producto()
            o.idProducto = fila['idProducto']
            o.nombre_producto = fila['nombre_producto']
            o.precio_unidad = fila['precio_unidad']
            o.iva = fila['iva']
            o.foto_producto = fila['foto_producto']
            o.ingredientes = fila['ingredientes']
            o.familia = fila['familia']
            return o

    def get_productos(self, filtro=None):
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




