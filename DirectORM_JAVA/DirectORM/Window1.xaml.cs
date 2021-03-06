﻿/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 26/07/2013
 * Hora: 18:16
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

using Microsoft.Win32;

namespace DirectORM
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		private Logica logica = new Logica();
		private Tabla tablaEditada = null;
		private List<String> TIPOS_DE_DATOS= new List<string>();
		
		
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
			}
		}
		
		void agregarTablaTreeView(object sender, RoutedEventArgs e)
		{
			TreeViewItem twItem = new TreeViewItem();			
			twItem.Header="t" ;
			twItem.Tag = new Tabla("t");
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
		
		private void ponerTablaEnTreeView(Tabla _t){
			TreeViewItem twItem = new TreeViewItem();			
			twItem.Header=_t.NombreTabla ;
			twItem.Tag = _t;
			NodoTablas.Items.Add(twItem);
			NodoTablas.ExpandSubtree();
		}
		
		void CargarDesdeXML(object sender, RoutedEventArgs e)
		{			
			OpenFileDialog dialogo = new OpenFileDialog();
			dialogo.ShowDialog();
			
			if(String.IsNullOrEmpty( dialogo.FileName )==false){
				string ruta = dialogo.FileName.ToString();
				string DirectorioActual = System.Windows.Forms.Application.StartupPath.ToString();
				Tabla t = null;
				ColumaTabla col = null;
				
				//eliminamos las tablas existentes
				NodoTablas.Items.Clear();
				
				//cargamos el archivo xml
				Console.WriteLine("Cargando archivo XML");
				XmlDocument xDoc = new XmlDocument();
        		xDoc.Load(Path.Combine(DirectorioActual, ruta));
        	
        		XmlNodeList nodoTablas = xDoc.GetElementsByTagName("Tablas");
	
        		foreach(XmlElement ntabla in nodoTablas[0].ChildNodes){
        			t = new Tabla(ntabla.Name.ToString());
        			Console.WriteLine("Nombre tabla: " + ntabla.Name.ToString());
        			
        			//navegar por las columnas de la tabla
        			foreach(XmlElement nColumna in ntabla.ChildNodes){
        				Console.WriteLine("\tColumna: " + nColumna.Name.ToString());
        				col = new ColumaTabla(nColumna.Name, nColumna.Attributes["tipo"].Value.ToString());
        				
        				if(nColumna.Attributes.Count >1){
        					col.isKey = true;
        				}
        				
        				t.Columnas.Add(col);
        			}
        			
        			//agregar a la lista
        			ponerTablaEnTreeView(t);
        		}
				
        		xDoc = null;
        		GC.Collect();
        		Console.WriteLine("XML cargado");
			}
			
		}
	}
}