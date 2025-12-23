using System;

public static class FacadeScene
{
    public static Action<string> OnSceneLoadFinish;

    public static Action<float> ABSceneLoadProgress;
}