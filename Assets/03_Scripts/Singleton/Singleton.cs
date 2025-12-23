public class Singleton<T> where T : class, new()
{
    private static T instance;
    private static readonly object syslock = new object();
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syslock)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}