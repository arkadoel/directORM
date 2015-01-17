
import sqlite3
import sys
import directORM

class Telefono:
    def __init__(self):

        self.id = -1
        self.numero = -1
        self.descripcion = ''
        self.idPersona = -1

class TbTelefonos:
    INSERT = '''
        insert into Telefonos
        ( numero, descripcion)
        values (?,?)
        '''
    DELETE = 'delete from Telefonos where id = ? and idPersona = ?'
    SELECT = 'select * from Telefonos'
    UPDATE = '''
        update Telefonos set  
        numero = ?,
        descripcion = ?
        where  id = ? and idPersona = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def eliminar(self, id):
        sql = self.DELETE.replace('?', str(id))
        self.gestorDB.ejecutarSQL(sql, ())

    def getTelefono(self, id=None, idPersona=None):
        sql = self.SELECT + " where id=None, idPersona=None;"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = Telefono()
            o.id = fila['id']
            o.numero = fila['numero']
            o.descripcion = fila['descripcion']
            o.idPersona = fila['idPersona']
            return o

