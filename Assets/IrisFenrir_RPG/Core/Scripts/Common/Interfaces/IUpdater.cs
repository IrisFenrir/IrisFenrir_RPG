namespace IrisFenrir
{
    public interface IUpdater
    {
        void Init();

        void Update(float deltaTime);

        void Stop();
    }
}
