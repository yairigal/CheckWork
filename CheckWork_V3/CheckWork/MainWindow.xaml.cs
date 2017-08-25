using System;
using System.Windows;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace CheckWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables 
        public enum Months
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }
        int start_stop = 0;//0 = start,1 = stop.
        static string Path, // = @"C:\Users\yairi\Desktop\מחקר\Times.txt";
                      xmlPath = "database.JSON";
        //DateTime[] Dt;
        WorkStamp Dt;
        StreamWriter file;
        XElement root;
        Thread nUI,timeCountThread;
        double timeCount = 0;
        bool timeCountThreadFlag = true,
             UIThreadFlag = true;
        #endregion
        //CTOR
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
           
        #region Initials
        void init()
        {
            initVars();
            initXml();
            initMonthCombobox();
        }
        void initVars()
        {
            Dt = new WorkStamp();
            nUI = new Thread(new ThreadStart(resetTime));
            nUI.Start();
            //start_stopBtn.FontSize = 15;
            count.Text = "Stopped";
            count.FontSize = 15;
            count.Foreground = System.Windows.Media.Brushes.Red;
            FileTB.IsEnabled = false;
        }
        void initXml()
        {
            try
            {
                loadXmlFile();
            }
            catch (Exception)
            {
                createXmlFile();
            }


        }
        #endregion
        #region Events
        //private void start_stopBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    //start_stop = 0 starting , = 1 stopping.
        //    Dt[start_stop] = DateTime.Now;

        //    start_stop = 1 - start_stop;
        //    switch (start_stop)
        //    {
        //        case 0://stopping.
        //            start_stopBtn.Content = this.FindResource("start");

        //            addElement();
        //            ListBox.Items.Add("Start: " + Dt[0].ToShortTimeString() + " - " + Math.Round(((Dt[1] - Dt[0]).TotalHours), 2) + " Hours" + " - Registerd Successfully");
        //            count.Text = "Stopped";
        //            count.Foreground = System.Windows.Media.Brushes.Red;
        //            break;
        //        case 1://starting
        //            start_stopBtn.Content = this.FindResource("stop");
        //            count.Text = "Running";
        //            count.Foreground = System.Windows.Media.Brushes.Green;
        //            break;
        //    }
        //}
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                UIThreadFlag = false;
            }
            catch (Exception)
            {

            }
            Application.Current.Shutdown();
            saveXmlFile();
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
                ImportBtn.IsEnabled = true;
            }
        }
        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //   TotalTextBox.Text = "Total Hours Overall: "+ calcTotal().ToString();
        //}
        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For JAN: " + calcMonth(1).ToString();
        //}
        //private void button2_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For FEB: " + calcMonth(2).ToString();
        //}
        //private void button3_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For MAR: " + calcMonth(3).ToString();
        //}
        //private void button4_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For APR: " + calcMonth(4).ToString();
        //}
        //private void button5_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For MAY: " + calcMonth(5).ToString();
        //}
        //private void button6_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For JUN: " + calcMonth(6).ToString();
        //}
        //private void button7_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For JUL: " + calcMonth(7).ToString();
        //}
        //private void button8_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For AUG: " + calcMonth(8).ToString();
        //}
        //private void button10_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For OCT: " + calcMonth(10).ToString();
        //}
        //private void button11_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For NOV: " + calcMonth(11).ToString();
        //}
        //private void button12_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For DEC: " + calcMonth(12).ToString();
        //}
        //private void button9_Click(object sender, RoutedEventArgs e)
        //{
        //    TotalTextBox.Text = "Total Hours For SEP: " + calcMonth(9).ToString();
        //}
        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            int month = monthComboBox.SelectedIndex;
            importToTxtFile(file, month);
            ImportBtn.IsEnabled = false;
            MessageBox.Show("Export Succeeded");
            FileTB.Text = "Browse";
            file.Close();
        }
        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult ans = MessageBox.Show("This will delete the database permenetly", "Clear Database", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (ans == MessageBoxResult.Yes)
            {
                clearXmlFile();
                MessageBox.Show("Succesfully Cleard the Database");
            }

            refreshShowBox();
        }
        private void richTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
        private void monthComboBox_DropDownClosed(object sender, EventArgs e)
        {
            refreshShowBox();
        }
        private void start_stopBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            start_stop = Dt.startStop();
            switch (start_stop)
            {
                case 0://stopping.
                    //resetting the Time count.
                    startStopTimeCount("stop");
                    start_stopBtn.Source = new BitmapImage(new Uri(@"/icon/start.png", UriKind.Relative));
                    addElement();
                    ListBox.Items.Add("Start: " + Dt.getStart().ToShortTimeString() + " - " + Dt.getTotalHours() + " Hours" + " - Registerd Successfully");
                    count.Text = "Stopped";
                    this.Title = "CheckWork - Stopped";
                    count.Foreground = System.Windows.Media.Brushes.Red;
                    break;
                case 1://starting
                    //starting the time count.
                    startStopTimeCount("start");
                    start_stopBtn.Source = new BitmapImage(new Uri(@"/icon/stop.png", UriKind.Relative));
                    count.Text = "Running";
                    this.Title = "CheckWork - Running"; //changed in TimeCountThread.
                    count.Foreground = System.Windows.Media.Brushes.Green;
                    break;
            }
        }
        #endregion
        #region Help Functions
        /// <summary>
        /// adding time to the xml file.
        /// </summary>
        void addElement()
        {
            //string line = "";
            //line += Dt[0].Day + "\\" + Dt[0].Month + "\\" + Dt[0].Year + "\t";
            //line += Dt[0].Hour + ":" + Dt[0].Minute + " - " + Dt[1].Hour + ":" + Dt[1].Minute + " ";
            //line += "Total: " + Math.Round(((Dt[1] - Dt[0]).TotalHours), 2) + "\n";
            //file.WriteLine(line);
            //file.Flush();

            root.Add(new XElement("Entry",
                new XElement("StartDate", Dt.getStart()),
                new XElement("FinishDate", Dt.getStop()),
                new XElement("Hours", Dt.getTotalHours()
                )));
            saveXmlFile();
            refreshShowBox();
        }
        /// <summary>
        /// UI changing function.
        /// </summary>
        void UI()
        {
            TimeTB.Text = DateTime.Now.ToShortDateString() + "\n";
            TimeTB.Text += DateTime.Now.ToShortTimeString();
        }
        void createXmlFile()
        {
            root = new XElement("root");
            saveXmlFile();
        }
        void loadXmlFile()
        {
            root = XElement.Load(xmlPath);
        }
        void saveXmlFile()
        {
            root.Save(xmlPath);
        }
        void importToTxtFile(StreamWriter file, int month = 0)
        {
            string output = DatabaseToString(month);
            file.WriteLine(output);
        }
        /// <summary>
        ///  returns a string with all the times in string format
        /// </summary>
        /// <param name="month">optional if wanna get string for only specific month</param>
        /// <returns>returns the string for the current month.</returns>
        string DatabaseToString(int month = 0)
        {
            saveXmlFile();
            DateTime startDate, finishDate;
            WorkStamp Ws = new WorkStamp();
            string output = "";
            float hours;
            try
            {
                foreach (XElement item in root.Elements())
                {
                    startDate = DateTime.Parse(item.Element("StartDate").Value);
                    finishDate = DateTime.Parse(item.Element("FinishDate").Value);
                    hours = float.Parse(item.Element("Hours").Value);
                    if (month == 0 || startDate.Month == month)
                    {
                        //file.WriteLine(startDate.ToShortDateString() + "\t" + startDate.ToShortTimeString() + " - " + finishDate.ToShortTimeString() + "\t Total: " + hours + " Hours");
                        //file.Flush();
                        output += startDate.ToShortDateString() + "\t" + startDate.ToShortTimeString() + " - " + finishDate.ToShortTimeString() + "\t Total: " + hours + " Hours";
                        output += Environment.NewLine;
                    }
                }
                //file.WriteLine("Total Hours Overall: " + calcTotal());
                //file.Close();
                if (month == 0)
                    output += "Total Hours Overall: " + calcTotal();
                else
                    output += "Total Hours Overall: " + calcMonth(month);
                output += Environment.NewLine;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return output;
        }
        void addToShowBox(int month)
        {
            List<WorkStamp> list = new List<WorkStamp>();
            saveXmlFile();
            DateTime startDate, finishDate;
            WorkStamp Ws;
            double hours;
            try
            {
                foreach (XElement item in root.Elements())
                {
                    startDate = DateTime.Parse(item.Element("StartDate").Value);
                    finishDate = DateTime.Parse(item.Element("FinishDate").Value);
                    hours = double.Parse(item.Element("Hours").Value);
                    Ws = new WorkStamp(startDate, finishDate, hours);
                    if (month == 0 || startDate.Month == month)
                        showTextBox.Items.Add(Ws);              
                }
                //file.WriteLine("Total Hours Overall: " + calcTotal());
                //file.Close()
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        void clearXmlFile()
        {
            root.RemoveAll();
            saveXmlFile();
        }
        float calcTotal()
        {
            float sum = 0;
            foreach (XElement item in root.Elements())
                sum += float.Parse(item.Element("Hours").Value);
            return sum;
        }
        /// <summary>
        ///  returns the total time for a month
        /// </summary>
        /// <param name="month"> can be 1 to 12</param>
        /// <returns> sum of hours for a specific month</returns>
        float calcMonth(int month)
        {
            if (month == 0)
                return calcTotal();

            float sum = 0;
            float curHours;
            DateTime cur = new DateTime();
            foreach (XElement item in root.Elements())
            {
                cur = DateTime.Parse(item.Element("StartDate").Value);
                curHours = float.Parse(item.Element("Hours").Value);
                if (cur.Month == month)
                    sum += curHours;
            }
            return sum;
        }
        void initMonthCombobox()
        {
            monthComboBox.Items.Add("All Time");
            for (int i = 1; i <= 12; i++)
                monthComboBox.Items.Add(i + " - " + (Months)i);
            refreshShowBox();
        }
        void refreshShowBox()
        {
            showTextBox.Items.Clear();
            addToShowBox(monthComboBox.SelectedIndex);
            refreshTotalTime(monthComboBox.SelectedIndex);
        }
        void refreshTotalTime(int month = 0)
        {
            TotalTimeTB.Text = calcMonth(month) + " Hours";
        }  
        void resetTime()//thread
        {
            int i = 1;
            Dispatcher.Invoke(UI);
            while (UIThreadFlag)
            {
                /*adding the if to make the while loop check the UIThreadFlag more often 
                 * (every sec instead of every 60 sec incase of terminate)
                 * so if the application is closed , this thread will shut himself after 1 sec.*/
                if(i++ % 60 == 0)
                {
                    Dispatcher.Invoke(UI);
                    i = 1;
                }           
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// UI functions.
        /// </summary>
        void TimeCountRefresh()
        {
            timeCount += 0.01;
            TimeCountTB.Text = Math.Round(timeCount,2)+" Work Hours";
            this.Title = "CheckWork - Running - " + Math.Round(timeCount, 2);
        }
        /// <summary>
        /// infinite Loop with dispacther.
        /// </summary>
        void timeCountRefresh()
        {
            int i = 1;
            while (timeCountThreadFlag)
            {
                Thread.Sleep(1000);
                if (i++ % 36 == 0)
                {// doing this to make the while loop check the timeCountThreadFlag Faster (every 1 sec instead of every 36 secs) so if its terminated it will close.                   
                    Dispatcher.Invoke(TimeCountRefresh);
                    i = 1;
                }
            }
        }
        void startStopTimeCount(string startstop)
        {
            switch (startstop)
            {
                case "start":
                    timeCountThreadFlag = true;
                    timeCountThread = new Thread(new ThreadStart(timeCountRefresh));
                    timeCountThread.Start();
                    break;
                case "stop":
                    timeCountThreadFlag = false;
                    timeCount = 0;
                    TimeCountTB.Text = "";
                    break;
                default:
                    MessageBox.Show("Input Error Start Stop");
                    break;
            }
        }
        #endregion
    }
}
