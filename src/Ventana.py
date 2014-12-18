
from PyQt4 import QtGui
import src.Constantes as const

class vPrincipal(QtGui.QMainWindow):

    def __init__(self):
        super(vPrincipal, self).__init__(parent=None)
        self.setGeometry(100, 100, 600, 400)
        self.setWindowTitle(const.APP_NAME)
