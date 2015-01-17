
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
        ( nombre, apellidos, edad, casado)
        values (?,?,?,?)
        '''
    DELETE = 'delete from Personas where id = ?'
    SELECT = 'select * from Personas'
    UPDATE = '''
        update Personas set  
        nombre = ?,
        apellidos = ?,
        edad = ?,
        casado = ?
        where  id = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def eliminar(self, id):
        sql = self.DELETE.replace('?', str(id))
        self.gestorDB.ejecutarSQL(sql, ())

    def getPersona(self, id=None):
        sql = self.SELECT + " where id=None;"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = Persona()
            o.id = fila['id']
            o.nombre = fila['nombre']
            o.apellidos = fila['apellidos']
            o.edad = fila['edad']
            o.casado = fila['casado']
            return o

