using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FruitImportTestApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
       
        static void Main(string[] args)
        {
            FruitImportLogic.Main myFileMover = new FileLogic.Main();
            myFileMover.Start();

            var input = Console.ReadKey();
            if (input.Key.ToString() == "A")
                myFileMover.Stop();

            Console.ReadKey();
        }
    }
}
