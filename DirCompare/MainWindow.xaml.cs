﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace DirCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDir1_Click(object sender, RoutedEventArgs e)
        {
            DirectoryDialog(txtDir1);
        }

        private void btnDir2_Click(object sender, RoutedEventArgs e)
        {
            DirectoryDialog(txtDir2);
        }

        private void DirectoryDialog(System.Windows.Controls.TextBox textbox)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    textbox.Text = dialog.SelectedPath;
                }
            }
        }
    }
}