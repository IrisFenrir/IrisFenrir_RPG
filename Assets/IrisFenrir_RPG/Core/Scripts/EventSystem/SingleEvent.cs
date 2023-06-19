using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisFenrir.EventSystem
{
    public class SingleEvent : IEvent
    {
        private Func<bool> m_condition;
        private Action m_action;

        public SingleEvent(string name) : base(name)
        {
            
        }

        public virtual SingleEvent Register(Func<bool> condition)
        {
            m_condition += condition;
            return this;
        }

        public virtual SingleEvent Unregister(Func<bool> condition)
        {
            m_condition -= condition;
            return this;
        }

        public virtual SingleEvent Subscribe(params Action[] actions)
        {
            Array.ForEach(actions, act => m_action += act);
            return this;
        }

        public virtual SingleEvent Unsubscribe(params Action[] actions)
        {
            Array.ForEach(actions, act => m_action -= act);
            return this;
        }

        public override void Update(float deltaTime)
        {
            if (!enable) return;
            if (m_condition != null && m_condition())
                m_action?.Invoke();
        }
    }

    public class SingleEvent<T> : IEvent
    {
        private Func<bool> m_condition;
        private Action<T> m_action;
        private Func<T> m_parameter;

        public SingleEvent(string name) : base(name)
        {
        }

        public SingleEvent<T> Register(Func<bool> condition, Func<T> parameter)
        {
            m_condition += condition;
            m_parameter += parameter;
            return this;
        }

        public SingleEvent<T> Unregister(Func<bool> condition, Func<T> parameter)
        {
            m_condition -= condition;
            m_parameter -= parameter;
            return this;
        }

        public SingleEvent<T> Subscribe(params Action<T>[] actions)
        {
            Array.ForEach(actions, act => m_action += act);
            return this;
        }

        public SingleEvent<T> Unsubscribe(params Action<T>[] actions)
        {
            Array.ForEach(actions, act => m_action -= act);
            return this;
        }

        public override void Update(float deltaTime)
        {
            if (!enable) return;
            if (m_condition != null && m_condition())
                m_action?.Invoke(m_parameter());
        }
    }
}
