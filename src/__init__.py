__author__ = 'fer'
__version__ = '0.0.1'

from PyQt4 import QtGui
from src.Ventana import vPrincipal
import sys

if __name__ == '__main__':
    app = QtGui.QApplication(sys.argv)
    v = vPrincipal()
    v.show()

    sys.exit(app.exec_())