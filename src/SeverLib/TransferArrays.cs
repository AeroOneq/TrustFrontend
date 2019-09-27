
namespace ServerLib
{
    /// <summary>
    /// Transfers integer arrays to a string and a string to an integer array
    /// </summary>
    public class TransferArrays
    {
        /// <summary>
        /// Transfers a string to an integer array 
        /// </summary>
        public static int[] StringToInt(string inputS)
        {
            if (inputS == string.Empty)
            {
                return new int[0];
            }
            string[] stringArr = inputS.Split(Constants.Devider);
            int[] intArr = new int[stringArr.Length];
            for (int i = 0; i < intArr.Length; i++)
            {
                intArr[i] = int.Parse(stringArr[i]);
            }
            return intArr;
        }
        /// <summary>
        /// Transfers integer arrays to a string
        /// </summary>
        public static string IntToString(int[] intArr)
        {
            string resStr = string.Empty;
            for (int i = 0; i < intArr.Length - 1; i++)
            {
                resStr += intArr[i].ToString() + Constants.Devider;
            }
            if (intArr.Length > 0)
            {
                resStr += intArr[intArr.Length - 1].ToString();
            }
            return resStr;
        }
    }
}
