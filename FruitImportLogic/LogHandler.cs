using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitImportLogic
{
    class LogHandler
    {
        string _logFilePath = "";
        StreamWriter filetoWrite;
        public LogHandler(string logFilePath)
        {
            this._logFilePath = logFilePath;


        }

        public void logMessage(string message)
        {
            filetoWrite = new StreamWriter(_logFilePath, true);


            string time = DateTime.Now.ToString();
            filetoWrite.WriteLine(time + " " + message);

            filetoWrite.Close();
        }

    }
}
