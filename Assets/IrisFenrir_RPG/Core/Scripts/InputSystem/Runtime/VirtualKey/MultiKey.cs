using System;
using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class MultiKey : IVirutalKey
    {
        public float interval = 0.5f;
        public SimpleKey[] keys;

        public bool isTriggered;

        public override int KeyCount => keys.Length;

        private float m_currentInterval;
        private bool[] m_keyState;

        public MultiKey(int count)
        {
            keys = new SimpleKey[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = new SimpleKey();
            }
            m_keyState = new bool[count];
        }

        public bool GetState(int index)
        {
            if (index < 0 || index >= keys.Length)
                return false;
            return m_keyState[index];
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable);
            m_currentInterval = 0f;
            Array.Fill(m_keyState, false);
            if (includeChildren)
                Array.ForEach(keys, key => key.SetEnable(enable));
        }

        public override void Update(float deltaTime)
        {
            if (!Enable || keys.Length == 0 || interval <= 0f) return;

            isTriggered = false;
            m_currentInterval += deltaTime;
            if(m_currentInterval <= interval)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i].GetKeyDown())
                    {
                        m_keyState[i] = true;
                    }
                }
                if (Array.TrueForAll(m_keyState, s => s))
                {
                    isTriggered = true;
                    m_currentInterval = 0f;
                    Array.Fill(m_keyState, false);
                }
            }
            else
            {
                m_currentInterval = 0f;
                Array.Fill(m_keyState, false);
            }
        }

        public override void SetKeyCode(KeyCode keyCode, int index = 0, int subIndex = 0)
        {
            if (index < 0 || index >= keys.Length) return;
            keys[index].SetKeyCode(keyCode, subIndex);
        }

        public override KeyCode GetKeyCode(int index = 0, int subIndex = 0)
        {
            if (index < 0 || index >= keys.Length)
                return KeyCode.None;
            return keys[index].GetKeyCode(subIndex);
        }

        public override void AddKey(IKey key, int index = 0)
        {
            if (index < 0 || index >= keys.Length) return;
            keys[index].AddKey(key);
        }

        public override void RemoveKey(IKey key, int index = 0)
        {
            if (index < 0 || index >= keys.Length) return;
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
