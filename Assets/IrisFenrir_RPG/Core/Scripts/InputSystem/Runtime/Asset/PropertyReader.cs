using System;
using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.InputSystem
{
    [Serializable]
    public class PropertyReader<T>
    {

        public KeyDataUnit.PropertyType type;

        [SerializeField]
        private List<T> m_propertyValues = new List<T>();
        [SerializeField]
        private Queue<int> m_avaliableIndex = new Queue<int>();

        public PropertyReader(KeyDataUnit.PropertyType type)
        {
            this.type = type;
        }

        private int AllocateIndex()
        {
            if(m_avaliableIndex.Count == 0)
            {
                m_propertyValues.Add(default);
                return m_propertyValues.Count - 1;
            }
            return m_avaliableIndex.Dequeue();
        }

        private void CollectIndex(int index)
        {
            m_avaliableIndex.Enqueue(index);
        }

        public void Add(KeyDataUnit data, string propertyName, T propertyValue)
        {
            if(data.propertyNames.Contains(propertyName))
            {
                Debug.Log($"名称为{propertyName}的键已存在！");
                return;
            }

            int index;
            for (index = 0; index < data.propertyIndex.Count && data.propertyIndex[index] >= 0; index++) ;
            if (index == data.propertyIndex.Count)
            {
                data.propertyNames.Add(default);
                data.propertyTypes.Add(default);
                data.propertyIndex.Add(default);
            }

            int propIndex = AllocateIndex();

            data.propertyNames[index] = propertyName;
            data.propertyTypes[index] = type;
            data.propertyIndex[index] = propIndex;
            m_propertyValues[propIndex] = propertyValue;
        }

        public void Remove(KeyDataUnit data, string propertyName)
        {
            int index = data.propertyNames.FindIndex(p => p == propertyName);
            if (index < 0) return;

            data.propertyNames[index] = "None";
            data.propertyTypes[index] = KeyDataUnit.PropertyType.None;
            CollectIndex(data.propertyIndex[index]);
            data.propertyIndex[index] = -1;
        }

        public void Set(KeyDataUnit data, string propertyName, T value)
        {
            int index = data.propertyNames.FindIndex(p => p == propertyName);
            if (index < 0) return;

            m_propertyValues[data.propertyIndex[index]] = value;
        }

        public T Get(KeyDataUnit data, string propertyName)
        {
            int index = data.propertyNames.FindIndex(p => p == propertyName);
            if (index < 0) return default;

            return m_propertyValues[data.propertyIndex[index]];
        }

        public void Clear()
        {
            m_propertyValues.Clear();
            m_avaliableIndex.Clear();
        }
    }
}
