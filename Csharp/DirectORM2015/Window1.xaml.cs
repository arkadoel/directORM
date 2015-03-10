/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 01/03/2015
 * Hora: 10:28
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using DirectORM;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace DirectORM2015
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		//private Logica logica = new Logica();
		private DirectORM.Table tablaEditada = null;
		private List<string> TIPOS_DE_DATOS= new List<string>();
		private Logica logica = null;
        private String FicheroXML = "";
		
		public Window1()
		{
			InitializeComponent();
			TIPOS_DE_DATOS.Add("varchar");
			TIPOS_DE_DATOS.Add("int");
			TIPOS_DE_DATOS.Add("datetime");
            TIPOS_DE_DATOS.Add("float");
			TWTablas.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(TWTablas_SelectedItemChanged);
		}

		void TWTablas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			TreeViewItem twitem=TWTablas.SelectedItem as TreeViewItem;
			
			if(twitem.Tag!=null)
			{
				tablaEditada = twitem.Tag as Table;
				txtNombreTabla.Text = tablaEditada.TableName;
				dgridColumnas.ItemsSource = tablaEditada.Cols;
				combo.ItemsSource = TIPOS_DE_DATOS;
			}
		}
		
		void agregarTablaTreeView(object sender, RoutedEventArgs e)
		{
			TreeViewItem twItem = new TreeViewItem();			
			twItem.Header="Tabla" ;
			
			twItem.Tag = new Table("Tabla");
			NodoTablas.Items.Add(twItem);
			NodoTablas.ExpandSubtree();
		}
		
		public void agregarTabla(Table t)
		{
			TreeViewItem twItem = new TreeViewItem();
            PonerNombreTablaIcono(t.TableName, twItem);
			
			twItem.Tag = t;
			NodoTablas.Items.Add(twItem);
			NodoTablas.ExpandSubtree();
		}

        private void PonerNombreTablaIcono(String nombre, TreeViewItem twItem)
        {
            Image imagen = new Image();
            imagen.Height = 20;
            imagen.Width = 20;
            imagen.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            imagen.Source = PonerIcono("pack://siteoforigin:,,,/Resources/Table-icon.png");
            Label lblTabla = new Label();
            lblTabla.Content = nombre;

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(imagen);
            panel.Children.Add(lblTabla);
            twItem.Header = panel;
        }
		
		void quitarTablaTreeView(object sender, RoutedEventArgs e)
		{
			NodoTablas.Items.Remove(TWTablas.SelectedItem);
		}
		
		void guardarCambiosTabla(object sender, RoutedEventArgs e)
		{
			tablaEditada.TableName = txtNombreTabla.Text;
			TreeViewItem twitem=TWTablas.SelectedItem as TreeViewItem;
			twitem.Tag = tablaEditada;
            PonerNombreTablaIcono(txtNombreTabla.Text, twitem);

            if (FicheroXML != "")
            {
                GuardarArchivoXML(FicheroXML);
            }
		}

        private ImageSource PonerIcono(string path)
        {
            return new BitmapImage(new Uri(path));
        }
		
		void btnGenerar_Click(object sender, RoutedEventArgs e)
		{
            switch (cmbMotor.Text.ToString().ToUpper())
            {
                case "SQLCLIENT":
                    logica = new Logica(Logica.MOTORES.SQL_CLIENT);
                    break;
                case "OLEDB":
                    logica = new Logica(Logica.MOTORES.OLE_DB);
                    break;
            }
            
			logica.Tablas.Clear();
			logica.NAMESPACE = txtNamespace.Text;
			
			foreach(TreeViewItem tw in NodoTablas.Items){
				logica.Tablas.Add(tw.Tag as Table);
			}
			
			VentanaResultado v = new VentanaResultado();			
			logica.ventana = v;
			logica.procesar();
			v.Show();		
			
			MessageBox.Show("Proceso terminado");
		}
		
		void GuardaXML(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialogo = new SaveFileDialog();
			dialogo.Filter = "xml |*.xml";
			dialogo.ShowDialog();
			
			if(string.IsNullOrWhiteSpace( dialogo.FileName) == false)
			{
                GuardarArchivoXML(dialogo.FileName);
				MessageBox.Show("Exportacion terminada","Terminado", 
				                MessageBoxButton.OK, 
				                MessageBoxImage.Information);
			}
		}

        /// <summary>
        /// Exporta automaticamente al fichero que se esta usando
        /// </summary>
        /// <param name="destino"></param>
        private void GuardarArchivoXML(string destino)
        {
            if (System.IO.File.Exists(destino) == true)
            {
                List<Table> tablas = new List<Table>();
                foreach (TreeViewItem tw in NodoTablas.Items)
                {
                    tablas.Add(tw.Tag as Table);
                }

                gestorXML.ExportarXML(tablas, destino);
            }
        }
		
		void ImportaXML(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialogo = new OpenFileDialog();
			dialogo.Filter = "xml |*.xml";
			dialogo.ShowDialog();
			
			if(string.IsNullOrWhiteSpace( dialogo.FileName) == false)
			{
				NodoTablas.Items.Clear();
				List<Table> tablas = gestorXML.ImportarXML(dialogo.FileName);
				foreach(var tabla in tablas)
				{
					agregarTabla(tabla);
				}

                FicheroXML = dialogo.FileName;
			}
			
		}
	}
}