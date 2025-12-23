using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using FullSerializer;

public class QuoteResourcesManager : MonoSingleton<QuoteResourcesManager>
{
    [HideInInspector]
    public List<SpecialEfficacyData> SpecialEfficacyDataList;

    [HideInInspector]
    public List<ModelScreenData> ModelScreenDataList;

    public const float defWidthHeight = 100f;

    public const float defScale = 0.001f;

    private string specialEfficacyDataJsonPath;

    private string ImageVideoQuoteDataFolderPath;

    private string ImageVideoQuoteDataJsonPath;

    private string ModelScreenDataJsonPath;

    private Dictionary<string, QuoteImageData> imageVideoQuoteDict;

    [SerializeField]
    private GameObject canvasPrefab;

    private Dictionary<int, GameObject> screenCanvasDict = new Dictionary<int, GameObject>();

    public void InitData(string dataPath, GameObject canvasPrefab)
    {
        string text = dataPath;
        this.canvasPrefab = canvasPrefab;
        FileIOUtil.CreateFolder(text, "QuoteResources");
        text += "/QuoteResources";
        FileIOUtil.CreateFolder(text, "SpecialEfficacy");
        specialEfficacyDataJsonPath = text + "/SpecialEfficacy/SpecialEfficacyData.json";
        InitSpecialEfficacyData(specialEfficacyDataJsonPath);
        FileIOUtil.CreateFolder(text, "ModelScreen");
        ImageVideoQuoteDataFolderPath = text + "/ModelScreen";
        ImageVideoQuoteDataJsonPath = ImageVideoQuoteDataFolderPath + "/ImageVideoQuote.json";
        ModelScreenDataJsonPath = ImageVideoQuoteDataFolderPath + "/ModelScreenData.json";
        InitImageQuoteData(ImageVideoQuoteDataJsonPath);
        InitModelScreenData(ModelScreenDataJsonPath);
        LoadModelScreen();
    }

    public void SaveData()
    {
        SaveSpecialEfficacyData(specialEfficacyDataJsonPath);
        SaveImageQuoteData(ImageVideoQuoteDataJsonPath);
        SaveModelScreenData(ModelScreenDataJsonPath);
    }

    public QuoteImageData GetQuoteImageData(string imageName)
    {
        if (!imageVideoQuoteDict.ContainsKey(imageName))
        {
            return null;
        }

        return imageVideoQuoteDict[imageName];
    }

    public void AddImageVideoQuote(string fileName, string filePath)
    {
        if (!imageVideoQuoteDict.ContainsKey(fileName))
        {
            AddImageVideoFile(filePath);
            QuoteImageData value = new QuoteImageData(fileName, 1);
            imageVideoQuoteDict.Add(fileName, value);
        }
        else
        {
            imageVideoQuoteDict[fileName].QuoteCount++;
        }

        SaveImageQuoteData(ImageVideoQuoteDataJsonPath);
    }

    public void RemoveImageVideoQuote(string fileName)
    {
        if (imageVideoQuoteDict.ContainsKey(fileName))
        {
            if (imageVideoQuoteDict[fileName].QuoteCount <= 1)
            {
                DeleteImageVideoFile(fileName);
                imageVideoQuoteDict.Remove(fileName);
            }
            else
            {
                imageVideoQuoteDict[fileName].QuoteCount--;
            }

            SaveImageQuoteData(ImageVideoQuoteDataJsonPath);
        }
    }

    public string GetImageVideoFolderPath()
    {
        return ImageVideoQuoteDataFolderPath;
    }

    public ModelScreenData GetModelScreenData(int id)
    {
        return ModelScreenDataList.Find((ModelScreenData x) => x.Id.Equals(id));
    }

    public void LoadModelScreen()
    {
        screenCanvasDict.Clear();
        for (int i = 0; i < ModelScreenDataList.Count; i++)
        {
            CreateModelScreen(ModelScreenDataList[i]);
        }
    }

    public GameObject CreateModelScreen(ModelScreenData modelScreenData)
    {
        GameObject gameObject = Object.Instantiate(canvasPrefab);
        gameObject.name = "ModelScreenCanvas_" + modelScreenData.Id;
        ModifyParent(gameObject, modelScreenData);
        screenCanvasDict.Add(modelScreenData.Id, gameObject);
        return gameObject;
    }

    public GameObject GetScreenCanvas(int id)
    {
        if (screenCanvasDict.ContainsKey(id))
        {
            return screenCanvasDict[id];
        }

        return null;
    }

    public bool RemoveScreenCanvas(int id)
    {
        if (screenCanvasDict.ContainsKey(id))
        {
            Object.Destroy(screenCanvasDict[id]);
            screenCanvasDict.Remove(id);
            return true;
        }

        return false;
    }

    public void ModifyParent(GameObject screenCanvas, ModelScreenData modelScreenData, bool isModifyParent = false)
    {
        if (!string.IsNullOrEmpty(modelScreenData.ParentTrs))
        {
            GameObject gameObject = GameObject.Find(modelScreenData.ParentTrs);
            screenCanvas.transform.SetParent(gameObject.transform);
            if (isModifyParent)
            {
                screenCanvas.transform.position = Vector3.zero;
                screenCanvas.transform.eulerAngles = Vector3.zero;
                modelScreenData.Pos = Vector3.zero;
                modelScreenData.Rot = Vector3.zero;
                modelScreenData.Width = 100f;
                modelScreenData.Height = 100f;
            }

            screenCanvas.transform.localPosition = modelScreenData.Pos;
            screenCanvas.transform.localEulerAngles = modelScreenData.Rot;
            Vector3 localScale = Vector3.one * 0.001f;
            localScale.x = 0.001f * modelScreenData.Width / 100f;
            localScale.y = 0.001f * modelScreenData.Height / 100f;
            screenCanvas.transform.localScale = localScale;
        }
    }

    private void InitSpecialEfficacyData(string dataPath)
    {
        SpecialEfficacyDataList = JsonFullSerializer.LoadJsonFile<List<SpecialEfficacyData>>(dataPath);
        if (SpecialEfficacyDataList == null)
        {
            SpecialEfficacyDataList = new List<SpecialEfficacyData>();
        }
    }

    private void SaveSpecialEfficacyData(string dataPath)
    {
        JsonFullSerializer.SaveJsonFile(dataPath, SpecialEfficacyDataList);
    }

    private void DeleteImageVideoFile(string fileName)
    {
        FileIOUtil.DeleteFlle(FileAttributes.Archive, ImageVideoQuoteDataFolderPath + "/" + fileName);
    }

    private void AddImageVideoFile(string filePath)
    {
        FileIOUtil.CopyToFile(filePath, ImageVideoQuoteDataFolderPath);
    }

    private void InitImageQuoteData(string dataPath)
    {
        imageVideoQuoteDict = JsonFullSerializer.LoadJsonFile<Dictionary<string, QuoteImageData>>(dataPath);
        if (imageVideoQuoteDict == null)
        {
            imageVideoQuoteDict = new Dictionary<string, QuoteImageData>();
        }
    }

    private void SaveImageQuoteData(string dataPath)
    {
        JsonFullSerializer.SaveJsonFile(dataPath, imageVideoQuoteDict);
    }

    private void InitModelScreenData(string dataPath)
    {
        ModelScreenDataList = JsonFullSerializer.LoadJsonFile<List<ModelScreenData>>(dataPath);
        if (ModelScreenDataList == null)
        {
            ModelScreenDataList = new List<ModelScreenData>();
        }
    }

    private void SaveModelScreenData(string dataPath)
    {
        JsonFullSerializer.SaveJsonFile(dataPath, ModelScreenDataList);
    }
}