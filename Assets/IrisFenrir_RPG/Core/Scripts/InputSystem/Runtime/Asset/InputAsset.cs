using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    [CreateAssetMenu(fileName = "New Input Asset", menuName = "IrisFenrirRPG/InputAsset")]
    public class InputAsset : ScriptableObject
    {
        [HideInInspector]
        public List<ObjectHandle> keys;

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= keys.Count)
                return;
            keys.RemoveAt(index);
        }

        public void AddUnknownKey()
        {
            ObjectHandle key = new ObjectHandle();
            AddCommonProperties(key);
            keys.Add(key);
        }

        public void AddAxis(string axisName)
        {
            ObjectHandle key = new ObjectHandle();
            key.AddProperty("canDraw", false);
            key.AddProperty("name", axisName);
            key.AddProperty("range", new Vector2(-1f, 1f));
            key.AddProperty("start", 0f);
            key.AddProperty("posSpeed", new Vector2(5f, 5f));
            key.AddProperty("negSpeed", new Vector2(5f, 5f));
            key.AddProperty("posKey0", KeyCode.None);
            key.AddProperty("negKey0", KeyCode.None);
            key.AddProperty("posKeyCount", 1);
            key.AddProperty("negKeyCount", 1);
            key.AddProperty("isFoldout", true);
            keys.Add(key);
        }

        public void RemoveAxis(string axisName)
        {
            ObjectHandle key = keys.Find(k => k.GetProperty<string>("name") == axisName);
            keys.Remove(key);
        }

        public void ConvertTo(int index, string type)
        {
            switch (type)
            {
                case "TapKey":
                    ConvertToTapKey(index);
                    break;
                case "ValueKey":
                    ConvertToValueKey(index);
                    break;
                case "PressKey":
                    ConvertToPressKey(index);
                    break;
                case "AxisKey":
                    ConvertToAxisKey(index);
                    break;
                case "MultiKey":
                    ConvertToMultiKey(index);
                    break;
                case "ComboKey":
                    ConvertToComboKey(index);
                    break;
            }
        }

        private void AddCommonProperties(ObjectHandle key)
        {
            key.AddProperty("canDraw", true);
            key.AddProperty("name", "Custom Key");
            key.AddProperty("type", "Unknown");
            key.AddProperty("isFoldout", true);
        }

        private string GetDefaultName(string typeName)
        {
            string name = typeName;
            int i = 0;
            while (keys.Exists(k => k.GetProperty<string>("name") == name))
            {
                i++;
                name = typeName + i.ToString();
            }
            return name;
        }

        private void ConvertToTapKey(int index)
        {
            if (index < 0 || index >= keys.Count) return;
            ObjectHandle key = new ObjectHandle();
            AddCommonProperties(key);
            
            key.SetProperty("name", GetDefaultName("TapKey"));
            key.SetProperty("type", "TapKey");

            key.AddProperty("clickCount", 1);
            key.AddProperty("clickInterval", 0.5f);
            key.AddProperty("keyCount", 1);
            key.AddProperty("key0", KeyCode.None);
            keys[index] = key;
        }

        private void ConvertToValueKey(int index)
        {
            if (index < 0 || index >= keys.Count) return;
            ObjectHandle key = new ObjectHandle();
            AddCommonProperties(key);

            key.SetProperty("name", GetDefaultName("ValueKey"));
            key.SetProperty("type", "ValueKey");

            key.AddProperty("range", new Vector2(0f, 1f));
            key.AddProperty("start", 0f);
            key.AddProperty("speed", new Vector2(5f, 5f));
            key.AddProperty("keyCount", 1);
            key.AddProperty("key0", KeyCode.None);
            keys[index] = key;
        }

        private void ConvertToPressKey(int index)
        {
            if (index < 0 || index >= keys.Count) return;
            ObjectHandle key = new ObjectHandle();
            AddCommonProperties(key);
            
            key.SetProperty("name", GetDefaultName("PressKey"));
            key.SetProperty("type", "PressKey");

            key.AddProperty("keyCount", 1);
            key.AddProperty("key0", KeyCode.None);
            keys[index] = key;
        }

        private void ConvertToAxisKey(int index)
        {
            if (index < 0 || index >= keys.Count) return;
            ObjectHandle key = new ObjectHandle();
            AddCommonProperties(key);
            
            key.SetProperty("name", GetDefaultName("AxisKey"));
            key.SetProperty("type", "AxisKey");

            key.AddProperty("axesCount", 0);
            keys[index] = key;
        }

        private void ConvertToMultiKey(int index)
        {
            if (index < 0 || index >= keys.Count) return;
            ObjectHandle key = new ObjectHandle();
            AddCommonProperties(key);

            key.SetProperty("name", GetDefaultName("MultiKey"));
            key.SetProperty("type", "MultiKey");

            key.AddProperty("interval", 0.5f);
            key.AddProperty("keyCount", 2);
            key.AddProperty("key0_0", KeyCode.None);
            key.AddProperty("key1_0", KeyCode.None);
            key.AddProperty("key0Count", 1);
            key.AddProperty("key1Count", 1);

            keys[index] = key;
        }

        private void ConvertToComboKey(int index)
        {
            if (index < 0 || index >= keys.Count) return;
            ObjectHandle key = new ObjectHandle();
            AddCommonProperties(key);

            key.SetProperty("name", GetDefaultName("ComboKey"));
            key.SetProperty("type", "ComboKey");

            key.AddProperty("interval", 0.5f);
            key.AddProperty("keyCount", 2);
            key.AddProperty("key0_0", KeyCode.None);
            key.AddProperty("key1_0", KeyCode.None);
            key.AddProperty("key0Count", 1);
            key.AddProperty("key1Count", 1);

            keys[index] = key;
        }
    }
}
