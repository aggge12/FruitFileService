using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FruitImportLogic
{
    public class Main
    {
        string connectionstring, readableFolder, movableFolder;

        Thread _MainThread;
        bool _StopFlag = false;
        LogHandler logger;
        string logPath;

        public Main(string logPath, string connectionstring, string readableFolder, string movableFolder)
        {
            this.logPath = logPath;
            this.connectionstring = connectionstring;
            this.readableFolder = readableFolder;
            this.movableFolder = movableFolder;
            logger = new LogHandler(logPath);
        }

        public Main()
        {
  
        }

        public void Start()
        {
            try
            {
                if (_MainThread != null)
                {
                    if (_MainThread.ThreadState == ThreadState.Running)
                    {
                        Console.WriteLine("Stopping current thread");

                        //Lock to prevent other threads accessing thread 
                        lock (_MainThread)
                        {
                            _MainThread.Abort();
                        }
                    }
                }
                _MainThread = new Thread(Run);
                _StopFlag = false;
                _MainThread.Start();
            }
            catch (Exception ex)
            {
                logger.logMessage(ex.Message);
            }
        }

        private void Run()
        {
            try
            {
                logger.logMessage("Filservice starting");
                Console.WriteLine("Filservice starting");
                ImportHandler fileChecking = new ImportHandler(readableFolder, movableFolder, connectionstring); // get path from configurable
                while (_StopFlag != true)
                { 
                    try
                    {
                        string events = fileChecking.MakeImportChanges();
                        if (events != "") // if something was saved for logging inside the function then log it to file
                        {
                            logger.logMessage(events);
                            Console.WriteLine(events);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.logMessage("Could not handle import files for following reasons: " + ex.Message + "File Will be ignored until it is removed and added again");
                        Console.WriteLine("Could not handle import files for following reasons: " + ex.Message + "File Will be ignored until it is removed and added again");
                    }

                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                logger.logMessage(ex.Message);
            }
        }

        public void Stop()
        {
            logger.logMessage("Stopping filservice");
            _StopFlag = true;
        }
    }
}
