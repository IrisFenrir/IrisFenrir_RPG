using System;
using System.Collections.Generic;

namespace IrisFenrir
{
    [Serializable]
    public class ObjectHandle
    {
        public List<string> propertyNames = new List<string>();
        public List<PropertyHandle> properties = new List<PropertyHandle>();

        public void AddProperty<T>(string name, T value)
        {
            propertyNames.Add(name);
            PropertyHandle property = new PropertyHandle();
            property.SetProperty(value);
            properties.Add(property);
        }

        public void RemoveProperty(string name)
        {
            int index = propertyNames.IndexOf(name);
            if (index < 0) return;
            propertyNames.RemoveAt(index);
            properties.RemoveAt(index);
        }

        public void SetProperty<T>(string name, T value)
        {
            int index = propertyNames.IndexOf(name);
            if (index < 0) return;
            properties[index].SetProperty(value);
        }

        public T GetProperty<T>(string name)
        {
            int index = propertyNames.IndexOf(name);
            if (index < 0) return default;
            return properties[index].GetProperty<T>();
        }
    }
}
