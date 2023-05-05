using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElvuiUpdater.SoftwareUpdate
{
    class ProgressIndicator
    {
        long max;
        long current;
        int progressLength = 50;
        int progress;
        static object locker = new object();
        static Mutex mutexObj = new Mutex();

        public ProgressIndicator(long max, int current = 0)
        {
            this.max = max;
            this.current = current;

            progress = (int)(((double)current) / ((double)max) * progressLength);
            Console.WriteLine($"Progress:");

        }

        public void SetProgress(long current)
        {
            this.current = current;
            var neWprogress = (int)(((double)current) / ((double)max) * progressLength);
            if (progress != neWprogress)
            {
                progress = neWprogress;
                WriteProgress();

            }
        }

        public void SetProgressDelta(long delta)
        {
            current += delta;
            var neWprogress = (int)(((double)current) / ((double)max) * progressLength);
            if (progress != neWprogress || current == delta)
            {
                progress = neWprogress;
                WriteProgress();

            }
        }

        void WriteProgress()
        {
            lock (locker)
            {
                var prog = "";
                for (int i = 0; i < progress; i++)
                {
                    prog += "X";
                }

                for (int i = progress; i < progressLength; i++)
                {
                    prog += ".";
                }

                Console.WriteLine($"[{prog}]");
                if (progress != progressLength)
                    Console.SetCursorPosition(0, Console.CursorTop - 1);


            }
        }

    }
}
