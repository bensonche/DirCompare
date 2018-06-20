using DirCompare.model;
using DirCompare.src;
using System.IO;
using System.Text;
using System.Windows;
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

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            btnCompare.IsEnabled = false;

            try
            {
                if (ValidateInputs())
                    return;

                FileMetadata.checkDate = cbxDate.IsChecked.HasValue ? false : cbxDate.IsChecked.Value;
                Compare();
            }
            finally
            {
                btnCompare.IsEnabled = true;
            }
        }

        private void Compare()
        {
            Comparator comparator = new Comparator(txtDir1.Text, txtDir2.Text);

            txtResult.Text = comparator.Execute();
            txtResult1.Text = comparator.result1;
            txtResult2.Text = comparator.result2;
        }

        private bool ValidateInputs()
        {
            lblError.Text = "";

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
