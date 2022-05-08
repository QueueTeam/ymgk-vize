using System.IO;

namespace Assets.Scripts.Data_Access_Layer
{
    public class Acess_File_Data
    {
        public static void writeOnFile(string path, string data)
        {
            StreamWriter writing = new StreamWriter(path, true);
            writing.WriteLine(data);
            writing.Close();
        }

        public static string readFromFile(string path)
        {
            StreamReader reading = new StreamReader(path);
            string fileData = reading.ReadToEnd().ToString();
            return fileData;

        }
    }
}
