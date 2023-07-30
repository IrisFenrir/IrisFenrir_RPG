using System.Collections.Generic;

namespace IrisFenrir.InputSystem
{
    public class InputSystem : Singleton<InputSystem>, IUpdater, IEnable, ISavable
    {
        public bool Enable { get; private set; }

        public List<IVirutalKey> Keys { get; private set; }

        public InputSystem()
        {
            Keys = new List<IVirutalKey>();
        }

        public static void AddKey(IVirutalKey key)
        {
            if (key == null) return;
            Instance.Keys.Add(key);
        }

        public static void RemoveKey(IVirutalKey key)
        {
            Instance.Keys.Remove(key);
        }

        public static T FindKey<T>(string name) where T:IVirutalKey
        {
            return Instance.Keys.Find(key => key.Name == name) as T;
        }

        public static IVirutalKey FindKey(string name)
        {
            return Instance.Keys.Find(key => key.Name == name);
        }

        public void SetEnable(bool enable, bool includeChildren = true)
        {
            Enable = enable;
            if (includeChildren)
                Keys.ForEach(key => key.SetEnable(enable, includeChildren));
        }

        public bool GetEnable()
        {
            return Enable;
        }

        public void Init()
        {
            Keys.ForEach(key => key.Init());
        }

        public void Update(float deltaTime)
        {
            Keys.ForEach(key => key.Update(deltaTime));
        }

        public void Stop()
        {
            Keys.ForEach(key => key.Stop());
        }

        public Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["enable"] = Enable;
            Json arr = new Json(Json.Type.Array);
            Keys.ForEach(key => arr.Add(key.Save()));
            json["keys"] = arr;
            return json;
        }

        public void Load(Json json)
        {
            try
            {
                SetEnable(json["enable"], false);
                List<Json> arr = json["keys"].array;
                int index = 0;
                Keys.ForEach(key => key.Load(arr[index++]));
            }
            catch
            {
                ErrorLog.Log(ErrorSetting.jsonAnalysisError);
            }
        }
    }
}
