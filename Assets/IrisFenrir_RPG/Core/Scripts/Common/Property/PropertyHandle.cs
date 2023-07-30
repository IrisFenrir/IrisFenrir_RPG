using System;
using UnityEngine;

namespace IrisFenrir
{
    [Serializable]
    public class PropertyHandle
    {
        public string content;

        public void SetProperty<T>(T value)
        {
            content = value.ToString();
        }

        public T GetProperty<T>()
        {
            try
            {
                Type type = typeof(T);
                if (type == typeof(int))
                    return (T)(object)int.Parse(content);
                else if (type == typeof(float))
                    return (T)(object)float.Parse(content);
                else if (type == typeof(bool))
                    return (T)(object)bool.Parse(content);
                else if (type == typeof(string))
                    return (T)(object)content;
                else if (type.IsEnum)
                    return (T)Enum.Parse(type, content);
                else if (type == typeof(Vector2))
                    return (T)(object)String2Vector2(content);
                else if (type == typeof(Vector3))
                    return (T)(object)String2Vector3(content);

                return default;
            }
            catch
            {
                return default;
            }
        }

        private Vector2 String2Vector2(string str)
        {
            str = str[1..^1];
            string[] values = str.Split(',');
            return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
        }

        private Vector3 String2Vector3(string str)
        {
            str = str[1..^1];
            string[] values = str.Split(',');
            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }
    }
}
