using System.Collections.Generic;
using System.Text;

namespace IrisFenrir
{

    public static class JsonMapper
    {
        public static Json ToJsonObject(string jsonText)
        {
            int index = 0;
            return ProcessJsonString(jsonText, ref index);
        }

        public static string ToJsonString(Json json)
        {
            if (json == null) return null;

            StringBuilder stringBuilder = new StringBuilder();
            ProcessJsonObject(json, stringBuilder);

            return stringBuilder.ToString();
        }

        private static Json ProcessJsonString(string json, ref int index)
        {
            if (index < 0 || index >= json.Length)
            {
                return null;
            }

            SkipWhiteSpace(json, ref index);

            char cur = json[index];
            Json js = null;
            if (cur == '\"')
            {
                js = ToString(json, ref index);
            }
            else if (cur == 'f' || cur == 't')
            {
                js = ToBoolean(json, ref index);
            }
            else if (cur == '-' || char.IsDigit(cur))
            {
                js = ToNumber(json, ref index);
            }
            else if (cur == '{')
            {
                js = ToObject(json, ref index);
            }
            else if (cur == '[')
            {
                js = ToArray(json, ref index);
            }

            SkipWhiteSpace(json, ref index);
            return js;
        }

        private static void SkipWhiteSpace(string json, ref int index)
        {
            if (index < 0) return;
            while (index < json.Length && char.IsWhiteSpace(json[index]))
                index++;
        }

        private static Json ToString(string json, ref int index)
        {
            if (index < 0 || index >= json.Length || json[index] != '\"')
            {
                return null;
            }

            int start = index++;
            while (index < json.Length && json[index] != '\"') index++;
            if (index >= json.Length)
            {
                return null;
            }

            return new Json(Json.Type.String) { json = json[start..(++index)]};
        }

        private static Json ToBoolean(string json, ref int index)
        {
            if (index < 0)
            {
                return null;
            }

            if (index + 3 < json.Length && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
            {
                index += 4;
                return new Json(Json.Type.Boolean) { json = "true" };
            }
            else if (index + 4 < json.Length && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
            {
                index += 5;
                return new Json(Json.Type.Boolean) { json = "false"};
            }

            return null;
        }

        private static Json ToNumber(string json, ref int index)
        {
            if (index < 0 || index >= json.Length)
            {
                return null;
            }

            int start = index;
            if (json[index] == '-')
                index++;

            while (index < json.Length && char.IsNumber(json[index]))
                index++;
            if (index >= json.Length || (json[index] != '.' && json[index] != 'e' && json[index] != 'E'))
                return new Json(Json.Type.Number) { json = json[start..index] };

            if (json[index] == '.')
            {
                int pointIndex = index++;
                while (index < json.Length && char.IsDigit(json[index])) index++;
                if (index == pointIndex + 1)
                {
                    return null;
                }
                if (index >= json.Length || (json[index] != 'e' && json[index] != 'E'))
                    return new Json(Json.Type.Number) { json = json[start..index] };
            }

            int eIndex = index++;
            if (json[index] == '+' || json[index] == '-') index++;
            while (index < json.Length && char.IsDigit(json[index])) index++;
            if (index == eIndex + 1)
            {
                return null;
            }
            return new Json(Json.Type.Number) { json = json[start..index] };
        }

        private static Json ToObject(string json, ref int index)
        {
            if (index < 0 || index >= json.Length || json[index] != '{')
            {
                return null;
            }

            int start = index++;

            Json obj = new Json(Json.Type.Object);

            do
            {
                if (json[index] == ',')
                    index++;
                SkipWhiteSpace(json, ref index);
                if (json[index] != '"')
                {
                    return null;
                }

                int keyStart = index++;
                while (index < json.Length && json[index] != '"')
                    index++;
                if (index >= json.Length)
                {
                    return null;
                }
                index++;
                string key = json[(keyStart + 1)..(index - 1)];
                if (obj.HasKey(key))
                {
                    return null;
                }
                SkipWhiteSpace(json, ref index);

                if (json[index] != ':')
                {
                    return null;
                }
                index++;
                SkipWhiteSpace(json, ref index);
                Json sub = ProcessJsonString(json, ref index);
                if (sub == null) return null;

                obj[key] = sub;
                SkipWhiteSpace(json, ref index);
            } while (json[index] == ',');

            if (json[index] == '}')
            {
                obj.json = json[start..(++index)];
                return obj;
            }

            return null;
        }

        private static Json ToArray(string json, ref int index)
        {
            if (index < 0 || index >= json.Length || json[index] != '[')
            {
                return null;
            }

            int start = index++;

            Json arr = new Json(Json.Type.Array);
            do
            {
                if (json[index] == ',')
                    index++;
                SkipWhiteSpace(json, ref index);
                Json sub = ProcessJsonString(json, ref index);
                if (sub == null) return null;
                arr.Add(sub);
                SkipWhiteSpace(json, ref index);
            } while (index < json.Length && json[index] == ',');

            if (index >= json.Length || json[index] != ']')
            {
                return null;
            }

            index++;
            arr.json = json[start..index];
            return arr;
        }


        private static void ProcessJsonObject(Json json, StringBuilder builder)
        {
            if (json == null || builder == null)
                return;

            switch(json.type)
            {
                case Json.Type.String:
                case Json.Type.Boolean:
                case Json.Type.Number:
                    builder.Append(json.json);
                    break;
                case Json.Type.Object:
                    builder.Append('{');
                    foreach (var key in json.map.Keys)
                    {
                        builder.Append("\"" + key + "\":");
                        ProcessJsonObject(json[key], builder);
                        builder.Append(',');
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append('}');
                    break;
                case Json.Type.Array:
                    builder.Append('[');
                    foreach (var item in json.array)
                    {
                        ProcessJsonObject(item, builder);
                        builder.Append(",");
                    }
                    builder.Remove(builder.Length-1, 1);
                    builder.Append("]");
                    break;
            }
        }
    }
}
