namespace IrisFenrir.EventSystem
{
    public abstract class IEvent : IUpdater, IEnable
    {
        public string name { get; private set; }
        public bool enable { get; private set; }

        public IEvent(string name)
        {
            this.name = name;
        }

        public bool GetEnable()
        {
            return enable;
        }

        public virtual void SetEnable(bool enable, bool includeChildren = true)
        {
            this.enable = enable;
        }

        public virtual void Init() { }

        public virtual void Stop() { }

        public virtual void Update(float deltaTime) { }

        public override string ToString()
        {
            return GetType().Name + ": " + name;
        }
    }
}
