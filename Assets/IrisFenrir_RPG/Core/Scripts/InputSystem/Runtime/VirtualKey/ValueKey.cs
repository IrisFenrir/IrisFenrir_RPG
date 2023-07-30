using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class ValueKey : IVirutalKey
    {
        public Vector2 range = new Vector2(0f, 1f);
        public float start = 0f;
        public Vector2 speed = 5f * Vector2.one;
        public SimpleKey key;

        public float value = 0f;     

        public ValueKey()
        {
            key = new SimpleKey();
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable, includeChildren);
            value = start;
            if(includeChildren)
            {
                key.SetEnable(enable, includeChildren);
            }
        }

        public override void Update(float deltaTime)
        {
            if (speed.x <= 0 || speed.y <= 0 || range.x >= range.y ||
                start >= range.y || start < range.x) return;

            if (Enable && key.GetKeyPressing())
            {
                if (value < range.y)
                {
                    value += speed.x * deltaTime;
                    value = Mathf.Min(value, range.y);
                }
            }
            else
            {
                if (value > range.x)
                {
                    value -= speed.y * deltaTime;
                    value = Mathf.Max(value, range.x);
                }
            }
        }

        public override void SetKeyCode(KeyCode keyCode, int index = 0, int subIndex = 0)
        {
            key.SetKeyCode(keyCode, subIndex);
        }

        public override KeyCode GetKeyCode(int index = 0, int subIndex = 0)
        {
            return key.GetKeyCode(subIndex);
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
            json["name"] = Name;
            json["enable"] = Enable;
            json["range.x"] = range.x;
            json["range.y"] = range.y;
            json["start"] = start;
            json["speed.x"] = speed.x;
            json["speed.y"] = speed.y;
            json["key"] = key.Save();
            return json;
        }

        public override void Load(Json json)
        {
            try
            {
                Name = json["name"];
                SetEnable(json["enable"], false);
                range.x = json["range.x"];
                range.y = json["range.y"];
                start = json["start"];
                speed.x = json["speed.x"];
                speed.y = json["speed.y"];
                key.Load(json["key"]);
            }
            catch
            {
                ErrorLog.Log(ErrorSetting.jsonAnalysisError);
            }
        }

        
    }
}
