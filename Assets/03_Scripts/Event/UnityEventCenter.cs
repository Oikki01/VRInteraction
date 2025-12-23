using UnityEngine.Events;

namespace GameEvent
{
    public class UnityEventCenter : UnityEvent
    {
    }
    
    public class UnityEventCenter<T> : UnityEvent<T>
    {
    }
    
    public class UnityEventCenter<T,T2> : UnityEvent<T,T2>
    {
    }
    
    public class UnityEventCenter<T,T2,T3> : UnityEvent<T,T2,T3>
    {
    }
    
    public class UnityEventCenter<T,T2,T3,T4> : UnityEvent<T,T2,T3,T4>
    {
    }
}

