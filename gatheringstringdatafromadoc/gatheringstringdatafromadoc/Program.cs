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
           // changeFileExtension(@"Testing.zip", ".docx");

            string path = getFilesInDirectory(".docx");   // search working directory and retrieve the path of any Docx file
            changeFileExtension(path, ".zip");           // we need to convert this docx file to a ZIP.
            path = getFilesInDirectory(".zip");         // search for the zip file
            extractor(path, "XMLdata");                // extract the newly created zip if nessecary
            string readData = readXmlData();          // read the XML data in the zip file (from converting a docx file to a zip)
           
           Console.WriteLine(readData);
           Console.ReadLine();
        }

        private static string getFilesInDirectory(string type)
        {

            //populate an array of type FileInfo with the paths of the items in the working directory
            DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo[] files = d.GetFiles();

            /* convert files array into a string array to check for .DOCX file and have a string return type and gets the file
               path of the file that contains .docx then returns it to main 
               we also need to make sure zip files make it through so when 
               only an .zip file exists we return that as well */
            string path = string.Empty;

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].ToString().Contains(type))
                {
                    path = files[i].FullName.ToString();
                    break;
                }
            }

            return path;

        }

        private static dynamic readXmlData()
        {

            string r = readTxtFile(@"XMLdata\word\document.xml");
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
