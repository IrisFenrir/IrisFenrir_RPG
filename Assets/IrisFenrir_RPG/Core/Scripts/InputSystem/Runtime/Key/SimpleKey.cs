using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class SimpleKey : IKey
    {
        private List<IKey> m_keys;

        public SimpleKey()
        {
            m_keys = new List<IKey> { new RealKey() };
        }

        public void AddKey(IKey key)
        {
            if (key == null) return;
            m_keys.Add(key);
        }

        public bool RemoveKey(IKey key)
        {
            return m_keys.Remove(key);
        }

        public T GetKey<T>(int index) where T : IKey
        {
            if (index < 0 || index >= m_keys.Count) return null;
            return m_keys[index] as T;
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable, includeChildren);
            if(includeChildren)
            {
                m_keys.ForEach(k => k.SetEnable(enable, includeChildren));
            }
        }

        public void SetKeyEnable(bool enable, bool includeChildren = true, int index = 0)
        {
            if (index < 0 || index >= m_keys.Count) return;
            m_keys[index].SetEnable(enable, includeChildren);
        }


        public void SetKeyCode(KeyCode keyCode, int index = 0)
        {
            RealKey key = GetKey<RealKey>(index);
            if (key == null) return;
            key.keyCode = keyCode;
        }

        public KeyCode GetKeyCode(int index = 0)
        {
            RealKey key = GetKey<RealKey>(index);
            if (key == null) return KeyCode.None;
            return key.keyCode;
        }



        public override bool GetKeyPressing()
        {
            if (!Enable) return false;
            foreach (IKey key in m_keys)
            {
                if (key.GetKeyPressing())
                    return true;
            }
            return false;
        }

        public override bool GetKeyDown()
        {
            if (!Enable) return false;
            foreach (IKey key in m_keys)
            {
                if (key.GetKeyDown())
                    return true;
            }
            return false;
        }

        public override bool GetKeyUp()
        {
            if (!Enable) return false;
            foreach (IKey key in m_keys)
            {
                if (key.GetKeyUp())
                    return true;
            }
            return false;
        }

        public override Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["enable"] = Enable;
            Json arr = new Json(Json.Type.Array);
            m_keys.ForEach(k => arr.Add(k.Save()));
            json["keys"] = arr;
            return json;
        }

        public override void Load(Json json)
        {
            try
            {
                SetEnable(json["enable"], false);
                List<Json> arr = json["keys"].array;
                for (int i = 0; i < m_keys.Count; i++)
                {
                    m_keys[i].Load(arr[i]);
                }
            }
            catch
            {
                ErrorLog.Log(ErrorSetting.jsonAnalysisError);
            }
        }
    }
}
