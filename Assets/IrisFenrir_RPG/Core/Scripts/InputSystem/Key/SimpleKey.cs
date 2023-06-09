using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class SimpleKey : IKey
    {
        private RealKey m_realKey;

        private List<IKey> m_keys;

        public SimpleKey()
        {
            m_realKey = new RealKey();
            m_realKey.keyCode = KeyCode.None;

            m_keys = new List<IKey>
            {
                m_realKey
            };
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


        public void SetKeyCode(KeyCode keyCode)
        {
            m_realKey.keyCode = keyCode;
        }
        public KeyCode GetKeyCode()
        {
            return m_realKey.keyCode;
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

        public T GetKey<T>(int index) where T:IKey
        {
            if (index < 0 || index >= m_keys.Count) return null;
            return m_keys[index] as T;
        }

        public override bool GetKeyPressing()
        {
            if (!enable) return false;
            bool result = false;
            foreach (IKey key in m_keys)
            {
                result = result || key.GetKeyPressing();
            }
            return result;
        }

        public override bool GetKeyDown()
        {
            if (!enable) return false;
            bool result = false;
            foreach (IKey key in m_keys)
            {
                result = result || key.GetKeyDown();
            }
            return result;
        }

        public override bool GetKeyUp()
        {
            if (!enable) return false;
            bool result = false;
            foreach (IKey key in m_keys)
            {
                result = result || key.GetKeyUp();
            }
            return result;
        }

        public override Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["enable"] = enable;
            Json arr = new Json(Json.Type.Array);
            m_keys.ForEach(k => arr.Add(k.Save()));
            json["keys"] = arr;
            return json;
        }

        public override void Load(Json json)
        {
            SetEnable(json["enable"]);
            List<Json> arr = json["keys"].array;
            for (int i = 0; i < m_keys.Count; i++)
            {
                m_keys[i].Load(arr[i]);
            }
        }
    }
}
