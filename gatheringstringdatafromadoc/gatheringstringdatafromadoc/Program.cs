using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Xml;
using System.Xml.Resolvers;
namespace gatheringstringdatafromadoc
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo h = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo[] pong = h.GetFiles();
            Console.WriteLine(pong[pong.Length - 1].Extension);
           
           //changeFileExtension(@"Testing.zip", ".docx"); // we need to convert this docx file to a ZIP.
           //extractor(@"Testing.zip", "Testing"); // extract the newly created zip is nessecary
           //string readData = readXmlData(); // read the XML data in the zip file (from converting a docx file to a zip)
           
           //Console.WriteLine(readData);
           Console.ReadLine();
        }

        private static dynamic readXmlData()
        {
            string r = readTxtFile(@"Testing\word\document.xml");
            StringBuilder output = new StringBuilder();

            using( XmlReader reader = XmlReader.Create( new StringReader(r) ) ) 
            {
                reader.ReadToFollowing("w:r");
                reader.MoveToFirstAttribute();
                string word = reader.Value;

                output.AppendLine(word);

                return output.ToString();
            }

        }

        private static string writeTxtFile(string path)
        {
            try
            {

                StreamWriter w = new StreamWriter(path);
               /* place XML data in a data structure and write to this */

            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't write to  File!");
                throw;
               
            }

            return null;
        }
        private static string readTxtFile(string path)
        {
          /*read the wirtten file if nesecary */
            try
            {
                StreamReader reader = new StreamReader(path);
                string info = reader.ReadToEnd();
                reader.Close();
                return info;
            }
            catch (Exception)
            {
                Console.WriteLine("File not found ( or file in the wrong format )");
                return string.Empty;
            }
     

        }

        private static void extractor(string zipPath , string newPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipPath, newPath);
            }
            catch (Exception)
            {
                Console.WriteLine("File already Extracted, moving along");
               
            }
            
        }
        private static void changeFileExtension(string path, string newType)
        {
            try
            {
                File.Move(path, Path.ChangeExtension(path, newType));       
            }
            catch (Exception)
            {
                Console.WriteLine("File has already been changed to the proper format");
               
            }
         
        }
    }
}
