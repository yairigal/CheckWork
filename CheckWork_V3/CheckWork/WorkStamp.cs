using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWork
{
    class WorkStamp
    {
        private DateTime startT, stopT;
        double totalWorkTime;//hours
        bool running = false;

        public WorkStamp()
        {
            startT = new DateTime();
            stopT = new DateTime();
            totalWorkTime = 0;
        }
        public WorkStamp(DateTime start,DateTime stop,double hours)
        {
            startT = start;
            stopT = stop;
            totalWorkTime = hours;
        }

        public DateTime getStart()
        {
            return startT;
        }
        public DateTime getStop()
        {
            return stopT;
        }
        private void start()
        {
            startT = DateTime.Now;
            running = true;
        }
        private void stop()
        {
            stopT = DateTime.Now;
            totalWorkTime = Math.Round(((stopT - startT).TotalHours), 2);
            running = false;
        }
        public int startStop()
        {
            //if we need to start now
            if(!running)
            {
                start();
                return 1;
            }
            else // if we need to stop
            {
                stop();
                return 0;
            }
        }
        public override string ToString()
        {
            return startT.ToShortDateString() + "\t" + startT.ToShortTimeString() + " - " + stopT.ToShortTimeString() + "\t Total: " + totalWorkTime + " Hours";
        }
        public double getTotalHours()
        {
            return totalWorkTime;
        }
        public void reset()
        {
            startT = DateTime.Now;
            stopT = DateTime.Now;
            totalWorkTime = 0;
        }
    }
}
