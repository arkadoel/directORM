using Microsoft.Win32;
/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 07/26/2013
 * Hora: 20:29
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
	/// Interaction logic for VentanaResultado.xaml
	/// </summary>
	public partial class VentanaResultado : Window
	{
		public VentanaResultado()
		{
			InitializeComponent();
		}

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialogo = new SaveFileDialog();
            dialogo.Filter = ".cs | *.cs";
            dialogo.ShowDialog();

            if (string.IsNullOrWhiteSpace(dialogo.FileName) == false)
            {
                System.IO.StreamWriter fich = new System.IO.StreamWriter(dialogo.FileName, false);
                fich.WriteLine(txtResultado.Text);
                fich.Close();
            }
        }
	}
}