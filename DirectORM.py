__author__ = 'fer'
__version__ = '0.0.1'

import sys

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

        if parametros[0] == '--xml':
            xml = Xml(ruta='./pruebas/db.xml')
            tablas = xml.mapper_xml_to_objects()

            tb = tablas['Personas']
            assert isinstance(tb, Table)
            print(tb.__str__())






