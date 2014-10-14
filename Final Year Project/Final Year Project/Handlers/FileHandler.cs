using System;
using System.IO;
using System.Xml.Serialization;
using Final_Year_Project.GameData;

namespace Final_Year_Project.Handlers
{
    static class FileHandler
    {
        #region Variables
        private static XmlSerializer serializerObject;
        private static TextWriter writeFileStream;
        #endregion

        #region General Methods
        public static void writeToFile(SystemOptions _options, String path, XmlSerializer type)
        {
            if (File.Exists(path))
                return;
            serializerObject = type;
            writeFileStream = new StreamWriter(path);
            serializerObject.Serialize(writeFileStream, _options);
            writeFileStream.Close();
        }

        public static void writeToFile(SystemOptions _options, String path, XmlSerializer type, Boolean overwrite)
        {
            if (overwrite == false)
                return;
            serializerObject = type;
            writeFileStream = new StreamWriter(path);
            serializerObject.Serialize(writeFileStream, _options);
            writeFileStream.Close();
        }

        public static SystemOptions readFromFile(String path, XmlSerializer type)
        {
            if (!File.Exists(path))
                return null;
            serializerObject = type;
            FileStream ReadFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            SystemOptions LoadedObj = (SystemOptions)serializerObject.Deserialize(ReadFileStream);
            ReadFileStream.Close();

            return LoadedObj;
        }
        #endregion
    }
}
