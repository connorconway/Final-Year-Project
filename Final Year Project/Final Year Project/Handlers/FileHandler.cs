using System;
using System.IO;
using System.Xml.Serialization;
using Multiplayer_Software_Game_Engineering.GameData;

namespace Multiplayer_Software_Game_Engineering.Handlers
{
    static class FileHandler
    {
        private static XmlSerializer serializerObject;
        private static TextWriter    writeFileStream;

        public static void writeToFile(SystemOptions options, String path, XmlSerializer type)
        {
            if (File.Exists(path))
                return;

            serializerObject = type;
            writeFileStream  = new StreamWriter(path);
            
            serializerObject.Serialize(writeFileStream, options);
            writeFileStream.Close();
        }

        public static void writeToFile(SystemOptions options, String path, XmlSerializer type, Boolean overwrite)
        {
            if (overwrite == false)
                return;

            serializerObject = type;
            writeFileStream  = new StreamWriter(path);

            serializerObject.Serialize(writeFileStream, options);
            writeFileStream.Close();
        }

        public static SystemOptions readFromFile(String path, XmlSerializer type)
        {
            if (!File.Exists(path))
                return null;

            serializerObject   = type;
            var ReadFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var LoadedObj      = (SystemOptions)serializerObject.Deserialize(ReadFileStream);

            ReadFileStream.Close();
            return LoadedObj;
        }
    }
}