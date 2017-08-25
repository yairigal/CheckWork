using System;
using System.Windows;
using System.IO;
using System.Threading;

namespace CheckWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Path; // = @"C:\Users\yairi\Desktop\מחקר\Times.txt";
        DateTime[] Dt;
        uint start_stop = 0;//0 = start,1 = stop.
        public static StreamWriter file;
        StreamWriter cfgWrite;
        StreamReader cfgRead;
        string filename = "Times.txt";
        public string fileName
        {
            get { return filename; }
            set
            {
                filename = value;
                filenameTB.Text = value;
                writeToCfg(Path);
            }
        }
        Thread nUI;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                init();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// inititating all the variables.
        /// </summary>
        void init()
        {
            //adding the fucntion to the event.
            ChooseDir.ev += (sender, a) =>
            {
                fileName = a;
            };

            //creating new cfg File.
            cfgWrite = new StreamWriter("config.txt", true);
            cfgWrite.Close();


            //reading the path.
            Path = readFromCfg();
            //trying to access the Path
            try
            {
                file = new StreamWriter(Path, true);
            }
            catch (Exception)
            {
                new ChooseDir().ShowDialog();
                file = new StreamWriter(Path, true);
            }

            Dt = new DateTime[2];
            nUI = new Thread(new ThreadStart(resetTime));
            nUI.Start();
            start_stopBtn.FontSize = 15;
            count.Text = "Stopped";
            count.FontSize = 15;
            count.Foreground = System.Windows.Media.Brushes.Red;
            FileTB.IsEnabled = false;


            fileName = extractNamefromPath();

        }
        private void start_stopBtn_Click(object sender, RoutedEventArgs e)
        {
            //start_stop = 0 starting , = 1 stopping.
            Dt[start_stop] = DateTime.Now;

            start_stop = 1 - start_stop;
            switch (start_stop)
            {
                case 0://stopping.
                    start_stopBtn.Content = "Start";
                    addElement();
                    ListBox.Items.Add(Math.Round(((Dt[1] - Dt[0]).TotalHours), 2) + " Hours" + " - Registerd Successfully");
                    count.Text = "Stopped";
                    count.Foreground = System.Windows.Media.Brushes.Red;
                    break;
                case 1://starting
                    start_stopBtn.Content = "Stop";
                    count.Text = "Running";
                    count.Foreground = System.Windows.Media.Brushes.Green;
                    break;
            }
        }
        /// <summary>
        /// adding time to the txt file.
        /// </summary>
        void addElement()
        {
            string line = "";
            line += Dt[0].Day + "\\" + Dt[0].Month + "\\" + Dt[0].Year + "\t";
            line += Dt[0].Hour + ":" + Dt[0].Minute + " - " + Dt[1].Hour + ":" + Dt[1].Minute + " ";
            line += "Total: " + Math.Round(((Dt[1] - Dt[0]).TotalHours), 2) + "\n";
            file.WriteLine(line);
            file.Flush();
        }
        /// <summary>
        /// UI changing function.
        /// </summary>
        void UI()
        {
            TimeTB.Text = DateTime.Now.ToShortDateString() + "\n";
            TimeTB.Text += DateTime.Now.ToShortTimeString();
        }
        /// <summary>
        /// writes new time to the UI windows.
        /// </summary>
        void resetTime()
        {
            while (true)
            {
                Dispatcher.Invoke(UI);
                Thread.Sleep(60000);
            }

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                nUI.Abort();         
            }
            catch (Exception)
            {

            }
            Application.Current.Shutdown();
        }
        //opening new file.
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
                Path = dialog.FileName;
                file = new StreamWriter(Path, true);
                MessageBox.Show("File Directory has been changed successfully");
                fileName = dialog.SafeFileName;
            }
        }
        /// <summary>
        /// writing str to config file.
        /// </summary>
        /// <param name="str"> writing this str to config file</param>
        void writeToCfg(string str)
        {
            cfgWrite = new StreamWriter("config.txt");
            cfgWrite.WriteLine(str);
            cfgWrite.Close();
        }
        /// <summary>
        /// reading from config file
        /// </summary>
        /// <returns>returs whats in the config file</returns>
        string readFromCfg()
        {
            cfgRead = new StreamReader("config.txt");
            string s = cfgRead.ReadLine();
            cfgRead.Close();
            return s;
        }
        /// <summary>
        /// returns the file name from the path file.
        /// </summary>
        /// <returns></returns>
        string extractNamefromPath()
        {
            string name = "";
            for (int i = Path.Length - 1;  i >= 0;  i--)
            {
                if (Path[i] == '\\')
                    return name;
                name = Path[i] + name;
            }
            return "FileName.txt";
        }
    }
}
