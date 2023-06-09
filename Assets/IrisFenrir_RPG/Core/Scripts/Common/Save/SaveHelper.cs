using System.IO;
using System.Text;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class SaveHelper
    {
        public static void Save(Json json, string path)
        {
            if (!Directory.GetParent(path).Exists)
            {
                ErrorLog.Log(ErrorSetting.pathNotExistError);
                return;
            }
            string jsonText = JsonMapper.ToJsonString(json);
            byte[] bytes = Encoding.UTF8.GetBytes(jsonText);
            FileStream stream = File.Create(path);
            stream.Write(bytes);
            stream.Close();
        }

        public static void Save(ISavable obj, string path)
        {
            Save(obj.Save(), path);
        }

        public static Json Load(string path)
        {
            if (!Directory.Exists(path))
            {
                ErrorLog.Log(ErrorSetting.pathNotExistError);
                return null;
            }
            FileStream stream = File.Open(path, FileMode.Open);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes);
            stream.Close();
            string json = Encoding.UTF8.GetString(bytes);
            return JsonMapper.ToJsonObject(json);
        }

        public static void Load(ISavable obj, string path)
        {
            obj.Load(Load(path));
        }
    }
}
