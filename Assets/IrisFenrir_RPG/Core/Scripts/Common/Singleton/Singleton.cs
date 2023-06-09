namespace IrisFenrir
{
    public class Singleton<T> where T:Singleton<T>,new()
    {
        private static T m_instance;

        public static T instance
        {
            get
            {
                m_instance ??= new T();
                return m_instance;
            }
        }
    }
}
