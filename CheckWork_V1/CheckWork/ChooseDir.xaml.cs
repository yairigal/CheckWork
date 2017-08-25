using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace CheckWork
{
    /// <summary>
    /// Interaction logic for ChooseDir.xaml
    /// </summary>
    public partial class ChooseDir : Window
    {
        public static EventHandler<string> ev;

        public ChooseDir()
        {
            InitializeComponent();
        }


        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                //if its not a txt file.
                if (!(dialog.SafeFileName.EndsWith("txt")))
                {
                    MessageBox.Show("Please Select a txt File");
                    return;
                }
                FileTB.Text = dialog.SafeFileName;
                MainWindow.Path = dialog.FileName;
                //MainWindow wdw = new MainWindow();
                ev.Invoke(this, dialog.SafeFileName);
                this.Close();
            }
        }
    }
}
