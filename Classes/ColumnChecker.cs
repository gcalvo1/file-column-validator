using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CCubed_2012.Classes
{
    public class ColumnChecker
    {
        private readonly string _filePathRaw;
        private readonly string _filePathValidation;

        /// <summary>
        /// Class relating to all thigs file validation:
        /// </summary>
        /// <param name="filePathRaw">
        /// File Path of the raw file to be validated
        /// </param>
        /// <param name="filePathValidation">
        /// File path of the validation folder
        /// </param>
        public ColumnChecker(string filePathRaw, string filePathValidation)
        {
            _filePathRaw = filePathRaw;
            _filePathValidation = filePathValidation;
        }

        /// <summary>
        /// Returns column names that do not match the template
        /// </summary>
        /// <param name="rawColumnNameRowNumber">
        /// The number of the row with column names in the raw file
        /// </param>
        /// <param name="columnDelimiter">
        /// The column delimiter of the file to be validated
        /// </param>
        public string ColumnDifference(int rawColumnNameRowNumber, string columnDelimiter)
        {
            var rawFileManipulator = new FileManipulator();
            var templateFileManipulator = new FileManipulator();
            char[] commaDelimiter = { ',' };

            string discrepanicies = "";

            
            //Read contents of Template file into a String List
            var templateColumns = new List<string>(templateFileManipulator.ReadDataFromFile(_filePathValidation));
            //Read contents of Raw file into a String List
            var rawColumns = new List<string>(rawFileManipulator.ReadDataFromFile(_filePathRaw));

            var templateColumnsComma = new List<string>();
            var rawColumnsComma = new List<string>();
            
            if (columnDelimiter == "\\t")
            {
                templateColumnsComma.AddRange(templateColumns.Select(column => column.Replace("\t", ",")));
                rawColumnsComma.AddRange(rawColumns.Select(column => column.Replace("\t", ",")));
            }
            else
            {
                templateColumnsComma = templateColumns;
                rawColumnsComma = rawColumns;
            }

            //Spilt the headers into its own list
            var templateColumnsArray = templateColumnsComma[0].Split(commaDelimiter);

            //Spilt the headers into its own list
            var rawColumnsArray = rawColumnsComma[rawColumnNameRowNumber - 1].Split(commaDelimiter);

            //More or equal columns in raw data than template
            if (rawColumnsArray.Length >= templateColumnsArray.Length)
            {
                var i = 0;
                while (templateColumnsArray.Length > i)
                {
                    if (!string.Equals(rawColumnsArray[i], templateColumnsArray[i],StringComparison.CurrentCultureIgnoreCase))
                    {
                        discrepanicies += rawColumnsArray[i] + ", ";
                    }
                    i++;
                }
                for (var j = i; j < rawColumnsArray.Length; j++)
                {
                    if (rawColumnsArray[j].IsNullOrWhiteSpace())
                    {
                        discrepanicies += "[Unexpected Blank Trailing Column], ";
                    }
                    else
                    {
                        discrepanicies += rawColumnsArray[j] + ", ";
                    }
                }
            }

            //More columns in the template than the raw data
            if (templateColumnsArray.Length > rawColumnsArray.Length)
            {
                var i = 0;
                while (rawColumnsArray.Length > i)
                {
                    if (!string.Equals(rawColumnsArray[i], templateColumnsArray[i], StringComparison.CurrentCultureIgnoreCase))
                    {
                        discrepanicies += templateColumnsArray[i] + ", ";
                    }
                    i++;
                }
                for (var j = i; j < templateColumnsArray.Length; j++)
                {
                    discrepanicies += templateColumnsArray[j] + ", ";
                }
            }

            if (discrepanicies != "")
            {
                discrepanicies = discrepanicies.Remove(discrepanicies.Length - 2,2);
            }

            return discrepanicies;
        }

    }
}