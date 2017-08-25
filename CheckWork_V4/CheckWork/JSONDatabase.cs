using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace CheckWork
{
    class JSONDatabase : IDatabase
    {
        private static string path = @"database.json";
        StreamWriter wrt;

        public string Path
        {
            get
            {
                return path;
            }
        }

        public void addElement(WorkStamp element)
        {
            string jsonObject = JsonConvert.SerializeObject(element,Formatting.Indented);
            writeToFile(jsonObject);
        }

        public float calculateMonthHours(int month)
        {
            if (month == 0)
                return calculateTotalHours();

            float sum = 0;
            float curHours;
            DateTime cur = new DateTime();
            foreach (var item in elementsToList())
            {
                cur = item.getStart();
                curHours = (float)item.getTotalHours();
                if (cur.Month == month)
                    sum += curHours;
            }
            return sum;
        }

        public float calculateTotalHours()
        {
            float sum = 0;
            foreach (var item in elementsToList())
                sum += (float)item.getTotalHours();
            return sum;
        }

        public void clearFile()
        {
            wrt = new StreamWriter(path);
            wrt.Write("[]");
            wrt.Close();
        }

        public void createFile()
        {
            wrt = new StreamWriter(path);
            wrt.Close();
        }

        public string DatabaseToString(int month = 0)
        {
            saveFile();
            DateTime startDate, finishDate;
            WorkStamp Ws = new WorkStamp();
            string output = "";
            float hours;
            try
            {
                foreach (var item in elementsToList())
                {
                    startDate = item.getStart();
                    finishDate = item.getStop();
                    hours = (float)item.getTotalHours();
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

        public IEnumerable<WorkStamp> elementsToList()
        {
            StreamReader rd = new StreamReader(path);
            string allJSONobjects = rd.ReadToEnd();
            rd.Close();
            if(allJSONobjects == "")
                return new List<WorkStamp>();
            try
            {
                WorkStamp[] list = JsonConvert.DeserializeObject<WorkStamp[]>(allJSONobjects);
                return list;
            }
            catch (Exception)
            {

                return new List<WorkStamp>();
            }


        }

        public void initFile()
        {
            try
            {
                //load file
                StreamReader rd;
                rd = new StreamReader(path);
                rd.Close();
            }
            catch (Exception)
            {
                clearFile();
            }
        }

        public void loadFile()
        {
            // no need to load 
        }

        public void removeElement(WorkStamp element)
        {
            throw new NotImplementedException();
        }

        public void saveFile()
        {
            //no need for save becuase after writing it is saved.
        }

        private void writeToFile(string file)
        {
            StreamReader rd = new StreamReader(path);
            string allFile = rd.ReadToEnd();
            allFile = allFile.Substring(0,allFile.Length - 1);
            rd.Close();

            if (allFile != "[")
                allFile += ",\n";


            allFile += file;
            allFile += "]";
            StreamWriter wrt = new StreamWriter(path);
            wrt.Write(allFile);
            wrt.Close();
        }
    }
}
