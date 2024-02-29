using System;
using System.IO;
using System.Text;

namespace WumpusCore.HighScoreNS
{
    internal class SaveFile
    {
        public SaveFile(string text)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string wumpusSaveDirectory = Path.Combine(appDataPath, "WumpusGame");
            if (!Directory.Exists(wumpusSaveDirectory))
            {
                Directory.CreateDirectory(wumpusSaveDirectory);
            }

            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(Path.Combine(wumpusSaveDirectory, "SaveData.txt")))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(text);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // Open the stream and read it back.
            using (StreamReader sr = File.OpenText(Path.Combine(wumpusSaveDirectory, "SaveData.txt")))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }
    }
}
