using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FruitImportLogic
{
    class ImportHandler
    {
        string fileIgnore;
        string _readPath = "";
        string _apiUrl = "";
        string _movePath = "";

        public ImportHandler(string readPath,string movePath,string apiUrl)
        {
            fileIgnore = "_ign_"; // files with this token at the start of name will be ignored
            _readPath = readPath;
            _movePath = movePath;
            _apiUrl = apiUrl;
            try
            {
                if (Directory.Exists(_readPath) == false)
                    Directory.CreateDirectory(_readPath);

                if (Directory.Exists(_movePath) == false)
                    Directory.CreateDirectory(_movePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public string MakeImportChanges()
        {
            string logstring = ""; // logstring is used to return what's supposed to be logged by the main class.
            try
            {
                DirectoryInfo myFileList = new DirectoryInfo(_readPath);
                FileInfo[] filer = myFileList.GetFiles();
                if (filer.Length == 0)
                {
                    Console.WriteLine("No files found");
                }
                else
                {
                    foreach (FileInfo item in filer) // loop through files in directory
                    {
                        if (item.Name.Substring(0, fileIgnore.Length) != fileIgnore)
                        {

                            try
                            {

                                //read file


                                XmlDocument doc = new XmlDocument();
                                StreamReader reader = new StreamReader(item.FullName);
                                string textFileContent = reader.ReadToEnd();
                                reader.Close();


                                doc.LoadXml(textFileContent);

                                if (doc.DocumentElement.Name.ToString() == "InGoingFruit") // loop through document
                                {
                                    List<ContentOfIncomingTransaction> transactionContent = new List<ContentOfIncomingTransaction>();
                                    ProcessedIncomingTransactions transaction = new ProcessedIncomingTransactions();
                                    foreach (XmlElement element in doc.DocumentElement.ChildNodes)
                                    {
                                        if (element.Name == "Fruit" && element.HasChildNodes && element.HasAttributes && element.Attributes[0].Name == "ID") // the file needs to have a fruit element
                                        {
                                            string fruitID = element.Attributes[0].Value;


                                            foreach (XmlElement elementInFruit in element.ChildNodes)
                                            {

                                                string suppliedQuantity = "";
                                                if (elementInFruit.Name == "SuppliedQuantity")
                                                {
                                                    suppliedQuantity = elementInFruit.InnerText;
                                                    transactionContent.Add(new ContentOfIncomingTransaction(int.Parse(fruitID), int.Parse(suppliedQuantity)));

                                                }

                                            }
                                        }
                                        if (element.Name == "Supplier" && element.Attributes[0].Name == "ID") // it also should have a supplier but it is not required
                                        {
                                            transaction.Supplier = int.Parse(element.Attributes[0].Value);
                                        }
                                    }

                                    if (transactionContent.Count > 0) // if fruits were added to the transactions list
                                    {
                                        TransactionWithContent transactionWithContent = new TransactionWithContent(transaction, transactionContent);
                                        // POST TO API
                                        HttpClient client = new HttpClient();
                                        client.BaseAddress = new Uri(_apiUrl);
                                        client.DefaultRequestHeaders.Accept.Clear();
                                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                        var stringContent = new StringContent(JsonConvert.SerializeObject(transactionWithContent), System.Text.Encoding.UTF8, "application/json");
                                        HttpResponseMessage response = client.PostAsync("/Fruits/PostFruitImport", stringContent).Result;
                                        using (HttpContent content = response.Content)
                                        {

                                        }

                                        if (!response.IsSuccessStatusCode)
                                        {
                                            throw new Exception("Failed API request: failed to send the imported content to address " + _apiUrl + "/Fruits/PostFruitImport");
                                        }
                                        else
                                        {
                                            logstring += Environment.NewLine + string.Format("Sucessful API request: sucessfully sent incoming transaction from supplier: {0} to API.", transaction.Supplier);
                                        }

                                    }

                                }

                                // move
                                logstring += MoveFileFromFolder(item);

                            }
                            catch (Exception ex)
                            {
                                File.Move(item.FullName, Path.Combine(item.DirectoryName, fileIgnore + item.Name)); // set file to ignore
                                throw new Exception(ex.Message);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

            return logstring;

        }
        public string MoveFileFromFolder(FileInfo item)
        {
            try
            {
               
                item.MoveTo(_movePath +"\\"+DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + item.Name); // moves file and adds date to the name
                return Environment.NewLine + item.Name + " moved to: " + _movePath;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
