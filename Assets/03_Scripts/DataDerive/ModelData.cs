using UnityEngine;

public class ModelData
{
    public string ModelName;

    public Vector3 ModelInitPos;

    public Vector3 ModelInitRot;

    public ModelData()
    {
    }

    public ModelData(string modelName)
    {
        ModelName = modelName;
        ModelInitPos = Vector3.zero;
        ModelInitRot = Vector3.zero;
    }

    public ModelData(string modelName, Vector3 modelInitPos, Vector3 modelInitRot)
    {
        ModelName = modelName;
        ModelInitPos = modelInitPos;
        ModelInitRot = modelInitRot;
    }
}