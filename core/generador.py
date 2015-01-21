__author__ = 'Arkadoel'

import constantes as const
import sys
import os
import shutil
from core.tableObjects import Table, Column

class Generador:
    '''
    Clase encargada de la generacion de los objetos y la salida a
    archivos de los mismos
    '''
    def __init__(self):
        self.diccionario_objetos=dict()
        self.dir_destino = const.DIRECTORIO_DESTINO
        self.comprobar_dir_destino()

    def agregar_diccionario(self, diccionario=None):
        if diccionario is not None:
            self.diccionario_objetos = diccionario

    def comprobar_dir_destino(self):
        '''
        Comprueba si existe el directorio donde se generaran los archivos,
        en caso de no existir, se creara
        :return: True or False
        '''
        try:
            print('Comprobando lugar de destino')
            self.dir_destino += '/directORM'

            if os.path.isdir(self.dir_destino) is False:
                os.makedirs(self.dir_destino)
        except:
            print(sys.exc_info()[0])
            return False

        return True

    def generar_objetos(self):
        print('Generando objetos-entidad...')
        if self.diccionario_objetos is not None \
            and self.diccionario_objetos.__len__()>0:

            try:
                print('\tDetectando __init__.py')
                archivo_init = self.dir_destino + '/__init__.py'
                if os.path.isfile(archivo_init) is True:
                    #archivo init existente, hay que modificarlo
                    os.remove(archivo_init)

                #copiamos la plantilla al directorio de destino
                print('\tCopiando plantilla __init__')
                shutil.copy2(const.TEMPLATE_INIT, archivo_init)

                self.generar_clases_objeto()

            except :
                print(sys.exc_info()[0])
                raise

    def generar_insert(self, columnas, f, nombreTabla):
        print('\tGenerando insert...')
        f.write(self.espacio(1) + 'INSERT = \'\'\'\n')
        f.write(self.espacio(2) + 'insert into ' + nombreTabla + '\n')
        columnasForWrite = self.espacio(2) + '('

        for columna in columnas:
            assert isinstance(columna, Column)
            if columna.is_key is False:
                columnasForWrite += ' ' + columna.colname + ','

        columnasForWrite = columnasForWrite[:-1] + ')\n'
        columnasForWrite += self.espacio(2) + 'values ('

        for columna in columnas:
            assert isinstance(columna, Column)
            if columna.is_key is False:
                columnasForWrite += '?,'

        columnasForWrite = columnasForWrite[:-1] + ')\n'
        f.write(columnasForWrite)
        f.write(self.espacio(2) + '\'\'\'\n')

    def put_keys_query(self, columnas, f) -> str:
        numeroKeys = 0
        txt =''
        for columna in columnas:
            assert isinstance(columna, Column)

            if columna.is_key is True:
                if numeroKeys > 0:
                    txt += ' and '
                txt += columna.colname + ' = ?'
                numeroKeys += 1
        return txt

    def generar_delete(self, columnas, f, nombreTabla):
        print('\tGenerando DELETE...')
        f.write(self.espacio(1) + 'DELETE = \'delete from ' + nombreTabla)
        f.write(' where ')

        f.write(self.put_keys_query(columnas, f))
        f.write('\'\n')

    def generar_select(self, f, nombreTabla):
        print('\tGenerando SELECT...')
        f.write(self.espacio(1) + 'SELECT = \'select * from ' +
                nombreTabla + '\'\n')

    def generar_update(self, columnas, f, nombreTabla):
        print('\tGenerando UPDATE...')
        f.write(self.espacio(1) + 'UPDATE = \'\'\'\n')
        f.write(self.espacio(2) + 'update ' + nombreTabla + ' set  \n')
        columnasForWrite = ''
        for columna in columnas:
            assert isinstance(columna, Column)
            if columna.is_key is False:
                columnasForWrite += self.espacio(2) + columna.colname + ' = ?,\n'
        columnasForWrite = columnasForWrite[:-2] + '\n'
        columnasForWrite += self.espacio(2) + 'where  '
        columnasForWrite += self.put_keys_query(columnas, f) + '\n'
        f.write(columnasForWrite)
        f.write(self.espacio(2) + '\'\'\'\n')

    def metodo_eliminar(self, f):
        print('\tGenerando metodo eliminar...')
        f.write(self.espacio(1) + 'def eliminar(self, id):\n')
        f.write(self.espacio(2) + 'sql = self.DELETE.replace(\'?\', str(id))\n')
        f.write(self.espacio(2) + 'self.gestorDB.ejecutarSQL(sql, ())\n\n')

    def metodo_getObjeto(self, columnas, f, nombreObjeto):
        print('\tProcediendo a escribir getObjeto...')
        f.write(self.espacio(1) + 'def get_' + str(nombreObjeto).lower() + '(self')
        # escribir ids
        ids = list()
        for columna in columnas:
            assert isinstance(columna, Column)
            if columna.is_key is True:
                f.write(', ' + columna.colname + '=None')
                ids.append(columna.colname)
        f.write('):\n')
        f.write(self.espacio(2) + 'sql = self.SELECT + \" where ')
        i = 0
        for ide in ids:
            if i > 0:
                f.write(' and ' + ide + '=\" + str(' + ide + ') +\"' )
            else:
                f.write(ide + '=\" + str(' + ide + ') +\"' )
            i += 1
        f.write(';\"\n')
        f.write(self.espacio(2) + 'fila = self.gestorDB.consultaUnicaSQL(sql)\n')
        f.write(self.espacio(2) + 'if fila is None:\n')
        f.write(self.espacio(3) + 'return None\n')
        f.write(self.espacio(2) + 'else: \n')
        f.write(self.espacio(3) + 'o = ' + nombreObjeto + '()\n')
        for columna in columnas:
            assert isinstance(columna, Column)
            f.write(self.espacio(3) + 'o.' + columna.colname)
            f.write(' = fila[\'' + columna.colname + '\']\n')

        f.write(self.espacio(3) + 'return o\n\n')

    def generar_clases_objeto(self):
        try:

            print('Generando clases-objeto')
            for nombreObjeto, tabla in self.diccionario_objetos.items():
                print('\r\n\t[' + nombreObjeto + ']')
                nombreTabla = self.plural(palabra=nombreObjeto)
                nombreArchivo = self.dir_destino + '/for' + nombreTabla + '.py'

                assert isinstance(tabla, Table)

                #generamos el archivo
                if os.path.isfile(nombreArchivo) is True:
                    os.remove(nombreArchivo)

                with open(nombreArchivo, 'w') as f:
                    inicio = const.FOR_IMPORTS.replace('@nombreObjeto', nombreObjeto)
                    f.write(inicio)
                    columnas = tabla.columns

                    for columna in columnas:
                        assert isinstance(columna, Column)

                        f.write(self.espacio(2) + 'self.'+ columna.colname)

                        if columna.type == 'int':
                            f.write(' = -1\n')
                        elif columna.type == 'varchar':
                            f.write(' = \'\'\n')
                        elif columna.type == 'boolean':
                            f.write(' = False\n')

                    f.write('\nclass Tb' + nombreTabla + ':\n')

                    self.generar_insert(columnas, f, nombreTabla)
                    self.generar_delete(columnas, f, nombreTabla)
                    self.generar_select(f, nombreTabla)
                    self.generar_update(columnas, f, nombreTabla)

                    f.write(self.espacio(1) + 'def __init__(self):\n')
                    f.write(self.espacio(2) + 'self.gestorDB = directORM.Db()\n\n')

                    self.metodo_eliminar(f)

                    self.metodo_getObjeto(columnas, f, nombreObjeto)

                    f.write('\n')
                    print('\tProcediendo a poner metodo Agregar...')
                    f.write(self.espacio(1) + 'def agregar(self, ' + str(nombreObjeto).lower() + '=None):\n')
                    f.write(self.espacio(2) + 'if ' + str(nombreObjeto).lower() + ' is not None:\n')
                    f.write(self.espacio(3) + 'if self.get_' + str(nombreObjeto).lower() + '(' )
                    ids = list()
                    for columna in columnas:
                        assert isinstance(columna, Column)
                        if columna.is_key is True:
                            ids.append(columna.colname)

                    i = 0
                    for ide in ids:
                        if i > 0:
                            f.write(', ' + str(nombreObjeto).lower() + '.' + ide )
                        else:
                            f.write(str(nombreObjeto).lower() + '.' + ide )
                        i += 1
                    f.write(') is None:\n')

                    f.write(self.espacio(4) + 'sql = self.INSERT\n')
                    f.write(self.espacio(4) + 'self.gestorDB.ejecutarSQL(sql, (\n')
                    i = 0
                    for columna in columnas:
                        assert isinstance(columna, Column)

                        if columna.is_key is False:
                            f.write(self.espacio(5) + str(nombreObjeto).lower() + '.' + columna.colname)

                            if (i+1) < len(columnas):
                                f.write(',\n')

                        i += 1

                    f.write('))\n')


                    f.write(self.espacio(3) + 'else:\n')

                    f.write(self.espacio(4) + 'sql = self.UPDATE\n')
                    f.write(self.espacio(4) + 'self.gestorDB.ejecutarSQL(sql, (\n')
                    i = 0
                    for columna in columnas:
                        assert isinstance(columna, Column)
                        i += 1
                        if columna.is_key is False:
                            f.write(self.espacio(5) + str(nombreObjeto).lower() + '.' + columna.colname)

                            if i < len(columnas):
                                f.write(',\n')

                        if i >= len(columnas):
                            f.write(',\n')
                            j = 0
                            for ide in ids:
                                if j > 0:
                                    f.write(',\n' + self.espacio(5) + str(nombreObjeto).lower() + '.' + ide )
                                else:
                                    f.write(self.espacio(5) + str(nombreObjeto).lower() + '.' + ide )
                                j += 1

                            f.write('))\n')










                    f.close()
        except:
            print(sys.exc_info()[0])
            raise

    def plural(self, palabra=None):
        if palabra is not None:
            final = palabra[-1]
            #print('Letra final: ' + final)
            if final == 'a' or final == 'e' \
                    or final == 'i' or final == 'o' \
                    or final == 'u':
                palabra += 's'
            else:
                palabra += 'es'

            return palabra

    def espacio(self, numero=None):
        txt = ''
        if numero is None:
            numero = 1

        for n in range(numero):
            txt += '    '

        return txt