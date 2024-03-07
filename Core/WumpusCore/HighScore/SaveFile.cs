using System;
using System.IO;
using System.Text;

namespace WumpusCore.HighScoreNS
{
    internal class SaveFile
    {
        string path;

        public SaveFile(string text)
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
            ReadFile(true);
        }

        private void CreateFile(string text)
        {
            using (FileStream fs = File.Create(Path.Combine(this.path, "SaveData.txt")))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(text);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

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
