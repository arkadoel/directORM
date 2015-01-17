
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
        ( id, numero, descripcion, idPersona)
        values (?,?,?,?)
        '''
    DELETE = 'delete from Telefonos where id = ? and idPersona = ?'
    SELECT = 'select * from Telefonos'
    UPDATE = '''
        update Telefonos set ( 
        id = ?,
        numero = ?,
        descripcion = ?,
        idPersona = ?)
        where  id = ? and idPersona = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.db()    def eliminar(self, id):
        sql = self.__DELETE__.replace('?', str(id))        self.gestorDB.ejecutarSQL(sql, ())