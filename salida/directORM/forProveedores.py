
import sqlite3
import directORM

class Proveedor:
    def __init__(self):

        self.idProveedor = -1
        self.nombre = ''
        self.email = ''
        self.tlf_fijo = ''
        self.tlf_movil = ''
        self.tlf_fijo2 = ''
        self.tlf_movil2 = ''
        self.banco = ''
        self.cuenta_bancaria = ''
        self.direccion = ''
        self.foto_logo = ''

class TbProveedores:
    INSERT = '''
        insert into Proveedores
        ( nombre, email, tlf_fijo, tlf_movil, tlf_fijo2, tlf_movil2, banco, cuenta_bancaria, direccion, foto_logo)
        values (?,?,?,?,?,?,?,?,?,?)
        '''
    DELETE = 'delete from Proveedores where idProveedor = ?'
    SELECT = 'select * from Proveedores'
    UPDATE = '''
        update Proveedores set  
        nombre = ?,
        email = ?,
        tlf_fijo = ?,
        tlf_movil = ?,
        tlf_fijo2 = ?,
        tlf_movil2 = ?,
        banco = ?,
        cuenta_bancaria = ?,
        direccion = ?,
        foto_logo = ?
        where  idProveedor = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def remove(self, proveedor ):
        sql = self.DELETE
        self.gestorDB.ejecutarSQL(sql, (proveedor.idProveedor))

    def get_proveedor(self, idProveedor=None):
        sql = self.SELECT + " where idProveedor=" + str(idProveedor) +";"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = self.mapear_objeto(fila)
            return o


    def save(self, proveedor=None):
        if proveedor is not None:
            if self.get_proveedor(proveedor.idProveedor) is None:
                sql = self.INSERT
                self.gestorDB.ejecutarSQL(sql, (
                    proveedor.nombre,
                    proveedor.email,
                    proveedor.tlf_fijo,
                    proveedor.tlf_movil,
                    proveedor.tlf_fijo2,
                    proveedor.tlf_movil2,
                    proveedor.banco,
                    proveedor.cuenta_bancaria,
                    proveedor.direccion,
                    proveedor.foto_logo))
            else:
                sql = self.UPDATE
                self.gestorDB.ejecutarSQL(sql, (
                    proveedor.nombre,
                    proveedor.email,
                    proveedor.tlf_fijo,
                    proveedor.tlf_movil,
                    proveedor.tlf_fijo2,
                    proveedor.tlf_movil2,
                    proveedor.banco,
                    proveedor.cuenta_bancaria,
                    proveedor.direccion,
                    proveedor.foto_logo,
                    proveedor.idProveedor))

    def mapear_objeto(self, fila=None):
        if fila is None:
            return None
        else:
            o = Proveedor()
            o.idProveedor = fila['idProveedor']
            o.nombre = fila['nombre']
            o.email = fila['email']
            o.tlf_fijo = fila['tlf_fijo']
            o.tlf_movil = fila['tlf_movil']
            o.tlf_fijo2 = fila['tlf_fijo2']
            o.tlf_movil2 = fila['tlf_movil2']
            o.banco = fila['banco']
            o.cuenta_bancaria = fila['cuenta_bancaria']
            o.direccion = fila['direccion']
            o.foto_logo = fila['foto_logo']
            return o

    def get_proveedores(self, filtro=None):
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




