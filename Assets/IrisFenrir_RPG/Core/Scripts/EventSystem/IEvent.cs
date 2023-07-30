namespace IrisFenrir.EventSystem
{
    public abstract class IEvent : IUpdater, IEnable
    {
        public string Name { get; private set; }
        public bool Enable { get; private set; }

        public IEvent(string name)
        {
            Name = name;
        }

        public bool GetEnable()
        {
            return Enable;
        }

        public virtual void SetEnable(bool enable, bool includeChildren = true)
        {
            Enable = enable;
        }

        public virtual void Init() { }

        public virtual void Stop() { }

        public virtual void Update(float deltaTime) { }

        public override string ToString()
        {
            return GetType().Name + ": " + Name;
        }
    }
}
