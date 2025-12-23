using System.Collections.Generic;

public class ModuleMgr : Singleton<ModuleMgr>, ILoad, IDispose
{
    private List<BaseModule> updateModList;

    private bool isLoaded;

    private SceneMod sceneMod;

    private ResourcesModule resourcesModule;

    public SceneMod SceneMod => sceneMod;

    public ResourcesModule ResourcesMod => resourcesModule;

    public void Load()
    {
        updateModList = new List<BaseModule>();
        sceneMod = new SceneMod();
        resourcesModule = new ResourcesModule();
    }

    public new void Dispose()
    {
        isLoaded = false;
        SceneMod.Dispose();
        resourcesModule.Dispose();
    }

    public void Start()
    {
        isLoaded = true;
        sceneMod.Load();
        resourcesModule.Load();
    }

    public void Update()
    {
        if (isLoaded)
        {
            for (int i = 0; i < updateModList.Count; i++)
            {
                updateModList[i].Update();
            }
        }
    }

    public void FixedUpdate()
    {
        if (isLoaded)
        {
            for (int i = 0; i < updateModList.Count; i++)
            {
                updateModList[i].FixedUpdate();
            }
        }
    }

    public void LateUpdate()
    {
    }
}