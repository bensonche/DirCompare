using System;
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
using System.IO;

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

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            lblError.Text = "";

            if (!ValidateInputs())
                return;
        }

        private bool ValidateInputs()
        {
            bool hasError = false;
            StringBuilder errorMsg = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtDir1.Text))
            {
                errorMsg.AppendLine("Directory 1 is required");
                hasError = true;
            }
            else if (!Directory.Exists(txtDir1.Text))
            {
                errorMsg.AppendLine("Directory 1 is invalid");
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(txtDir2.Text))
            {
                errorMsg.AppendLine("Directory 2 is required");
                hasError = true;
            }
            else if (!Directory.Exists(txtDir2.Text))
            {
                errorMsg.AppendLine("Directory 2 is invalid");
                hasError = true;
            }

            if (!hasError && txtDir1.Text == txtDir2.Text)
            {
                errorMsg.AppendLine("Cannot compare the same directories");
                hasError = true;
            }

            if (hasError)
                lblError.Text = errorMsg.ToString();

            return hasError;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            txtDir1.Focus();
        }
    }
}
