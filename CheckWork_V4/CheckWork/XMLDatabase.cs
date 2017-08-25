using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CheckWork
{
    class XMLDatabase : IDatabase
    {
        static string path, // = @"C:\Users\yairi\Desktop\מחקר\Times.txt";
                      xmlPath = "database.xml";
        XElement root;

        public string Path
        {
            get
            {
                return path;
            }
        }

        #region oldFunctions
        public void createXmlFile()
        {
            root = new XElement("root");
            saveXmlFile();
        }
        public void loadXmlFile()
        {
            root = XElement.Load(xmlPath);
        }
        public void saveXmlFile()
        {
            root.Save(xmlPath);
        }
        public void clearXmlFile()
        {
            root.RemoveAll();
            saveXmlFile();
        }
        public void initXml()
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

        #region Functions
        public void addElement(WorkStamp element)
        {
            var Dt = element;
            root.Add(new XElement("Entry",
                     new XElement("StartDate", Dt.getStart()),
                     new XElement("FinishDate", Dt.getStop()),
                     new XElement("Hours", Dt.getTotalHours()
                     )));
            saveFile();
        }

        public void clearFile()
        {
            clearXmlFile();
        }

        public void createFile()
        {
            createXmlFile();
        }

        public void initFile()
        {
            initXml();
        }

        public void loadFile()
        {
            loadXmlFile();
        }

        public void removeElement(WorkStamp element)
        {
            throw new NotImplementedException();
        }

        public void saveFile()
        {
            saveXmlFile();
        }

        public string DatabaseToString(int month)
        {
            saveFile();
            DateTime startDate, finishDate;
            WorkStamp Ws = new WorkStamp();
            string output = "";
            float hours;
            try
            {
                foreach (var item in root.Elements())
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
                    output += "Total Hours Overall: " + calculateTotalHours();
                else
                    output += "Total Hours Overall: " + calculateMonthHours(month);
                output += Environment.NewLine;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return output;
        }

        public float calculateTotalHours()
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
        public float calculateMonthHours(int month)
        {
            if (month == 0)
                return calculateTotalHours();

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
        public IEnumerable<WorkStamp> elementsToList()
        {
            List<WorkStamp> list = new List<WorkStamp>();
            DateTime startDate, finishDate;
            double hours;
            foreach (XElement item in root.Elements())
            {
                startDate = DateTime.Parse(item.Element("StartDate").Value);
                finishDate = DateTime.Parse(item.Element("FinishDate").Value);
                hours = double.Parse(item.Element("Hours").Value);
                list.Add(new WorkStamp(startDate, finishDate, hours));
            }
            if (list.Count > 0)
                return list;
            return null;
        }

        #endregion

    }
}
