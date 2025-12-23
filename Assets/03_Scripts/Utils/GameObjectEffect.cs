using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public static class GameObjectEffect
{
    public static class Dissolve
    {
        private static Texture mdissolveSrc;

        private static Texture mdissolveSrcBump;

        public static void SetTextture(Texture dissolveSrc, Texture dissolveSrcBump)
        {
            mdissolveSrc = dissolveSrc;
            mdissolveSrcBump = dissolveSrcBump;
        }

        public static void RunDissolve(GameObject go, float Time, bool DissolveOrNormal, UnityAction callback)
        {
            Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
            if (mdissolveSrc == null || mdissolveSrcBump == null)
            {
                Texture dissolveSrc = Resources.Load<Texture>("Texture/ShaderTexture/Clouds");
                Texture dissolveSrcBump = Resources.Load<Texture>("Texture/ShaderTexture/Clouds_NRM");
                SetTextture(dissolveSrc, dissolveSrcBump);
            }

            Renderer[] array = componentsInChildren;
            for (int i = 0; i < array.Length; i++)
            {
                Material material = array[i].material;
                material.shader = Shader.Find("Dissolve/Dissolve_WorldCoords");
                if (mdissolveSrc != null)
                {
                    material.SetTexture("_DissolveSrc", mdissolveSrc);
                }

                if (mdissolveSrcBump != null)
                {
                    material.SetTexture("_DissolveSrcBump", mdissolveSrcBump);
                }

                material.SetFloat("_Amount", 0f);
                material.SetColor("_DissColor", new Color(0.64f, 0.4f, 0.4f));
                material.SetVector("_ColorAnimate", new Vector4(0f, 0f, 0f, 0f));
                if (DissolveOrNormal)
                {
                    material.DOFloat(0.5f, "_Amount", Time).SetEase(Ease.Linear).OnComplete(delegate
                    {
                        callback?.Invoke();
                    });
                }
                else
                {
                    material.DOFloat(0f, "_Amount", Time).SetEase(Ease.Linear).OnComplete(delegate
                    {
                        callback?.Invoke();
                    });
                }
            }
        }
    }

    public static class HalfGlass
    {
        private const string ShaderName = "Custom/Aphla3";

        public static void RunHalf(GameObject go, UnityAction callback, float alpha = 0.3f)
        {
            Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                Material material = componentsInChildren[i].material;
                if (!(material.shader.name == "Custom/Aphla3"))
                {
                    material.shader = Shader.Find("Custom/Aphla3");
                    material.SetFloat("_AlphaScale", alpha);
                }
            }

            callback?.Invoke();
        }
    }

    public static class HalfGlassNormal
    {
        private const string ShaderName = "Custom/NewAphla";

        public static void RunHalf(GameObject go, UnityAction callback, float alpha = 0f)
        {
            Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                Material[] materials = componentsInChildren[i].materials;
                foreach (Material material in materials)
                {
                    if (!(material.shader.name == "Custom/NewAphla"))
                    {
                        material.shader = Shader.Find("Custom/NewAphla");
                        material.SetFloat("_AlphaScale", alpha);
                    }
                }
            }

            callback?.Invoke();
        }
    }

    private const string ShaderName = "Standard";

    public static void Restore(GameObject go, UnityAction callback)
    {
        Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            Material[] materials = componentsInChildren[i].materials;
            foreach (Material material in materials)
            {
                if (!(material.shader.name == "Standard"))
                {
                    material.shader = Shader.Find("Standard");
                }
            }
        }

        callback?.Invoke();
    }
}