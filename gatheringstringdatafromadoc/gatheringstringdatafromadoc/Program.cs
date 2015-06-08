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
          //  changeFileExtension(@"Testing.zip", ".docx");

             string path = getFilesInDirectory(".docx");   // search working directory and retrieve the path of any Docx file.
             changeFileExtension(path, ".zip");           // we need to convert this docx file to a ZIP.
             path = getFilesInDirectory(".zip");         // search for the zip file.
             extractor(path, "XMLdata");                // extract the newly created zip if nessecary.
             string output = readXmlData();            // read the XML data in the zip file (from converting a docx file to a zip).
             writeTxtFile("parsedXML.txt", output);   // write raw output to a text file.
             formatLunchDoc("parsedXML.txt");        // format the text files and move data around for easier access of information in node.js
             
           Console.WriteLine();
           Console.ReadLine();
        }

        #region --lunch DOC--

        /* a lot of different formating that involves seperating text into different files 
           and removing all unneeded text */
        private static void formatLunchDoc(string content)
        {
            List<string> contentList = new List<string>();
            StreamReader reader = new StreamReader(content);
            string readBuffer = string.Empty;


            // turns the string that held the read text file and makes a list of each of its lines
            while (true)
            {
                readBuffer = reader.ReadLine();
                contentList.Add(readBuffer);

                if (reader.EndOfStream)
                {
                    reader.Close();
                    break;
                }
            }

            // removes the header of the list 
            for (int i = 0; i <= 5; i++)
            {
                contentList.RemoveAt(0);
            }

            // seperates monday, tuesday, wednesday, thursday and friday into different text files.
            string[] dayData = new string[5];
            StringBuilder sb = new StringBuilder();

            List<string> DaysOfTheWeek = new List<string>();
            DaysOfTheWeek = initalizeDaysOfTheWeekList(DaysOfTheWeek);  // hurrdur my verbose language is the best
            DaysOfTheWeek.Remove("MONDAY");


            int counter = 0;
            

            for (int i = 0; i <= contentList.Count; i++)
            {
                //cover exception when  list is empty, we need to add friday back to the list.
                if (DaysOfTheWeek.Count == 0)
                {
                    DaysOfTheWeek.Add("FRIDAY");
                }

                //  I really want to refactor this
                if (contentList[i].Contains(DaysOfTheWeek[0]))
                {                 
                    counter++;
                    DaysOfTheWeek.RemoveAt(0);
                    sb.Clear();
                }

                try
                {
                    dayData[counter] = sb.AppendLine(contentList[i] + " ").ToString();         
                }
                catch (Exception)
                {
                    sb.Clear();
                    break;   
                    
                }
                 
            }

            // we need to specifically remove the footing information from the friday textfile.
            int subStrCounter = 
                SeperateString(dayData[4], '*');
           
            // assigns a substring of DayData[4] to DayData[4]
            dayData[4] = dayData[4].Substring(0, subStrCounter);


            // create a directory to hold all of the new files
            Directory.CreateDirectory(@"Days");

            DaysOfTheWeek = initalizeDaysOfTheWeekList(DaysOfTheWeek);
            // write all to seperate files

            for (int i = 0; i < dayData.Length; i++)
            {
               writeTxtFile(@"Days\" + DaysOfTheWeek[i] + ".txt", dayData[i]);
            }


            /*
             * 
             * due to the nature of tweets, text files will be seperated into a lunch menu and a sandwich menu
             * It will be like this:
             * MONDAY -> MONDAY_LUNCH | MONDAY_SANDWICH
             * 
             */

            string currentFileContent = string.Empty;
            subStrCounter = 0;
            for (int i = 0; i < DaysOfTheWeek.Count; i++)
            {
                currentFileContent = readTxtFile( @"Days\" + DaysOfTheWeek[i] + ".txt" );
                subStrCounter = SeperateString( currentFileContent, "SANDWICH CHOICE" );
                currentFileContent = currentFileContent.Substring(0, subStrCounter);
                writeTxtFile(@"Days\" + DaysOfTheWeek[i] + "_LUNCH" + ".txt", currentFileContent);
            }

      

        }

        /*mainly because we need to remove the elements and then use
        it again after we've removed the elements and it would be
        abstract pain to type it out again */
        private static List<string> initalizeDaysOfTheWeekList(List<string> DaysOfTheWeek)
        {
            
            DaysOfTheWeek.Clear();
            DaysOfTheWeek.Add("MONDAY");
            DaysOfTheWeek.Add("TUESDAY");
            DaysOfTheWeek.Add("WEDNESDAY");
            DaysOfTheWeek.Add("THURSDAY");
            DaysOfTheWeek.Add("FRIDAY");

            return DaysOfTheWeek;

        }


        //Checks a list to see if the first element is empty
       private static bool isEmptyList(List<string> l )
        {
            if (l[0].Contains(string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

  
        // generates how long different substrings will be based on a specfic condition
       private static int SeperateString(string str , char condition)
        {
            int counter = 0;
            char[] c = str.ToCharArray();


            for (int i = 0; i <= c.Length; i++)
            {
                if  (c[i] != condition) 
                {
                    counter++;
                }
                else
                {
                    break;
                }
            }
            return counter;
        }


        // overload method for SeperateString that evaluates the condition as a string and not a char
       private static int SeperateString(string str, string condition)
       {
           int accum = 0;
           string[] words = str.Split(' ');
          

           for (int i = 0; i < words.Length; i++)
           {
               string breakConditon = words[i] + " " + words[i + 1];
               breakConditon = RemoveBreakChars(breakConditon);

               if ( breakConditon != condition )
               {
                   accum += words[i].Length;
               }
               else
               {
                   accum += words[i].Length;
                   break;
               }
           }

           return accum;
       }

       private static string RemoveBreakChars(string s)
       {
           char[] c = s.ToCharArray();
           s = string.Empty;
           for (int i = 0; i < c.Length; i++)
           {
               if (!(c[i] == '\r' || c[i] == '\n'))
               {
                   s += c[i];
               }
           }

           return s;
       }





        #endregion


       #region --File Manipulation--


       // gets a the specific path of a file in a directory when there is only one of those files
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
        //read the XML from the converted document
       private static dynamic readXmlData()
       {

           string docPath = @"XMLdata\word\document.xml";
           StringBuilder output = new StringBuilder();

           // parse XML elements and builds a string based on the reader
           using (XmlReader reader = XmlReader.Create(docPath))
           {

               while (reader.Read())
               {
                   if (reader.HasValue == true)
                   {
                       output.AppendLine(reader.Value);
                   }
               }


           }

           return output.ToString();

       }
        // write to a text file
       private static void writeTxtFile(string name, string content)
       {
           string pathCheck = getFilesInDirectory(".txt"); //check to see if the working directory already contains a extractedXML.txt file
           try
           {

               content = content.Replace("version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"", String.Empty);


               if (pathCheck.Contains(name))
               { Console.WriteLine("parsedXML.txt already exists (nothing to write)"); }

               else
               {
                   StreamWriter w = new StreamWriter(name);

                   w.WriteLine(content);
                   w.Close();
               }



           }
           catch (Exception)
           {
               Console.WriteLine("Couldn't write to  File!");
               throw;

           }



       }
        //read a text file
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
        //extract the generated ZIP file
       private static void extractor(string zipPath, string newPath)
       {

           try
           {
               string checkPath = getFilesInDirectory(".zip");
               if (checkPath.Contains("XMLdata"))
               { Console.WriteLine("File already extracted, moving along"); }
               else
               {
                   ZipFile.ExtractToDirectory(zipPath, newPath);
               }

           }
           catch (Exception)
           {
               Console.WriteLine("File already Extracted, moving along");

           }

       }
        //convert the file from file.whatever to file.somethingElse
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

       #endregion
    
    }
}
