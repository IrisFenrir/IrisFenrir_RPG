namespace IrisFenrir.InputSystem
{
    public abstract class IKey: IEnable, ISavable
    {
        public bool Enable { get; protected set; }

        public virtual void SetEnable(bool enable, bool includeChildren = true)
        {
            Enable = enable;
        }

        public virtual bool GetEnable()
        {
            return Enable;
        }

        public abstract bool GetKeyPressing();

        public abstract bool GetKeyDown();

        public abstract bool GetKeyUp();

        public virtual Json Save()
        {
            Json json = new Json(Json.Type.Object);
            json["enable"] = Enable;
            return json;
        }

        public virtual void Load(Json json)
        {
            try
            {
                SetEnable(json["enable"]);
            }
            catch
            {
                ErrorLog.Log(ErrorSetting.jsonAnalysisError);
            }
        }
    }
}


