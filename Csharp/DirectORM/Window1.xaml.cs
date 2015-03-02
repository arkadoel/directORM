/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 26/07/2013
 * Hora: 18:16
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DirectORM
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		private Logica logica = new Logica();
		private Tabla tablaEditada = null;
		private List<string> TIPOS_DE_DATOS= new List<string>();
		
		
		public Window1()
		{
			InitializeComponent();
			TIPOS_DE_DATOS.Add("varchar");
			TIPOS_DE_DATOS.Add("int");
			TIPOS_DE_DATOS.Add("datetime");
			TWTablas.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(TWTablas_SelectedItemChanged);
		}

		void TWTablas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			TreeViewItem twitem=TWTablas.SelectedItem as TreeViewItem;
			
			if(twitem.Tag!=null)
			{
				tablaEditada = twitem.Tag as Tabla;
				txtNombreTabla.Text = tablaEditada.NombreTabla;
				dgridColumnas.ItemsSource = tablaEditada.Columnas;
				combo.ItemsSource = TIPOS_DE_DATOS;
			}
		}
		
		void agregarTablaTreeView(object sender, RoutedEventArgs e)
		{
			TreeViewItem twItem = new TreeViewItem();			
			twItem.Header="Nacimiento" ;
			
			twItem.Tag = new Tabla("Nacimiento");
			NodoTablas.Items.Add(twItem);
			NodoTablas.ExpandSubtree();
		}
		
		void quitarTablaTreeView(object sender, RoutedEventArgs e)
		{
			NodoTablas.Items.Remove(TWTablas.SelectedItem);
		}
		
		void guardarCambiosTabla(object sender, RoutedEventArgs e)
		{
			tablaEditada.NombreTabla = txtNombreTabla.Text;
			TreeViewItem twitem=TWTablas.SelectedItem as TreeViewItem;
			twitem.Tag = tablaEditada;
			twitem.Header = txtNombreTabla.Text;
		}
		
		void btnGenerar_Click(object sender, RoutedEventArgs e)
		{
			logica.Tablas.Clear();
			logica.NAMESPACE = txtNamespace.Text;
			
			foreach(TreeViewItem tw in NodoTablas.Items){
				logica.Tablas.Add(tw.Tag as Tabla);
			}
			
			VentanaResultado v = new VentanaResultado();			
			logica.ventana = v;
			logica.procesar();
			v.Show();
			
			
			
			MessageBox.Show("Proceso terminado");
		}
	}
}