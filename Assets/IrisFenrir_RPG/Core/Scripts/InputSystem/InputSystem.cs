using System.Collections.Generic;

namespace IrisFenrir.InputSystem
{
    public class InputSystem : Singleton<InputSystem>, IUpdater, IEnable, ISavable
    {
        public bool enable { get; private set; }

        private List<IVirutalKey> m_keys;

        public InputSystem()
        {
            m_keys = new List<IVirutalKey>();
        }

        public static void AddKey(IVirutalKey key)
        {
            if (key == null) return;
            instance.m_keys.Add(key);
        }

        public static void RemoveKey(IVirutalKey key)
        {
            instance.m_keys.Remove(key);
        }

        public static T FindKey<T>(string name) where T:IVirutalKey
        {
            return instance.m_keys.Find(key => key.name == name) as T;
        }

        public void SetEnable(bool enable, bool includeChildren = true)
        {
            this.enable = enable;
            if (includeChildren)
                m_keys.ForEach(key => key.SetEnable(enable, includeChildren));
        }

        public bool GetEnable()
        {
            return enable;
        }

        public void Init()
        {
            m_keys.ForEach(key => key.Init());
        }

        public void Update(float deltaTime)
        {
            m_keys.ForEach(key => key.Update(deltaTime));
        }

        public void Stop()
        {
            m_keys.ForEach(key => key.Stop());
        }

        public Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["enable"] = enable;
            Json arr = new Json(Json.Type.Array);
            m_keys.ForEach(key => arr.Add(key.Save()));
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
                m_keys.ForEach(key => key.Load(arr[index++]));
            }
            catch
            {
                ErrorLog.Log(ErrorSetting.jsonAnalysisError);
            }
        }
    }
}
