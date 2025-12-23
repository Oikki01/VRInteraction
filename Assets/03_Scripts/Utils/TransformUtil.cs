using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransformUtil
{
    public static string GetTransformFullPath(Transform trs)
    {
        string path = string.Empty;
        AddTrsPath(trs, trs.root, ref path);
        return path;
    }

    private static void AddTrsPath(Transform trs, Transform rootTrs, ref string path)
    {
        if (!trs.Equals(rootTrs))
        {
            path = path.Insert(0, "/" + trs.name);
            AddTrsPath(trs.parent, rootTrs, ref path);
        }
        else
        {
            path = path.Insert(0, trs.name);
        }
    }

    public static Bounds GetTrsBounds(MeshRenderer[] meshRenderers)
    {
        Bounds bounds = meshRenderers[0].bounds;
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            bounds.Encapsulate(meshRenderers[i].bounds);
        }

        return bounds;
    }

    public static Transform[] GetChildArr(Transform trs)
    {
        Transform[] array = new Transform[trs.childCount];
        for (int i = 0; i < trs.childCount; i++)
        {
            array[i] = trs.GetChild(i);
        }

        return array;
    }

    public static List<Transform> GetChildList(Transform trs)
    {
        return trs.GetComponentsInChildren<Transform>().ToList();
    }

    public static Material[] GetAllMeshRenderSharedMats(this GameObject game, Predicate<Material> predicate)
    {
        MeshRenderer[] componentsInChildren = game.GetComponentsInChildren<MeshRenderer>();
        HashSet<Material> hashSet = new HashSet<Material>();
        MeshRenderer[] array = componentsInChildren;
        for (int i = 0; i < array.Length; i++)
        {
            Material[] sharedMaterials = array[i].sharedMaterials;
            foreach (Material material in sharedMaterials)
            {
                if (predicate(material))
                {
                    hashSet.Add(material);
                }
            }
        }

        return new List<Material>(hashSet).ToArray();
    }
}