using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FruitImportService
{
    public partial class FruitImportFileChecker : ServiceBase
    {
        FruitImportLogic.Main myImportLogicHandler;
        
        public FruitImportFileChecker()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            myImportLogicHandler = new FruitImportLogic.Main(ConfigurationManager.AppSettings["LogHandler"], ConfigurationManager.AppSettings["FruitServiceBaseUrl"], ConfigurationManager.AppSettings["PathForReadingFiles"], ConfigurationManager.AppSettings["PathForMovingFiles"]);
            myImportLogicHandler.Start();
        }

        protected override void OnStop()
        {
            myImportLogicHandler.Stop();
        }
    }
}
