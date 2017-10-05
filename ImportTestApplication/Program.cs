using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            FruitImportLogic.Main importHandler = new FruitImportLogic.Main("C:\\Test\\Log.txt", "http://localhost:8081","C:\\Test\\Import", "C:\\Test\\ImportArchive");
            importHandler.Start();

            var input = Console.ReadKey();
            if (input.Key.ToString() == "A")
                importHandler.Stop();

            Console.ReadKey();
        }
    }
}
