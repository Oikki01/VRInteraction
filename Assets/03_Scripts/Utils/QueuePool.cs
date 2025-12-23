using System.Collections.Generic;

public class QueuePool<T>
{
    public Queue<T> Queuepool;

    public QueuePool()
    {
        Queuepool = new Queue<T>();
    }

    public T Pop()
    {
        if (Queuepool.Count > 0)
        {
            return Queuepool.Dequeue();
        }

        return default(T);
    }

    public void Push(T t)
    {
        Queuepool.Enqueue(t);
    }
}