
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

