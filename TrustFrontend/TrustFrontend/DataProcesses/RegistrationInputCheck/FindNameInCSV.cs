using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace TrustFrontend
{
    class FindNameInCSV
    {
        /// <summary>
        /// Method which scans the list of Russian names and tries to find the user's name in it
        /// </summary>
        /// <param name="name">
        /// User's name
        /// </param>
        /// <returns>
        /// true if the name is in the list, false otherwise 
        /// </returns>
        public static bool Find(string name)
        {
            //connect to data-file
            var assembly = Assembly.GetExecutingAssembly();
            string filePath = "TrustFrontend.russian_names.csv";
            //list to store data
            List<string> nameDataList = new List<string>();
            //reading the csv file
            using (Stream stream = assembly.GetManifestResourceStream(filePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Split(';')[1];
                        nameDataList.Add(line);
                    }
                }
            }
            //seraching the name (O(n) complexity)
            bool isNameInList = false;
            for (int i = 1; i<nameDataList.Count; i++)
            {
                if (nameDataList[i] == name)
                {
                    isNameInList = true;
                    break; 
                }
            }
            return isNameInList;
        }
    }
}
