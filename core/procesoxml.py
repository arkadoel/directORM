__author__ = 'fer'
from xml.etree import ElementTree
from xml.etree.ElementTree import Element
from xml.etree.ElementTree import SubElement
from core.tableObjects import Table, Column

class Xml:

    archivo = None

    def __init__(self, ruta=None):
        self.archivo = ruta

    def get_doc(self) -> ElementTree:
        doc = ElementTree.parse(self.archivo)

        return doc

    def mapper_xml_to_objects(self):
        doc = self.get_doc()

        tablas = dict()
        tb = None

        for xmltabla in doc._root:
            #decimos a python que es un elemento
            assert isinstance(xmltabla, Element)

            #si exsiste el objeto lo eliminamos
            if tb is not None:
                del tb

            tb = Table()
            tb.table_name = xmltabla.attrib['name']

            for xmlcolumna in list(xmltabla):
                assert isinstance(xmlcolumna, Element)

                col = Column()
                col.colname = xmlcolumna.attrib['name']
                col.type = xmlcolumna.attrib['type']

                if 'key' in xmlcolumna.attrib.keys():
                    clave = bool(xmlcolumna.attrib['key'])
                    if clave is True:
                        col.is_key = True

                tb.columns.append(col)
            tablas[tb.table_name] = tb
            '''
            for nombre, objeto in tablas.items():
                assert isinstance(objeto, Table)
                print(objeto.__str__())
            '''

        return tablas

    def print_table_structure(self):
        '''
            Imprime por pantalla la estructura de la base
            de datos detectada
        '''
        doc = self.get_doc()

        for tabla in doc._root:
            #decimos a python que es un elemento
            assert isinstance(tabla, Element)
            print('Tabla: ' + tabla.attrib['name'])

            for columna in list(tabla):
                assert isinstance(columna, Element)
                print('\t' + columna.attrib['name'] +
                      '\t' + columna.attrib['type'], end='')

                if 'key' in columna.attrib.keys():
                    clave = bool(columna.attrib['key'])
                    if clave is True:
                        print('\t[clave]', end='')

                print('')
            print('\r\n')


