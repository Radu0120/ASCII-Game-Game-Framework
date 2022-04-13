using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public class Logger
    {
        public static TraceSource ts;

        public Logger()
        {
            ts = new TraceSource("Logger");

            // setting the overall switch
            ts.Switch = new SourceSwitch("Log", "All");


            TraceListener fileLog = new TextWriterTraceListener(new StreamWriter("LogFile.txt"));
            ts.Listeners.Add(fileLog);
        }
    }
}
