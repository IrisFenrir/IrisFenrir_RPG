using System;
using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class ComboKey : IVirutalKey
    {
        public float interval = 0.5f;
        public SimpleKey[] keys;

        public bool isTriggered;
        public int combo;

        public override int KeyCount => keys.Length;

        private float m_currentInterval;

        public ComboKey(int count)
        {
            keys = new SimpleKey[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = new SimpleKey();
            }
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable);
            isTriggered = false;
            combo = 0;
            m_currentInterval = 0f;
            if (includeChildren)
                Array.ForEach(keys, key => key.SetEnable(enable, includeChildren));
        }

        public override void Update(float deltaTime)
        {
            if (!Enable || interval <= 0f || keys.Length == 0) return;

            isTriggered = false;
            m_currentInterval += deltaTime;
            if(m_currentInterval <= interval)
            {
                if (keys[combo].GetKeyDown())
                {
                    combo++;
                    m_currentInterval = 0f;
                    if(combo == keys.Length)
                    {
                        isTriggered = true;
                        combo = 0;
                        m_currentInterval = 0f;
                    }
                }
            }
            else
            {
                combo = 0;
                m_currentInterval = 0f;
            }
        }

        public override void SetKeyCode(KeyCode keyCode, int index = 0, int subIndex = 0)
        {
            if (index < 0 || index >= keys.Length) return;
            keys[index].SetKeyCode(keyCode, subIndex);
        }

        public override KeyCode GetKeyCode(int index = 0, int subIndex = 0)
        {
            if (index < 0 || index >= keys.Length) return KeyCode.None;
            return keys[index].GetKeyCode(subIndex);
        }

        public override void AddKey(IKey key, int index = 0)
        {
            if (index < 0 || index >= keys.Length) return;
            keys[index].AddKey(key);
        }

        public override void RemoveKey(IKey key, int index = 0)
        {
            if(index < 0 || index >= keys.Length) return;
            keys[index].RemoveKey(key);
        }

        public override Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["name"] = Name;
            json["enable"] = Enable;
            json["interval"] = interval;
            Json arr = new Json(Json.Type.Array);
            Array.ForEach(keys, key => arr.Add(key.Save()));
            json["keys"] = arr;
            return json;
        }

        public override void Load(Json json)
        {
            try
            {
                Name = json["name"];
                SetEnable(json["enable"], false);
                interval = json["interval"];
                List<Json> arr = json["keys"].array;
                for (int i = 0; i < keys.Length; i++)
                {
                    keys[i].Load(arr[i]);
                }
            }
            catch
            {
                ErrorLog.Log(ErrorSetting.jsonAnalysisError);
            }
        }
    }
}
