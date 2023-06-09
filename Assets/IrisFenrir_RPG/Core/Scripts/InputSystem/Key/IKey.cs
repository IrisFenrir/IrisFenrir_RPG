namespace IrisFenrir.InputSystem
{
    public abstract class IKey: IEnable, ISavable
    {
        public bool enable { get; protected set; }

        public virtual void SetEnable(bool enable, bool includeChildren = true)
        {
            this.enable = enable;
        }

        public virtual bool GetEnable()
        {
            return enable;
        }

        public abstract bool GetKeyPressing();

        public abstract bool GetKeyDown();

        public abstract bool GetKeyUp();

        public virtual Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["enable"] = enable;
            return json;
        }

        public virtual void Load(Json json)
        {
            SetEnable(json["enable"]);
        }
    }
}


