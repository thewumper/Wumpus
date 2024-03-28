using System;
using System.IO;
using System.Text;

namespace WumpusCore.HighScoreNS
{
    internal class SaveFile
    {
        /// <summary>
        /// The file path used by the savefile object
        /// </summary>
        public readonly string path;

        /// <summary>
        /// Creates a save file at a set directory
        /// </summary>
        /// <param name="text"> What information to save to the file </param>
        /// <param name="readText"> Whether or not to print the text to console </param>
        public SaveFile(string text, bool readText)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string wumpusSaveDirectory = Path.Combine(appDataPath, "WumpusGame");
            if (!Directory.Exists(wumpusSaveDirectory))
            {
                Directory.CreateDirectory(wumpusSaveDirectory);
            }

            this.path = wumpusSaveDirectory;

            // Create the file, or overwrite if the file exists.
            CreateFile(text);

            // Open the stream and read it back.
            ReadFile(readText);
        }

        /// <summary>
        /// Takes a directory to create a file
        /// </summary>
        /// <param name="readText"> file path to get information from </param>
        /// <param name="readText"> Whether or not to print the text to console </param>
        public SaveFile(bool readText, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            this.path = path;

            ReadFile(readText);
        }

        /// <summary>
        /// Uses the directory to create a file
        /// with the given text stored
        /// </summary>
        /// <param name="text"> what text to store in the file </param>
        public void CreateFile(string text)
        {
            using (FileStream fs = File.Create(Path.Combine(this.path, "SaveData.txt")))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(text);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        /// <summary>
        /// Reads the information on the file
        /// </summary>
        /// <param name="printText"> whether or not to print the text from the file </param>
        /// <returns> string information of file </returns>
        public string ReadFile(bool printText)
        {
            using (StreamReader sr = File.OpenText(Path.Combine(this.path, "SaveData.txt")))
            {
                string s = "";
                string fullText = "";
                while ((s = sr.ReadLine()) != null)
                {
                    fullText += s;
                    if (printText) { Console.WriteLine(s); }
                }
                return fullText;
            }
        }
    }
}
