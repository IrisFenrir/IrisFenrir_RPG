using System;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class InputHelper
    {
        public static bool GetInputKeyOnGUI(out KeyCode key)
        {
            key = KeyCode.None;
            if(Input.anyKeyDown)
            {
                if(Event.current.type == EventType.KeyDown)
                {
                    key = Event.current.keyCode;
                    return true;
                }
                else if(Event.current.type == EventType.MouseDown)
                {
                    key = Enum.Parse<KeyCode>($"Mouse{Event.current.button}");
                }
            }
            return false;
        }

        public static bool GetInputKey(out KeyCode key)
        {
            key = KeyCode.None;
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(keyCode))
                {
                    key = keyCode;
                    return true;
                }
            }
            return false;
        }
    }
}
