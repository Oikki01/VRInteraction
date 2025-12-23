using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraShootUtil
{
    public static Texture2D GetModelTexture(GameObject gObj, Rect rect)
    {
        gObj.SetLayer("ShotLayer");
        GameObject gameObject = CreateShotCamera();
        Texture2D result = null;
        MeshRenderer[] componentsInChildren = gObj.transform.GetComponentsInChildren<MeshRenderer>();
        if (componentsInChildren.Length != 0)
        {
            result = GetModelTextureByBounds(TransformUtil.GetTrsBounds(componentsInChildren), gameObject, rect);
        }

        Object.Destroy(gameObject);
        Object.Destroy(gObj);
        return result;
    }

    public static Texture2D GetModelTexture(Scene scene, Rect rect)
    {
        GameObject gameObject = CreateShotCamera();
        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        List<MeshRenderer> list = new List<MeshRenderer>();
        for (int i = 0; i < rootGameObjects.Length; i++)
        {
            rootGameObjects[i].SetLayer("ShotLayer");
            list.AddRange(rootGameObjects[i].transform.GetComponentsInChildren<MeshRenderer>());
        }

        Texture2D result = null;
        if (list.Count > 0)
        {
            result = GetModelTextureByBounds(TransformUtil.GetTrsBounds(list.ToArray()), gameObject, rect);
        }

        Object.Destroy(gameObject);
        SceneManager.UnloadSceneAsync(scene);
        return result;
    }

    public static Texture2D GetMaterialTexture(Material material, Rect rect)
    {
        GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("ShotCamera/MaterialSphere"));
        gameObject.GetComponent<MeshRenderer>().material = material;
        Material[] allMeshRenderSharedMats = gameObject.GetAllMeshRenderSharedMats((Material mat) => mat.shader.name.Contains("Standard"));
        foreach (Material obj in allMeshRenderSharedMats)
        {
            string name = obj.shader.name;
            obj.shader = Shader.Find(name);
            VEMToolUtil.SetMaterialRenderinMode(obj, (RenderingMode)obj.GetInt("_Mode"));
        }

        return GetModelTexture(gameObject, rect);
    }

    private static GameObject CreateShotCamera()
    {
        GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("ShotCamera/ShotCamera"));
        gameObject.GetComponent<Camera>().cullingMask = 65536;
        return gameObject;
    }

    public static Texture2D GetModelTextureByBounds(Bounds bounds, GameObject shotCameraGobj, Rect rect)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        Vector3 vector = min;
        vector.y += bounds.size.y;
        Vector3 vector2 = max;
        vector2.y -= bounds.size.y;
        Vector3 vector3 = vector;
        vector3.x += bounds.size.x;
        Vector3 vector4 = vector2;
        vector4.x -= bounds.size.x;
        Vector3 vector5 = Vector3.Cross(vector3 - vector4, min - max);
        Vector3 vector6 = Vector3.Cross(vector3 - vector4, vector - vector2);
        Vector3 vector7 = (vector5.normalized + vector6.normalized) * 0.55f;
        shotCameraGobj.transform.position = vector7 * Vector3.Distance(min, max) + bounds.center;
        if (shotCameraGobj.transform.position.y > 5000f)
        {
            Vector3 position = shotCameraGobj.transform.position;
            position.y = 5000f;
            shotCameraGobj.transform.position = position;
        }

        shotCameraGobj.transform.LookAt(bounds.center);
        return CaptureCamera(shotCameraGobj.GetComponent<Camera>(), rect);
    }

    public static Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        RenderTexture renderTexture = (camera.targetTexture = new RenderTexture((int)rect.width, (int)rect.height, 0));
        camera.enabled = false;
        camera.Render();
        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, mipChain: false);
        texture2D.ReadPixels(rect, 0, 0);
        texture2D.Apply();
        camera.targetTexture = null;
        RenderTexture.active = null;
        Object.Destroy(renderTexture);
        return texture2D;
    }
}