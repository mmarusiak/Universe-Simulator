using System;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Utilities.UniverseLibraries
{
    public class UniverseTools : MonoBehaviour
    {
        public static string RoundOutput(float input, int lenRound = 3)
        {
            int rounder = (int)Mathf.Pow(10, lenRound);
            string output =
                ((float)( (int)(input * rounder)) / rounder).ToString(CultureInfo.InvariantCulture);

            if (output.Contains("."))
            {
                while (output.Split(".")[1].Length < lenRound)
                {
                    output += "0";
                }
            }
            else
            {
                output += ".";
                for (int i = 0; i < lenRound; i++) output += "0";
            }

            return output;
        }

        public static bool IsSafeImage(string filePath)
        {
            try
            {
                using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] header = new byte[8];
                stream.Read(header, 0, 8);
                string headerString = BitConverter.ToString(header).Replace("-", string.Empty);

                // check header of a known image format (JPEG, PNG, or GIF) by reading first 8 bytes
                if (headerString.StartsWith("FFD8FF") ||
                    headerString.StartsWith("89504E47") ||
                    headerString.StartsWith("47494638"))
                {
                    return true;
                }

                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
    
        // https://stackoverflow.com/a/3288164/13786856
        public static string RemoveAccents(string input)
        {
            return new string(input
                .Normalize(System.Text.NormalizationForm.FormD)
                .ToCharArray()
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());
            // the normalization to FormD splits accented letters in letters+accents
            // the rest removes those accents (and other non-spacing characters)
            // and creates a new string from the remaining chars
        }
    }
}


