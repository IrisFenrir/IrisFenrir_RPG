using UnityEngine;

namespace IrisFenrir.InputSystem
{
    public abstract class IVirutalKey : IUpdater, IEnable, ISavable
    {
        public string name { get; set; }

        public bool enable { get; protected set; }

        public virtual void SetEnable(bool enable, bool includeChildren = true)
        {
            this.enable = enable;
        }

        public virtual bool GetEnable()
        {
            return enable;
        }

        public virtual void Init() { }
        public virtual void Update(float deltaTime) { }
        public virtual void Stop() { }

        public virtual void SetKeyCode(KeyCode keyCode, int index = 0) { }
        public virtual KeyCode GetKeyCode(int index = 0) { return KeyCode.None; }

        public virtual void AddKey(IKey key, int index = 0) { }
        public virtual void RemoveKey(IKey key, int index = 0) { }

        public virtual void Load(Json json) { }
        public virtual Json Save() { return null; }
    }
}
