public abstract class BaseScene
{
    public bool IsPreloadDone { get; protected set; }

    public void Load()
    {
        OnLoad();
    }

    public void Dispose()
    {
        IsPreloadDone = false;
        OnDispose();
    }

    public void Update()
    {
        OnUpdate();
    }

    public void FixedUpdate()
    {
        OnFixedUpdate();
    }

    public void Preload()
    {
        OnPreload();
    }

    protected virtual void OnLoad()
    {
    }

    protected virtual void OnDispose()
    {
    }

    protected virtual void OnUpdate()
    {
    }

    protected virtual void OnFixedUpdate()
    {
    }

    protected virtual void OnPreload()
    {
        IsPreloadDone = true;
    }

    protected virtual void InitSceneConf()
    {
    }
}