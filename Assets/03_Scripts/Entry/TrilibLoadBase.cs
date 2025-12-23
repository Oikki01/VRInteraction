using System;
using System.Collections;
using UnityEngine;

public abstract class TrilibLoadBase
{
    public abstract IEnumerator LoadGameObject(AssetInfo info, Action<GameObject, AssetInfo> action);
}