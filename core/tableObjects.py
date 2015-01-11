__author__ = 'fer'

class Table:

    def __init__(self):
        self.table_name = ''
        self.columns = []

    def __str__(self):
        print('Table: ' + self.table_name)
        for col in self.columns:
            assert isinstance(col, Column)
            print('\t' + col.colname +
                  '\t' + col.type, end='')

            if col.is_key is True:
                print('\t[key]', end='')
            print('')

class Column:
    def __init__(self):
        self.colname = ''
        self.is_key = False
        self.type = 'varchar'

