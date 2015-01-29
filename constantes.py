
from _datetime import datetime

#constantes de la aplicacion
APP_NAME = 'directORM'
APP_VERSION = '0.0.1'

DIRECTORIO_DESTINO = './salida'
TEMPLATE_INIT = './templates/__init__.py'

FOR_IMPORTS = '''
import sqlite3
import sys
import directORM

class @nombreObjeto:
    def __init__(self):

'''

def get_hora() -> str:
    '''
    Devuelve la hora en forma to hh:mm:ss
    :return:
    '''
    horas = datetime.now().hour
    minutos = datetime.now().minute
    segundos = datetime.now().second
    resultado = ''

    if horas <10:
        resultado += '0' + str(horas)
    else:
        resultado += str(horas)

    resultado += ':'

    if minutos <10:
        resultado += '0' + str(minutos)
    else:
        resultado += str(minutos)

    resultado += ':'
    if segundos <10:
        resultado += '0' + str(segundos)
    else:
        resultado += str(segundos)

    return resultado


def dia_semana() -> str:
    '''
    Devuelve el dia de la semana en el que estamos
    :rtype : basestring
    :return: String
    '''
    dias = ['Lunes',
            'Martes',
            'Miercoles',
            'Jueves',
            'Viernes',
            'Sabado',
            'Domingo']
    return dias[datetime.today().weekday()]

def get_fecha():
    fecha = dia_semana() + ', '
    fecha += datetime.today().date().__str__() + ' '
    fecha += get_hora()
    return fecha


