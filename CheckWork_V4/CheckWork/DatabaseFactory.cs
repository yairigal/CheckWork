using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWork
{
    class DatabaseFactory
    {
        private static Func<IDatabase> currentDatabase = getJSONDatabase;
        #region Functions
        public static IDatabase getDatabase()
        {
            return currentDatabase();
        }
        private static IDatabase getXMLDatabase()
        {
            return new XMLDatabase();
        }
        private static IDatabase getJSONDatabase()
        {
            return new JSONDatabase();
        }
        #endregion
    }
}
