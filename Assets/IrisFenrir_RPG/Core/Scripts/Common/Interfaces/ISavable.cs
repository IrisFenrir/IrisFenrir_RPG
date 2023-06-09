namespace IrisFenrir.InputSystem
{
    public interface ISavable
    {
        Json Save();
        void Load(Json json);
    }
}
