using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWork
{
    interface IDatabase
    {
        string Path { get; }
        void saveFile();
        void addElement(WorkStamp element);
        void removeElement(WorkStamp element);
        void createFile();
        void loadFile();
        void clearFile();
        void initFile();
        IEnumerable<WorkStamp> elementsToList();
        string DatabaseToString(int month = 0);
        float calculateTotalHours();
        float calculateMonthHours(int month);
    }
}
