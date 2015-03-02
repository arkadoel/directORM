from PyQt4 import QtGui

import constantes as const

class VentanaPrincipal(QtGui.QMainWindow):

    def __init__(self):
        super(VentanaPrincipal, self).__init__(parent=None)
        self.setGeometry(100, 100, 600, 400)
        self.setWindowTitle(const.APP_NAME)




