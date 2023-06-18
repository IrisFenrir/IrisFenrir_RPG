using System;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class RealKey : IKey
    {
        public KeyCode keyCode;

        public override bool GetKeyPressing()
        {
            if (!enable) return false;
            return Input.GetKey(keyCode);
        }

        public override bool GetKeyDown()
        {
            if (!enable) return false;
            return Input.GetKeyDown(keyCode);
        }

        public override bool GetKeyUp()
        {
            if (!enable) return false;
            return Input.GetKeyUp(keyCode);
        }

        public override Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["enable"] = enable;
            json["keyCode"] = keyCode.ToString();
            return json;
        }

        public override void Load(Json json)
        {
            try
            {
                SetEnable(json["enable"]);
                keyCode = Enum.Parse<KeyCode>(json["keyCode"]);
            }
            catch
            {
                ErrorLog.Log(ErrorSetting.jsonAnalysisError);
            }
        }
    }
}
