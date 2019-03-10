using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TrustFrontend
{
    class FindSurnameInCSV
    {
        /// <summary>
        /// Method which scans the list of Russian surnames and tries to find the user's surname in it
        /// </summary>
        /// <param name="surname">
        /// User's surname
        /// </param>
        /// <returns>
        /// true if the surname is in the list, false otherwise 
        /// </returns>
        public static bool Find(string surname)
        {
            //connect to data-file
            var assembly = Assembly.GetExecutingAssembly();
            string filePath = "TrustFrontend.russian_surnames.csv";
            //list to store data
            List<string> surnameDataList = new List<string>();
            //read data
            using (Stream stream = assembly.GetManifestResourceStream(filePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Split(';')[1];
                        surnameDataList.Add(line);
                    }
                }
            }
            //search for the surname (O(n) cmplexity)
            bool isNameInList = false;
            for (int i = 1; i < surnameDataList.Count; i++)
            {
                if (surnameDataList[i] == surname)
                {
                    isNameInList = true;
                    break;
                }
            }
            return isNameInList;
        }
    }
}
