__author__ = 'Arkadoel'
import sys
import sqlite3

class Db:
    DB_PATH = ''
    conexion = None

    def __init__(self):
        self.conectar()

    def conectar(self):
        '''
        Abre la conexion con la base de datos
        :return:
        '''
        try:
            self.conexion = sqlite3.connect(self.DB_PATH)

            #para evitar errores TypeError: tuple indices must be integers, not str
            self.conexion.row_factory = sqlite3.Row # its key
        except sqlite3.Error as e:
            print("Error %s:" % e.args[0])
            sys.exit(1)

    def ejecutarSQL(self, sql, parametros):
        '''
        Ejecuta una sentencia SQL y devuelve el numero de filas afectadas
        :param sql:
        :param parametros: sentencia sql, parametros
        :return: int
        '''
        self.conectar()
        cur = self.conexion.cursor()
        print(sql, parametros)
        n = cur.execute(sql, parametros)
        self.conexion.commit()
        self.cerrarDB()
        return n

    def consultaSQL(self, sql):
        '''
        Ejecuta una sentencia SQL y devuelve la salida de
        fetchall()
        :param sql:
        :return:
        '''
        self.conectar()
        cur = self.conexion.cursor()
        print(sql)
        cur.execute(sql)
        filas = cur.fetchall()
        self.cerrarDB()
        return filas

    def consultaUnicaSQL(self, sql):
        '''
        Devuelve el primer resultado de una consulta
        :param sql:
        :return:
        '''
        self.conectar()
        cur = self.conexion.cursor()
        print(sql)
        try:
            cur.execute(sql)
            unico = cur.fetchone()
        except:
            print('Error al ejecutar la consulta unica')
        self.cerrarDB()
        return unico

    def cerrarDB(self):
        '''
        Se asegura de cerrar la base de datos
        :return:
        '''
        if self.conexion is not None:
            self.conexion.close()
            self.conexion = None
