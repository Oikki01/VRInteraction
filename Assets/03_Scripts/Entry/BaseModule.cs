public abstract class BaseModule : ILoad, IDispose
{
    public void Load()
    {
        OnLoad();
    }

    public void Update()
    {
        OnUpdate();
    }

    public void FixedUpdate()
    {
        OnFixedUpdate();
    }

    public void LateUpdate()
    {
        OnLateUpdate();
    }

    public void Dispose()
    {
        OnDispose();
    }

    protected virtual void OnLoad()
    {
    }

    protected virtual void OnUpdate()
    {
    }

    protected virtual void OnFixedUpdate()
    {
    }

    protected virtual void OnLateUpdate()
    {
    }

    protected virtual void OnDispose()
    {
    }
}