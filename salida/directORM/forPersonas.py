
import sqlite3
import sys
import directORM

class Persona:
    def __init__(self):

        self.id = -1
        self.nombre = ''
        self.apellidos = ''
        self.edad = -1
        self.casado = False

class TbPersonas:
    INSERT = '''
        insert into Personas
        ( id, nombre, apellidos, edad, casado)
        values (?,?,?,?,?)
        '''
    DELETE = 'delete from Personas where id = ?'
    SELECT = 'select * from Personas'
    UPDATE = '''
        update Personas set ( 
        id = ?,
        nombre = ?,
        apellidos = ?,
        edad = ?,
        casado = ?)
        where  id = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.db()

    def eliminar(self, id):
        sql = self.__DELETE__.replace('?', str(id))
        self.gestorDB.ejecutarSQL(sql, ())