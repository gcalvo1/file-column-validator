#region Namespaces

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace CCubed_2012.Classes
{
    /// <summary>
    /// ScriptMain is the entry point class of the script.  Do not change the name, attributes,
    /// or parent of this class.
    /// </summary>

    public class FileManipulator
    {

        public List<String> ReadDataFromFile(string filePath)
        {
            string stringLine;
            List<String> stringListLines = new List<String>();

            //Read from the file
            var readFile = new StreamReader(filePath);

            // Read the file and store the data into string list line by line
            while ((stringLine = readFile.ReadLine()) != null)
            {
                //MessageBox.Show(line);
                stringListLines.Add(stringLine);
            }

            //Close the file
            readFile.Close();

            return stringListLines;
        }

        public void WriteToFile(string filePath, int rowStart, List<String> stringListLines)
        {
            var writeFile = new StreamWriter(filePath, false);

            var rowNumber = 0;

            // Take a line at a time from StringListLines and write it to the File.
            foreach (String line in stringListLines)
            {
                if (rowNumber > rowStart && (line[1] == 'M' || line[0] == 'm'))
                {
                    writeFile.WriteLine(line.Remove(0, 1) + ",File_Name");
                }

                else if (rowNumber > rowStart && Information.IsNumeric(line[1]))
                {
                    writeFile.WriteLine(line.Remove(0, 1) + ",File_Name");
                }
                rowNumber++;
            }

            //Close file
            writeFile.Close();
        }

        public bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                stream?.Close();
            }

            //file is not locked
            return false;
        }

    }
}