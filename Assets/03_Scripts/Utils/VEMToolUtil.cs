using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VEMToolUtil
{
    public static List<T> PraseList<T>(object listObj)
    {
        List<T> list = new List<T>();
        if (!listObj.GetType().IsGenericType)
        {
            throw new Exception("非集合类型");
        }

        if (listObj is ICollection)
        {
            ICollection collection = (ICollection)listObj;
            if (collection.Count > 0)
            {
                foreach (object item in collection)
                {
                    list.Add((T)item);
                }
            }
        }

        return list;
    }

    public static string ToTime(int second)
    {
        string text = null;
        if (second <= 59)
        {
            return $"{second}秒";
        }

        int num = second / 60;
        int num2 = second % 60;
        return $"{num}分{num2}秒";
    }

    public static void CalculationTimeDiff(ref int hours, ref int minutes, ref int seconds, DateTime oldTime, DateTime newTime)
    {
        hours = (newTime - oldTime).Hours;
        minutes = (newTime - oldTime).Minutes;
        seconds = (newTime - oldTime).Seconds;
    }

    public static double CalculationTimeLeagth(DateTime oldTime, DateTime newTime)
    {
        long ticks = oldTime.Ticks;
        long ticks2 = newTime.Ticks;
        return new TimeSpan(ticks2 - ticks).TotalSeconds;
    }

    public static void SetMaterialRenderinMode(Material material, RenderingMode mode)
    {
        switch (mode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", 1);
                material.SetInt("_DstBlend", 0);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHATBLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", 1);
                material.SetInt("_DstBlend", 0);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHATBLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", 5);
                material.SetInt("_DstBlend", 10);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHATBLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", 1);
                material.SetInt("_DstBlend", 10);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHATBLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                material.color = new Color(1f, 1f, 1f, 0.65f);
                break;
        }
    }

    public static void SetCursor(Texture sp, Vector2 hostpot, CursorMode mode = CursorMode.Auto)
    {
        Cursor.SetCursor((Texture2D)sp, hostpot, mode);
    }

    public static List<int> GetRandomNumList(int begin, int end, int count)
    {
        List<int> list = new List<int>();
        List<int> list2 = new List<int>();
        for (int i = begin; i < end; i++)
        {
            list2.Add(i);
        }

        for (int j = 0; j < count; j++)
        {
            int index = UnityEngine.Random.Range(0, list2.Count);
            list.Add(list2[index]);
            list2.Remove(list2[index]);
        }

        return list;
    }
}