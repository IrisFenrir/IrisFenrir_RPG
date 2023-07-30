using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public abstract class IVirutalKey : IUpdater, IEnable, ISavable
    {
        public string Name { get; set; }

        public bool Enable { get; protected set; }

        public virtual int KeyCount { get; } = 1;

        public virtual void SetEnable(bool enable, bool includeChildren = true)
        {
            Enable = enable;
        }

        public virtual bool GetEnable()
        {
            return Enable;
        }

        public virtual void Init() { }
        public virtual void Update(float deltaTime) { }
        public virtual void Stop() { }

        public virtual void SetKeyCode(KeyCode keyCode, int index = 0, int subIndex = 0) { }
        public virtual KeyCode GetKeyCode(int index = 0, int subIndex = 0) { return KeyCode.None; }

        public virtual void AddKey(IKey key, int index = 0) { }
        public virtual void RemoveKey(IKey key, int index = 0) { }

        public virtual void Load(Json json) { }
        public virtual Json Save() { return null; }
    }
}
