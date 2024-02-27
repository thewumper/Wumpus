using System;
using System.IO;
using System.Text;

namespace WumpusCore.HighScoreNS
{
    internal class SaveFile
    {
        public SaveFile()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string wumpusSaveDirectory = Path.Combine(appDataPath, "WumpusGame");
            if (!Directory.Exists(wumpusSaveDirectory))
            {
                Directory.CreateDirectory(wumpusSaveDirectory);
            }

            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(wumpusSaveDirectory))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // Open the stream and read it back.
            using (StreamReader sr = File.OpenText(wumpusSaveDirectory))
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
