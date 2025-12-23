using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class GameObjectUtil
{
    public static T GetOrCreatComponent<T>(this GameObject obj) where T : MonoBehaviour
    {
        T val = obj.GetComponent<T>();
        if (val == null)
        {
            val = obj.AddComponent<T>();
        }

        return val;
    }

    public static T GetOrCreatComponent<T>(this Transform obj) where T : MonoBehaviour
    {
        T val = obj.GetComponent<T>();
        if (val == null)
        {
            val = obj.gameObject.AddComponent<T>();
        }

        return val;
    }

    public static void SetParent(this GameObject obj, Transform parent)
    {
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localEulerAngles = Vector3.zero;
    }

    public static void SetLayer(this GameObject obj, string layerName)
    {
        Transform[] componentsInChildren = obj.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    public static void SetLayer(this GameObject obj, int layer)
    {
        Transform[] componentsInChildren = obj.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].gameObject.layer = layer;
        }
    }

    public static void SetNull(this MonoBehaviour[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }

            arr = null;
        }
    }

    public static void SetNull(this Transform[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }

            arr = null;
        }
    }

    public static void SetNull(this Sprite[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }

            arr = null;
        }
    }

    public static void SetNull(this GameObject[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }

            arr = null;
        }
    }

    public static void SetText(this Text txtObj, string text, bool isAnimation = false, float duration = 0.2f, ScrambleMode scrambleMode = ScrambleMode.None)
    {
        if (txtObj != null)
        {
            if (isAnimation)
            {
                txtObj.text = "";
                txtObj.DOText(text, duration, richTextEnabled: true, scrambleMode);
            }
            else
            {
                txtObj.text = text;
            }
        }
    }

    public static void SetSliderValue(this Slider sliderObj, float value)
    {
        if (sliderObj != null)
        {
            sliderObj.value = value;
        }
    }

    public static void SetImage(this Image imgObj, Sprite sprite)
    {
        if (imgObj != null)
        {
            imgObj.overrideSprite = sprite;
        }
    }

    public static void SetHideOrShow(this RectTransform rect, Direction dir, float distance, UnityAction moveCall = null)
    {
        switch (dir)
        {
            case Direction.ToUp:
                rect.DOLocalMoveY(distance, 0.85f).SetEase(Ease.OutCubic).OnComplete(delegate
                {
                    if (moveCall != null)
                    {
                        moveCall();
                    }
                });
                break;
            case Direction.ToDown:
                rect.DOLocalMoveY(distance, 0.85f).SetEase(Ease.InCubic).OnComplete(delegate
                {
                    if (moveCall != null)
                    {
                        moveCall();
                    }
                });
                break;
            case Direction.ToLeft:
                rect.DOLocalMoveX(distance, 0.85f).SetEase(Ease.OutCubic).OnComplete(delegate
                {
                    if (moveCall != null)
                    {
                        moveCall();
                    }
                });
                break;
            case Direction.ToRight:
                rect.DOLocalMoveX(distance, 0.85f).SetEase(Ease.InCubic).OnComplete(delegate
                {
                    if (moveCall != null)
                    {
                        moveCall();
                    }
                });
                break;
        }
    }
}