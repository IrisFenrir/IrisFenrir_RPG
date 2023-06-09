using System;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class PressKey : IVirutalKey
    {
        public SimpleKey key;

        public bool isDown;
        public bool isPressing;
        public bool isUp;
        public float pressTime;

        public PressKey()
        {
            key = new SimpleKey();
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable, includeChildren);
            isDown = false;
            isPressing = false;
            isUp = false;
            pressTime = 0f;
            if (includeChildren)
            {
                key.SetEnable(enable, includeChildren);
            }
        }

        public override void Update(float deltaTime)
        {
            if (!enable) return;

            pressTime = isPressing ? pressTime + deltaTime : 0f;
            isUp = key.GetKeyUp();
            isPressing = key.GetKeyPressing();
            isDown = key.GetKeyDown();
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
            json["key"] = key.Save();
            return json;
        }

        public override void Load(Json json)
        {
            try
            {
                name = json["name"];
                SetEnable(json["enable"]);
                key.Load(json["key"]);
            }
            catch
            {
                Debug.Log(ErrorSetting.jsonFormatErrorMessage);
            }
        }

    }
}
