
import sqlite3
import directORM

class Horario:
    def __init__(self):

        self.idHorario = -1
        self.idEmpleado = -1
        self.dia_semana = ''
        self.hora_entrada = ''
        self.hora_salida = ''

class TbHorarios:
    INSERT = '''
        insert into Horarios
        ( dia_semana, hora_entrada, hora_salida)
        values (?,?,?)
        '''
    DELETE = 'delete from Horarios where idHorario = ? and idEmpleado = ?'
    SELECT = 'select * from Horarios'
    UPDATE = '''
        update Horarios set  
        dia_semana = ?,
        hora_entrada = ?,
        hora_salida = ?
        where  idHorario = ? and idEmpleado = ?
        '''
    def __init__(self):
        self.gestorDB = directORM.Db()

    def remove(self, horario ):
        sql = self.DELETE
        self.gestorDB.ejecutarSQL(sql, (horario.idHorario, horario.idEmpleado))

    def get_horario(self, idHorario=None, idEmpleado=None):
        sql = self.SELECT + " where idHorario=" + str(idHorario) +" and idEmpleado=" + str(idEmpleado) +";"
        fila = self.gestorDB.consultaUnicaSQL(sql)
        if fila is None:
            return None
        else: 
            o = self.mapear_objeto(fila)
            return o


    def save(self, horario=None):
        if horario is not None:
            if self.get_horario(horario.idHorario, horario.idEmpleado) is None:
                sql = self.INSERT
                self.gestorDB.ejecutarSQL(sql, (
                    horario.dia_semana,
                    horario.hora_entrada,
                    horario.hora_salida))
            else:
                sql = self.UPDATE
                self.gestorDB.ejecutarSQL(sql, (
                    horario.dia_semana,
                    horario.hora_entrada,
                    horario.hora_salida,
                    horario.idHorario,
                    horario.idEmpleado))

    def mapear_objeto(self, fila=None):
        if fila is None:
            return None
        else:
            o = Horario()
            o.idHorario = fila['idHorario']
            o.idEmpleado = fila['idEmpleado']
            o.dia_semana = fila['dia_semana']
            o.hora_entrada = fila['hora_entrada']
            o.hora_salida = fila['hora_salida']
            return o

    def get_horarios(self, filtro=None):
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




