using System;
using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public class Axis : IVirutalKey
    {
        public Vector2 range = new Vector2(-1f, 1f);
        public Vector2 posSpeed = new Vector2(5f, 5f);
        public Vector2 negSpeed = new Vector2(5f, 5f);
        public float start = 0f;
        public SimpleKey posKey;
        public SimpleKey negKey;

        public float value = 0f;

        public Axis()
        {
            posKey = new SimpleKey();
            negKey = new SimpleKey();
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable, includeChildren);
            value = start;
            if(includeChildren)
            {
                posKey.SetEnable(enable, includeChildren);
                negKey.SetEnable(enable, includeChildren);
            }
        }

        public override void Update(float deltaTime)
        {
            if (posSpeed.x < 0 || posSpeed.y < 0 || negSpeed.x < 0 || negSpeed.y < 0 ||
                start < range.x || start >= range.y)
                return;

            if(enable && posKey.GetKeyPressing())
            {
                value = Mathf.Min(value + (value >= start ? posSpeed.x : negSpeed.y) * deltaTime, range.y);
            }
            else if(enable && negKey.GetKeyPressing())
            {
                value = Mathf.Max(value - (value >= start ? posSpeed.y : negSpeed.x) * deltaTime, range.x);
            }
            else
            {
                if(value > start)
                {
                    value = Mathf.Max(value - posSpeed.y * deltaTime, start);
                }
                else if(value < start)
                {
                    value = Mathf.Min(value + negSpeed.y * deltaTime, start);
                }
            }
        }

        public override void SetKeyCode(KeyCode keyCode, int index = 0)
        {
            if (index == 0)
                posKey.SetKeyCode(keyCode);
            else if (index == 1)
                negKey.SetKeyCode(keyCode);
        }

        public override KeyCode GetKeyCode(int index = 0)
        {
            if (index == 0)
                return posKey.GetKeyCode();
            else if (index == 1)
                return negKey.GetKeyCode();
            return KeyCode.None;
        }

        public override void AddKey(IKey key, int index = 0)
        {
            if (index == 0)
                posKey.AddKey(key);
            else if (index == 1)
                negKey.AddKey(key);
        }

        public override void RemoveKey(IKey key, int index = 0)
        {
            if (index == 0)
                posKey.RemoveKey(key);
            else if (index == 1)
                negKey.RemoveKey(key);
        }
        public override Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["name"] = name;
            json["enable"] = enable;
            json["range.x"] = range.x;
            json["range.y"] = range.y;
            json["posSpeed.x"] = posSpeed.x;
            json["posSpeed.y"] = posSpeed.y;
            json["negSpeed.x"] = negSpeed.x;
            json["negSpeed.y"] = negSpeed.y;
            json["start"] = start;
            json["posKey"] = posKey.Save();
            json["negKey"] = negKey.Save();
            return json;
        }

        public override void Load(Json json)
        {
            try
            {
                name = json["name"];
                SetEnable(json["enable"]);
                range.x = json["range.x"];
                range.y = json["range.y"];
                posSpeed.x = json["posSpeed.x"];
                posSpeed.y = json["posSpeed.y"];
                negSpeed.x = json["negSpeed.x"];
                negSpeed.y = json["negSpeed.y"];
                start = json["start"];
                posKey.Load(json["posKey"]);
                negKey.Load(json["negKey"]);
            }
            catch
            {
                Debug.Log(ErrorSetting.jsonFormatErrorMessage);
            }
        }

    }

    public class AxisKey : IVirutalKey
    {
        public Axis[] axes { get; private set; }

        private Vector2 m_output2d;
        private Vector3 m_output3d;

        public AxisKey(int count)
        {
            axes = new Axis[count];
            for (int i = 0; i < count; i++)
            {
                axes[i] = new Axis();
            }
        }

        public void AddAxis(int index, KeyCode posKey, KeyCode negKey, Vector2? range = null, float start = 0, Vector2? posSpeed = null, Vector2? negSpeed = null)
        {
            Axis axis = axes[index];
            axis.posKey.SetKeyCode(posKey);
            axis.negKey.SetKeyCode(negKey);
            if (range != null)
                axis.range = range.Value;
            if (posSpeed != null)
                axis.posSpeed = posSpeed.Value;
            if (negSpeed != null)
                axis.negSpeed = negSpeed.Value;
            axis.start = start;
        }

        public float GetValue(int index)
        {
            if (index < 0 || index >= axes.Length) return 0f;
            return axes[index].value;
        }

        public Vector2 GetVector2()
        {
            if (axes.Length < 2) return Vector2.zero;
            m_output2d.Set(axes[0].value, axes[1].value);
            return m_output2d;
        }

        public Vector3 GetVector3()
        {
            if (axes.Length < 3) return Vector3.zero;
            m_output3d.Set(axes[0].value, axes[1].value, axes[2].value);
            return m_output3d;
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable, includeChildren);
            if(includeChildren)
                Array.ForEach(axes, axis => axis.SetEnable(enable, includeChildren));
        }

        public override void Update(float deltaTime)
        {
            Array.ForEach(axes, axis => axis?.Update(deltaTime));
        }

        // index % 2 == 0  ==> posKey
        // index % 2 == 1  ==> negKey 
        public override void SetKeyCode(KeyCode keyCode, int index = 0)
        {
            int axisIndex = index / 2;
            if (axisIndex < 0 || axisIndex >= axes.Length) return;
            axes[axisIndex].SetKeyCode(keyCode, index % 2);
        }

        public override KeyCode GetKeyCode(int index = 0)
        {
            int axisIndex = index / 2;
            if (axisIndex < 0 || axisIndex >= axes.Length)
                return KeyCode.None;
            return axes[axisIndex].GetKeyCode(index % 2);
        }

        public override void AddKey(IKey key, int index = 0)
        {
            int axisIndex = index / 2;
            if (axisIndex < 0 || axisIndex >= axes.Length) return;
            axes[axisIndex].AddKey(key, index % 2);
        }

        public override void RemoveKey(IKey key, int index = 0)
        {
            int axisIndex = index / 2;
            if (axisIndex < 0 || axisIndex >= axes.Length) return;
            axes[axisIndex].RemoveKey(key, index % 2);
        }

        public override Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["name"] = name;
            json["enable"] = enable;
            Json arr = new Json(Json.Type.Array);
            Array.ForEach(axes, axis => arr.Add(axis.Save()));
            json["axes"] = arr;
            return json;
        }

        public override void Load(Json json)
        {
            try
            {
                name = json["name"];
                SetEnable(json["enable"], false);
                List<Json> arr = json["axes"].array;
                for (int i = 0; i < axes.Length; i++)
                {
                    axes[i].Load(arr[i]);
                }
            }
            catch
            {
                Debug.Log(ErrorSetting.jsonFormatErrorMessage);
            }
        }

    }
}
