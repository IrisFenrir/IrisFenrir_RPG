namespace IrisFenrir
{
    public interface IEnable
    {
        void SetEnable(bool enable, bool includeChildren = true);

        bool GetEnable();
    }
}
