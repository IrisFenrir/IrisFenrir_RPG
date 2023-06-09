using System;
using System.Collections.Generic;

namespace IrisFenrir
{
    public class Json
    {
        public enum Type
        {
            Object,
            Array,
            Number,
            String,
            Boolean
        }

        public Type type { get; set; }
        public string json { get; set; }

        public Dictionary<string, Json> map;
        public List<Json> array;

        public Json() { }

        public Json(Type type)
        {
            this.type = type;
            if (type == Type.Object)
                map = new Dictionary<string, Json>();
            else if (type == Type.Array)
                array = new List<Json>();
        }

        public static implicit operator Json(double value)
        {
            return new Json(Type.Number) { json = value.ToString() };
        }

        public static implicit operator double(Json json)
        {
            if (json == null || json.type != Type.Number || !double.TryParse(json.json, out double value))
                return default;
            return value;
        }

        public static implicit operator Json(int value)
        {
            return new Json(Type.Number) { json = value.ToString() };
        }

        public static implicit operator int(Json json)
        {
            if (json == null || json.type != Type.Number || !int.TryParse(json.json, out int value))
                return default;
            return value;
        }

        public static implicit operator Json(float value)
        {
            return new Json(Type.Number) { json = value.ToString() };
        }

        public static implicit operator float(Json json)
        {
            if (json == null || json.type != Type.Number || !float.TryParse(json.json, out float value))
                return default;
            return value;
        }

        public static implicit operator Json(bool value)
        {
            return new Json(Type.Boolean) { json = value.ToString().ToLower() };
        }

        public static implicit operator bool(Json json)
        {
            if (json == null || json.type != Type.Boolean || !bool.TryParse(json.json, out bool value))
                return default;
            return value;
        }

        public static implicit operator Json(string value)
        {
            return new Json(Type.String) { json = "\"" + value + "\"" };
        }

        public static implicit operator string(Json json)
        {
            if (json == null || json.type != Type.String)
                return default;
            return json.json.Substring(1, json.json.Length - 2);
        }

        public void Add(string value)
        {
            if (array == null) return;
            array.Add(value);
        }

        public void Add(bool value)
        {
            if (array == null) return;
            array.Add(value);
        }

        public void Add(int value)
        {
            if (array == null) return;
            array.Add(value);
        }

        public void Add(double value)
        {
            if (array == null) return;
            Json number = value;
            array.Add(number);
        }

        public void Add(Json json)
        {
            if (array == null || json == null) return;
            array.Add(json);
        }

        public bool HasKey(string key)
        {
            if (map == null) return false;
            return map.ContainsKey(key);
        }

        public Json this[string key]
        {
            get
            {
                if (map == null)
                    return null;
                if (map.TryGetValue(key, out Json j))
                    return j;
                return null;
            }
            set
            {
                if (map == null) return;
                if (map.ContainsKey(key))
                    map[key] = value;
                else
                    map.Add(key, value);
            }
        }

        public Json this[int index]
        {
            get
            {
                if (array == null || index < 0 || index >= array.Count)
                    return null;
                return array[index];
            }
            set
            {
                if (array == null || index < 0 || index >= array.Count)
                    return;
                array[index] = value;
            }
        }
    }
}
