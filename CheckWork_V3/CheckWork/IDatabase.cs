using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWork
{
    interface IDatabase
    {
        void saveFile();
        void addElement(Object element);
        void removeElement(Object element);
        void createFile();
        void loadFile();
        void clearFile();
        void initFile();
    }
}
