using System.Collections.Generic;

namespace IrisFenrir.EventSystem
{
    public class EventGroup : IEvent
    {
        public List<IEvent> events { get; private set; }

        public EventGroup(string name) : base(name)
        {
            events = new List<IEvent>();
        }

        public void AddEvent(IEvent e)
        {
            if (e == null) return;
            events.Add(e);
        }

        public bool RemoveEvent(IEvent e)
        {
            if (e == null) return false;
            return events.Remove(e);
        }

        public SingleEvent CreateEvent(string name)
        {
            IEvent target = events.Find(e => e.name == name);
            if (target != null) return target as SingleEvent;
            SingleEvent e = new SingleEvent(name);
            AddEvent(e);
            return e;
        }
        public SingleEvent<T> CreateEvent<T>(string name)
        {
            IEvent target = events.Find(e => e.name == name);
            if (target != null) return target as SingleEvent<T>;
            SingleEvent<T> e = new SingleEvent<T>(name);
            AddEvent(e);
            return e;
        }
        public SingleEvent[] CreateEvents(params string[] names)
        {
            SingleEvent[] events = new SingleEvent[name.Length];
            for (int i = 0; i < names.Length; i++)
            {
                events[i] = CreateEvent(names[i]);
            }
            return events;
        }
        public SingleEvent<T>[] CreateEvents<T>(params string[] names)
        {
            SingleEvent<T>[] events = new SingleEvent<T>[name.Length];
            for (int i = 0; i < names.Length; i++)
            {
                events[i] = CreateEvent<T>(names[i]);
            }
            return events;
        }

        public EventGroup CreateEventGroup(string name)
        {
            IEvent target = events.Find(e => e.name == name);
            if (target != null) return target as EventGroup;
            EventGroup e = new EventGroup(name);
            AddEvent(e);
            return e;
        }
        public EventGroup[] CreateEventGroups(params string[] names)
        {
            EventGroup[] events = new EventGroup[name.Length];
            for (int i = 0; i < names.Length; i++)
            {
                events[i] = CreateEventGroup(names[i]);
            }
            return events;
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable, includeChildren);
            if (includeChildren)
                events.ForEach(e => e.SetEnable(enable, includeChildren));
        }

        public override void Update(float deltaTime)
        {
            if (!enable) return;
            events.ForEach(e => e.Update(deltaTime));
        }
    }
}
