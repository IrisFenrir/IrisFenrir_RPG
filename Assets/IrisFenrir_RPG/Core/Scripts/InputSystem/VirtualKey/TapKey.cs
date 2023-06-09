﻿using System;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class TapKey : IVirutalKey
    {
        public int clickCount = 1;
        public float clickInterval = 0.5f;
        public SimpleKey key;

        public bool isTriggered;
        public int currentCount;

        private float m_currentClickInterval;

        public TapKey()
        {
            key = new SimpleKey();
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable, includeChildren);
            isTriggered = false;
            currentCount = 0;
            m_currentClickInterval = 0f;
            if(includeChildren)
            {
                key.SetEnable(enable, includeChildren);
            }
        }

        public override void Update(float deltaTime)
        {
            if (!enable || clickCount < 1) return;

            isTriggered = false;
            if (clickCount == 1)
            {
                isTriggered = key.GetKeyDown();
            }
            if (clickInterval <= 0f) return;
            m_currentClickInterval += deltaTime;
            if (m_currentClickInterval <= clickInterval)
            {
                if (key.GetKeyDown())
                {
                    currentCount++;
                    m_currentClickInterval = 0f;
                    if (currentCount >= clickCount)
                    {
                        isTriggered = true;
                        currentCount = 0;
                    }
                }
            }
            else
            {
                currentCount = 0;
                m_currentClickInterval = 0f;
            }
        }

        public override void SetKeyCode(KeyCode keyCode, int index = 0)
        {
            key.SetKeyCode(keyCode);
        }

        public override KeyCode GetKeyCode(int index = 0)
        {
            return key.GetKeyCode();
        }

        public override void AddKey(IKey key, int index = 0)
        {
            this.key.AddKey(key);
        }

        public override void RemoveKey(IKey key, int index = 0)
        {
            this.key.RemoveKey(key);
        }

        public override Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["name"] = name;
            json["enable"] = enable;
            json["clickCount"] = clickCount;
            json["clickInterval"] = clickInterval;
            json["key"] = key.Save();
            return json;
        }

        public override void Load(Json json)
        {
            try
            {
                name = json["name"];
                SetEnable(json["enable"]);
                clickCount = json["clickCount"];
                clickInterval = json["clickInterval"];
                key.Load(json["key"]);
            }
            catch
            {
                Debug.Log(ErrorSetting.jsonFormatErrorMessage);
            }
        }

        
    }
}
