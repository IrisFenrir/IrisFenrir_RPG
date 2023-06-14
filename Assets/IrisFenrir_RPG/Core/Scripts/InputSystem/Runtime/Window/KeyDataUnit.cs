using System;
using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    [Serializable]
    public class KeyDataUnit
    {
        public enum PropertyType
        {
            None, String, Int, Float, KeyCode, Vector2
        }

        public enum KeyType
        {
            None, TapKey, ValueKey, PressKey, AxisKey, MultiKey, ComboKey, Axis
        }

        public KeyType type = KeyType.None;

        public List<string> propertyNames = new List<string>();

        public List<PropertyType> propertyTypes = new List<PropertyType>();

        public List<int> propertyIndex = new List<int>();

        [SerializeField]
        private PropertyReader<string> m_stringProperties = new PropertyReader<string>(PropertyType.String);
        [SerializeField]
        private PropertyReader<int> m_intProperties = new PropertyReader<int>(PropertyType.Int);
        [SerializeField]
        private PropertyReader<float> m_floatProperties = new PropertyReader<float>(PropertyType.Float);
        [SerializeField]
        private PropertyReader<KeyCode> m_keyCodeProperties = new PropertyReader<KeyCode>(PropertyType.KeyCode);
        [SerializeField]
        private PropertyReader<Vector2> m_vector2Properties = new PropertyReader<Vector2>(PropertyType.Vector2);

        public List<KeyDataUnit> subKeys = new List<KeyDataUnit>();

        public void ClearAll()
        {
            propertyNames.Clear();
            propertyTypes.Clear();
            propertyIndex.Clear();

            m_stringProperties.Clear();
            m_intProperties.Clear();
            m_floatProperties.Clear();
            m_keyCodeProperties.Clear();
            m_vector2Properties.Clear();
        }

        public void AddStringProperty(string propertyName, string propertyValue)
        {
            m_stringProperties.Add(this, propertyName, propertyValue);
        }
        public void RemoveStringProperty(string propertyName)
        {
            m_stringProperties.Remove(this, propertyName);
        }
        public void SetStringProperty(string propertyName, string propertyValue)
        {
            m_stringProperties.Set(this, propertyName, propertyValue);
        }
        public string GetStringProperty(string propertyName)
        {
            return m_stringProperties.Get(this, propertyName);
        }

        public void AddIntProperty(string propertyName, int propertyValue)
        {
            m_intProperties.Add(this, propertyName, propertyValue);
        }
        public void RemoveIntProperty(string propertyName)
        {
            m_intProperties.Remove(this, propertyName);
        }
        public void SetIntProperty(string propertyName, int propertyValue)
        {
            m_intProperties.Set(this, propertyName, propertyValue);
        }
        public int GetIntProperty(string propertyName)
        {
            return m_intProperties.Get(this, propertyName);
        }

        public void AddFloatProperty(string propertyName, float propertyValue)
        {
            m_floatProperties.Add(this, propertyName, propertyValue);
        }
        public void RemoveFloatProperty(string propertyName)
        {
            m_floatProperties.Remove(this, propertyName);
        }
        public void SetFloatProperty(string propertyName, float propertyValue)
        {
            m_floatProperties.Set(this, propertyName, propertyValue);
        }
        public float GetFloatProperty(string propertyName)
        {
            return m_floatProperties.Get(this, propertyName);
        }

        public void AddKeyCodeProperty(string propertyName, KeyCode propertyValue)
        {
            m_keyCodeProperties.Add(this, propertyName, propertyValue);
        }
        public void RemoveKeyCodeProperty(string propertyName)
        {
            m_keyCodeProperties.Remove(this, propertyName);
        }
        public void SetKeyCodeProperty(string propertyName, KeyCode propertyValue)
        {
            m_keyCodeProperties.Set(this, propertyName, propertyValue);
        }
        public KeyCode GetKeyCodeProperty(string propertyName)
        {
            return m_keyCodeProperties.Get(this, propertyName);
        }

        public void AddVector2Property(string propertyName, Vector2 propertyValue)
        {
            m_vector2Properties.Add(this, propertyName, propertyValue);
        }
        public void RemoveVector2Property(string propertyName)
        {
            m_vector2Properties.Remove(this, propertyName);
        }
        public void SetVector2Property(string propertyName, Vector2 propertyValue)
        {
            m_vector2Properties.Set(this, propertyName, propertyValue);
        }
        public Vector2 GetVector2Property(string propertyName)
        {
            return m_vector2Properties.Get(this, propertyName);
        }
    }
}
