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

        public void SetEnable(bool enable, bool includeChildren = false)
        {
            this.enable = enable;
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
            return null;
        }

        public void Load(Json json)
        {

        }
    }
}
