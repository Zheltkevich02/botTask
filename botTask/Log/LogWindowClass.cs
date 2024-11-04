using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace botTask.Log
{
    public class LogWindowClass
    {
        LogClass logClass;
        bool isFirst = true;
        DispatcherTimer timer;
        MainWindow window;

        public LogWindowClass(MainWindow _window)
        {
            window = _window;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateList();
        }

        public void UpdateList()
        {
            List<LogClass> list = new List<LogClass>();
            string path = GlobalSettings.pathDoc + "\\LogBot.txt";
            if (isFirst)
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                fs.Close();
            }

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        string[] context = line.Split('|');
                        if (context.Length == 2)
                        {
                            logClass = new LogClass(context[0], context[1]);
                            list.Add(logClass);
                        }
                    }
                    catch { }
                }
                list.Reverse();
                window.LVLog.ItemsSource = list.ToList();
            }
        }
    }
}
