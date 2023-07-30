using System;
using System.Collections.Generic;

namespace IrisFenrir.EventSystem
{
    public class EventSystem : Singleton<EventSystem>, IUpdater, IEnable
    {
        public bool Enable { get; private set; }

        private EventGroup m_root = new EventGroup("Root");

        public bool GetEnable()
        {
            return Enable;
        }

        public void SetEnable(bool enable, bool includeChildren = true)
        {
            this.Enable = enable;
            if (includeChildren)
                m_root.SetEnable(enable, includeChildren);
        }

        public void Init() { }


        public void Stop() { }

        public void Update(float deltaTime)
        {
            if (!Enable) return;

            m_root.Update(deltaTime);
        }

        public static T FindWithName<T>(string name, string root = "Root") where T: IEvent
        {
            if (root == "Root")
                return Find<T>(e => e.Name == name);
            var rootEvent = Find<IEvent>(e => e.Name == root);
            if (rootEvent == null) return null;
            return EventFilter<T>(rootEvent, e => e.Name == root);
        }

        public static T FindWithPath<T>(string path) where T: IEvent
        {
            string[] eventNames = path.Split('/');
            EventGroup group = Instance.m_root;
            int i;
            for (i = 0; i < eventNames.Length - 1; i++)
            {
                group = group.Events.Find(e => e.Name == eventNames[i]) as EventGroup;
                if (group == null) return null;
            }
            return group.Events.Find(e => e.Name == eventNames[i]) as T;
        }

        public static T Find<T>(Predicate<T> condition) where T : IEvent
        {
            return EventFilter(Instance.m_root, condition);
        }

        public static SingleEvent CreateEvent(string parent, string name)
        {
            EventGroup group = FindWithName<EventGroup>(parent);
            if (group == null) return null;
            SingleEvent e = group.Events.Find(e => e.Name == name) as SingleEvent;
            if (e != null) return e;
            e = new SingleEvent(name);
            group.AddEvent(e);
            return e;
        }

        public static SingleEvent<T> CreateEvent<T>(string parent, string name)
        {
            EventGroup group = FindWithName<EventGroup>(parent);
            if (group == null) return null;
            SingleEvent<T> e = group.Events.Find(e => e.Name == name) as SingleEvent<T>;
            if (e != null) return e;
            e = new SingleEvent<T>(name);
            group.AddEvent(e);
            return e;
        }

        public static EventGroup CreateEventGroup(string parent, string name)
        {
            EventGroup group = FindWithName<EventGroup>(parent);
            if (group == null) return null;
            EventGroup e = group.Events.Find(e => e.Name == name) as EventGroup;
            if (e != null) return e;
            e = new EventGroup(name);
            group.AddEvent(e);
            return e;
        }

        public static SingleEvent[] CreateEvents(string parent, params string[] names)
        {
            EventGroup group = FindWithName<EventGroup>(parent);
            if (group == null) return null;
            return group.CreateEvents(names);
        }

        public static SingleEvent<T>[] CreateEvents<T>(string parent, params string[] names)
        {
            EventGroup group = FindWithName<EventGroup>(parent);
            if (group == null) return null;
            return group.CreateEvents<T>(names);
        }

        public static EventGroup[] CreateEventGroups(string parent, params string[] names)
        {
            EventGroup group = FindWithName<EventGroup>(parent);
            if (group == null) return null;
            return group.CreateEventGroups(names);
        }

        public static SingleEvent Register(string name, Func<bool> condition)
        {
            return EventFunction<SingleEvent>(name, e => e.Register(condition));
        }
        public static SingleEvent<T> Register<T>(string name, Func<bool> condition, Func<T> parameter)
        {
            return EventFunction<SingleEvent<T>>(name, e => e.Register(condition, parameter));
        }

        public static SingleEvent Unregister(string name, Func<bool> condition)
        {
            return EventFunction<SingleEvent>(name, e => e.Unregister(condition));
        }
        public static SingleEvent<T> Unregister<T>(string name, Func<bool> condition, Func<T> parameter)
        {
            return EventFunction<SingleEvent<T>>(name, e => e.Unregister(condition, parameter));
        }

        public static SingleEvent Subscribe(string name, params Action[] actions)
        {
            return EventFunction<SingleEvent>(name, e => e.Subscribe(actions));
        }
        public static SingleEvent<T> Subscribe<T>(string name, params Action<T>[] actions)
        {
            return EventFunction<SingleEvent<T>>(name, e => e.Subscribe(actions));
        }

        public static SingleEvent Unsubscribe(string name, params Action[] actions)
        {
            return EventFunction<SingleEvent>(name, e => e.Unsubscribe(actions));
        }
        public static SingleEvent<T> Unsubscribe<T>(string name, params Action<T>[] actions)
        {
            return EventFunction<SingleEvent<T>>(name, e => e.Unsubscribe(actions));
        }

        public static void RemoveEvent(string parent, string name)
        {
            EventGroup group = FindWithName<EventGroup>(parent);
            if (group == null) return;
            IEvent target = group.Events.Find(e => e.Name == name);
            if (target == null) return;
            group.RemoveEvent(target);
        }

        public static void EnableEvent(string name, bool enable, bool includeChildren = false)
        {
            IEvent target = FindWithName<IEvent>(name);
            if (target == null) return;
            target.SetEnable(enable, includeChildren);
        }


        private static T EventFilter<T>(IEvent e, Predicate<T> condition) where T : IEvent
        {
            if (e == null || condition == null) return null;
            if (e is T t && condition(t)) return t;
            if(e is EventGroup group)
            {
                foreach (var item in group.Events)
                {
                    var target = EventFilter(item, condition);
                    if (target != null) return target;
                }
            }
            return null;
        }

        private static void EventFilterAll<T>(IEvent e, Predicate<T> condition, List<T> container) where T: IEvent
        {
            if (e == null || container == null) return;
            if(e is T t)
            {
                if (condition == null)
                    container.Add(t);
                else if (condition(t))
                    container.Add(t);
            }
            if(e is EventGroup group)
            {
                foreach (var item in group.Events)
                {
                    EventFilterAll(item, condition, container);
                }
            }
        }

        private static T EventFunction<T>(string name, Action<T> action) where T:IEvent
        {
            T e = FindWithName<T>(name);
            if (e == null) return null;
            action(e);
            return e;
        }
    }
}
