__author__ = 'fer'
__version__ = '0.0.1'

import sys
import os
from core.generador import Generador
from core.procesoxml import Xml
from core.tableObjects import Table

from PyQt4 import QtGui
from gui.v_principal import VentanaPrincipal

def abrir_ventana():
    app = QtGui.QApplication(sys.argv)
    v = VentanaPrincipal()
    v.show()

    sys.exit(app.exec_())

if __name__ == '__main__':
    nparametros = len(sys.argv)

    if nparametros== 1:
        abrir_ventana()
    elif nparametros >1:
        parametros = sys.argv[1:]

        if parametros[0] == '--xml'  and nparametros >1:
            rutaXml = str(parametros[1])
            if os.path.isfile(rutaXml) is True:
                xml = Xml(ruta=rutaXml)
                tablas = xml.mapper_xml_to_objects()

                gen = Generador()
                gen.agregar_diccionario(diccionario=tablas)
                gen.generar_objetos()









