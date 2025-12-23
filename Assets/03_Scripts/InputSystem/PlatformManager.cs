using UnityEngine;
using UnityEngine.XR;

public class PlatformManager : Singleton<PlatformManager>
{
    public EuipmentType PlatformEuipment { get; private set; }

    public InputSystemBase input { get; private set; }

    //public BodyCarryOut body { get; private set; }

    public void SetEuipmentType(EuipmentType euipment)
    {
        if (PlatformEuipment != euipment)
        {
            PlatformEuipment = euipment;
        }
    }

    public void InitInputsystem(EuipmentType euipment)
    {
        //switch (euipment)
        //{
        //    case EuipmentType.PC:
        //        {
        //            //SetEuipmentType(EuipmentType.PC);
        //            //GameObject gameObject2 = new GameObject("PCSystem");
        //            //input = gameObject2.AddComponent<PCInputSystem>();
        //            //(input as PCInputSystem).PCInput = PCInput.Winodws;
        //            //XRSettings.enabled = false;
        //            //break;
        //        }
        //    case EuipmentType.VR:
        //        {
        //            //SetEuipmentType(EuipmentType.VR);
        //            //GameObject gameObject = new GameObject("VRSystem");
        //            //VEMSim_VR_EventSystem vEMSim_VR_EventSystem = gameObject.AddComponent<VEMSim_VR_EventSystem>();
        //            //input = vEMSim_VR_EventSystem.VrInputModule;
        //            //CheckVRPlatformEuipment((VRInputSystem)input);
        //            //break;
        //        }
        //}
    }

    public void InitCtrl()
    {
        //this.body = body;
    }

    public void CheckVRPlatformEuipment()
    {
        //Debug.Log("UnityEngine.XR.XRSettings.loadedDeviceName = " + XRSettings.loadedDeviceName);
        //string loadedDeviceName = XRSettings.loadedDeviceName;
        //string text = loadedDeviceName;
        //string text2 = text;
        //if (!(text2 == "OpenVR"))
        //{
        //    if (text2 == "Oculus")
        //    {
        //        input.VRInput = VRInput.Occlus;
        //    }
        //    else
        //    {
        //        Debug.Log("No VR");
        //    }
        //}
        //else
        //{
        //    input.VRInput = VRInput.SteamVR;
        //}

        //XRSettings.enabled = true;
    }
}