using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gatheringstringdatafromadoc
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"Testing.docx";
            changeEXTtoTxt(path);
            path = @"Testing.txt";
            string info = readTxtFile(path);

            Console.WriteLine(info);
            Console.ReadLine();
        }

        private static string readTxtFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            string info = reader.ReadToEnd();
            reader.Close();
            return info;

        }

        private static void changeEXTtoTxt(string path)
        {
            File.Move(path, Path.ChangeExtension(path, ".zip"));       
        }
    }
}
